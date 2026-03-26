using Confluent.Kafka;
using MedicalSystem.Shared.Contracts.Events;
using MedicalSystem.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;
using System.Text.Json;

namespace Profiles.Application.Services;

public class AccountCreatedConsumerService(IServiceScopeFactory _scopeFactory, ILogger<AccountCreatedConsumerService> _logger) : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = "profiles-consumer-group",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<Null, string>(config).Build();
        consumer.Subscribe("account-created");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);

                using var scope = _scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<IProfilesDbContext>();

                var data = JsonSerializer.Deserialize<AccountCreatedEvent>(consumeResult.Message.Value);

                if (data == null) 
                    continue;

                _logger.LogInformation("Received AccountCreatedEvent for AccountId: {AccountId}, Role: {Role}", data.AccountId, data.Role);

                if (await ProfileExists(dbContext, data, stoppingToken))
                {
                    _logger.LogInformation("Profile already exists for AccountId: {AccountId}", data.AccountId);
                    consumer.Commit(consumeResult);
                    continue;
                }

                CreateProfile(dbContext, data);

                consumer.Commit(consumeResult);

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming message from Kafka");
            }
        }
    }

    private async Task<bool> ProfileExists(IProfilesDbContext dbContext, AccountCreatedEvent data, CancellationToken ct)
    {
        return data.Role switch
        {
            Role.Patient => await dbContext.PatientProfiles
                .AnyAsync(p => p.AccountId == data.AccountId, ct),

            Role.Doctor => await dbContext.DoctorProfiles
                .AnyAsync(p => p.AccountId == data.AccountId, ct),

            Role.Receptionist => await dbContext.ReceptionistProfiles
                .AnyAsync(p => p.AccountId == data.AccountId, ct),

            _ => false
        };
    }

    private void CreateProfile(IProfilesDbContext dbContext, AccountCreatedEvent data)
    {
        switch (data.Role)
        {
            case Role.Patient:
                dbContext.PatientProfiles.Add(new PatientProfile(
                    data.AccountId, "", "", null, DateTime.MinValue, null, null));
                break;

            case Role.Doctor:
                dbContext.DoctorProfiles.Add(new DoctorProfile(
                    data.AccountId, "", "", null, DateTime.MinValue,
                    DateTime.UtcNow.Year, null, Guid.Empty, Guid.Empty));
                break;

            case Role.Receptionist:
                dbContext.ReceptionistProfiles.Add(new ReceptionistProfile(
                    data.AccountId, "", "", null, Guid.Empty, null));
                break;

            default:
                _logger.LogWarning("Unknown role: {Role}", data.Role);
                break;
        }
    }
}