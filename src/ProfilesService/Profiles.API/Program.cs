using MedicalSystem.Shared.Behaviors;
using MedicalSystem.Shared.Interfaces;
using MedicalSystem.Shared.Middleware;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.Commands.Doctor.Create;
using Profiles.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProfilesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProfilesDb")));

builder.Services.AddScoped<IProfilesDbContext>(provider => provider.GetRequiredService<ProfilesDbContext>());
builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<ProfilesDbContext>());

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateDoctorCommand).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

builder.Services.AddOpenApi();

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