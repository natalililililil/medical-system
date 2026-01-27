using AuthService.Api.Middleware;
using AuthService.Application.Accounts.Commands.ConfirmEmail;
using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Application.Common;
using AuthService.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterAccountValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ConfirmEmailValidator>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterAccountCommand).Assembly));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

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

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
