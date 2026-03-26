using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Entities.Accounts;
using AuthService.Domain.Entities.Outbox;
using AuthService.Domain.Entities.Tokens;
using MediatR;
using MedicalSystem.Shared.Contracts.Events;
using MedicalSystem.Shared.Enums;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AuthService.Application.Accounts.Commands;

public class RegisterAccountHandler(IAuthDbContext _context, ILogger<RegisterAccountHandler> _logger) : IRequestHandler<RegisterAccountCommand, Unit>
{
    public async Task<Unit> Handle(RegisterAccountCommand request, CancellationToken ct)
    {
        if (await _context.Accounts.FirstOrDefaultAsync(a => a.Email == request.Email, ct) != null)
        {
            _logger.LogWarning("Attempt to register with an email that already exists: {Email}", request.Email);
            throw new ConflictException("EMAIL_ALREADY_CONFIRMED", "An account with this email already exists");
        }

        if (!Enum.TryParse<Role>(request.Role, true, out var parsedRole))
        {
            parsedRole = Role.Patient;
            _logger.LogDebug("Invalid or missing role '{Role}' for {Email}. Defaulted to Patient", request.Role, request.Email);
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var account = new Account(request.Email, passwordHash, parsedRole);

        await _context.Accounts.AddAsync(account, ct);

        var emailToken = new EmailConfirmationToken(account.Id);
        await _context.EmailConfirmationTokens.AddAsync(emailToken, ct);
        Console.WriteLine($"Email confirmation link: https://localhost:5173/confirm-email?token={emailToken.Token}");

        var outboxMessage = new OutboxMessage(
            type: "account-created",
            content: JsonSerializer.Serialize(new AccountCreatedEvent
            {
                AccountId = account.Id,
                Role = account.Role
            }) 
        );

        _context.OutboxMessages.Add(outboxMessage);

        _logger.LogInformation("Account {Email} saved to DB with Outbox message.", request.Email);
        return Unit.Value;
    }
}