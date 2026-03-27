using FluentValidation;
using MedicalSystem.Shared.Behaviors;
using MedicalSystem.Shared.Interfaces;
using MedicalSystem.Shared.Middleware;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.Commands.Doctor.Create;
using Profiles.Infrastructure.MessageBroker;
using Profiles.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProfilesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProfilesDb")));

builder.Services.AddScoped<IProfilesDbContext>(provider => provider.GetRequiredService<ProfilesDbContext>());
builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<ProfilesDbContext>());

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssembly(typeof(CreateDoctorValidator).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateDoctorCommand).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

builder.Services.AddOpenApi();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("ReadPolicy", opt =>
    {
        opt.PermitLimit = 60;
        opt.Window = TimeSpan.FromMinutes(1);
    });

    options.AddFixedWindowLimiter("WritePolicy", opt =>
    {
        opt.PermitLimit = 20;
        opt.Window = TimeSpan.FromMinutes(1);
    });
});

builder.Services.AddHostedService<AccountCreatedConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();