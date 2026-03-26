using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Accounts;
using AuthService.Domain.Tokens;
using Confluent.Kafka;
using MediatR;
using MedicalSystem.Shared.Contracts.Events;
using MedicalSystem.Shared.Enums;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AuthService.Application.Accounts.Commands;

public class RegisterAccountHandler(IAuthDbContext context, ILogger<RegisterAccountHandler> logger, 
    IProducer<Null, string> _kafkaProducer) : IRequestHandler<RegisterAccountCommand, Unit>
{
    public async Task<Unit> Handle(RegisterAccountCommand request, CancellationToken ct)
    {
        if (await context.Accounts.FirstOrDefaultAsync(a => a.Email == request.Email, ct) != null)
        {
            logger.LogWarning("Attempt to register with an email that already exists: {Email}", request.Email);
            throw new ConflictException("EMAIL_ALREADY_CONFIRMED", "An account with this email already exists");
        }

        if (!Enum.TryParse<Role>(request.Role, true, out var parsedRole))
        {
            parsedRole = Role.Patient;
            logger.LogDebug("Invalid or missing role '{Role}' for {Email}. Defaulted to Patient", request.Role, request.Email);
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var account = new Account(request.Email, passwordHash, parsedRole);

        await context.Accounts.AddAsync(account, ct);

        var emailToken = new EmailConfirmationToken(account.Id);
        await context.EmailConfirmationTokens.AddAsync(emailToken, ct);

        Console.WriteLine($"Email confirmation link: https://localhost:5173/confirm-email?token={emailToken.Token}");

        logger.LogInformation("New account registered with email: {Email}", request.Email);

        var accountCreatedEvent = new AccountCreatedEvent
        {
            AccountId = account.Id,
            Role = parsedRole
        };

        await _kafkaProducer.ProduceAsync(
            "account-created",
            new Message<Null, string> { Value = JsonSerializer.Serialize(accountCreatedEvent) }
        );
        return Unit.Value;
    }
}