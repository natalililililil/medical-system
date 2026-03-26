using AuthService.Api.Services.Cookies;
using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Application.Common;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using Confluent.Kafka;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MedicalSystem.Shared.Behaviors;
using MedicalSystem.Shared.Interfaces;
using MedicalSystem.Shared.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDb")));

builder.Services.AddScoped<IAuthDbContext>(x => x.GetRequiredService<AuthDbContext>());
builder.Services.AddScoped<IAppDbContext>(x => x.GetRequiredService<AuthDbContext>());

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssembly(typeof(RegisterAccountValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<TokenCookieService>();
builder.Services.AddScoped<ITokenCookieService>(x => x.GetRequiredService<TokenCookieService>());
builder.Services.AddScoped<IAuthCookieCleaner>(x => x.GetRequiredService<TokenCookieService>());

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterAccountCommand).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins("https://localhost:5173")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var jwtSection = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSection["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Request.Cookies.TryGetValue("accessToken", out var accessToken);

            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("AuthPolicy", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit = 10;
    });

    options.AddFixedWindowLimiter("RefreshTokenPolicy", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit = 60;
    });
});

var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

builder.Services.AddSingleton<IProducer<Null, string>>(sp => new ProducerBuilder<Null, string>(config).Build());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();