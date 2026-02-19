using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Application.Common.Exceptions;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Tokens;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands;

public class RegisterAccountHandler(IAuthDbContext context, ILogger<RegisterAccountHandler> logger) : IRequestHandler<RegisterAccountCommand, Unit>
{
    public async Task<Unit> Handle(RegisterAccountCommand request, CancellationToken ct)
    {
        if (await context.Accounts.FirstOrDefaultAsync(a => a.Email == request.Email, ct) != null)
        {
            logger.LogWarning("Attempt to register with an email that already exists: {Email}", request.Email);
            throw new ConflictException("EMAIL_ALREADY_CONFIRMED", "An account with this email already exists");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var account = new Account(request.Email, passwordHash);

        await context.Accounts.AddAsync(account, ct);

        var emailToken = new EmailConfirmationToken(account.Id);
        await context.EmailConfirmationTokens.AddAsync(emailToken, ct);

        Console.WriteLine($"Email confirmation link: https://localhost:5173/confirm-email?token={emailToken.Token}");

        logger.LogInformation("New account registered with email: {Email}", request.Email);
        return Unit.Value;
    }
}