using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Tokens;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands;

public class RegisterAccountHandler : IRequestHandler<RegisterAccountCommand, Unit>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmailConfirmationTokenRepository _emailTokenRepository;
    private readonly ILogger<RegisterAccountHandler> _logger;
    public RegisterAccountHandler(IAccountRepository accountRepository, IEmailConfirmationTokenRepository emailTokenRepository, 
        ILogger<RegisterAccountHandler> logger)
    {
        _accountRepository = accountRepository;
        _emailTokenRepository = emailTokenRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        if (await _accountRepository.GetByEmailAsync(request.Email, cancellationToken) != null)
        {
            _logger.LogWarning("Attempt to register with an email that already exists: {Email}", request.Email);
            throw new InvalidOperationException("An account with this email already exists");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var account = new Account(request.Email, passwordHash);

        await _accountRepository.AddAsync(account);
        await _accountRepository.SaveAsync(cancellationToken);

        var emailToken = new EmailConfirmationToken(account.Id);
        await _emailTokenRepository.AddAsync(emailToken);
        await _emailTokenRepository.SaveAsync(cancellationToken);

        Console.WriteLine($"Email confirmation link: http://localhost:5173/confirm-email?token={emailToken.Token}");

        _logger.LogInformation("New account registered with email: {Email}", request.Email);
        return Unit.Value;
    }
}