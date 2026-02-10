using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands.ConfirmEmail;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Unit>
{
    private readonly IEmailConfirmationTokenRepository _tokenRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<ConfirmEmailHandler> _logger;

    public ConfirmEmailHandler(IEmailConfirmationTokenRepository tokenRepository, IAccountRepository accountRepository, 
        ILogger<ConfirmEmailHandler> logger)
    {
        _tokenRepository = tokenRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        var emailToken = await _tokenRepository.GetByTokenAsync(request.Token, ct);

        if (emailToken == null || emailToken.IsUsed || emailToken.IsExpired)
        {
            _logger.LogWarning("Invalid or expired email confirmation token");
            throw new InvalidOperationException("Invalid or expired token");
        }    

        emailToken.Use();

        var account = await _accountRepository.GetByIdAsync(emailToken.AccountId, ct);
        account.ConfirmEmail();

        await _accountRepository.SaveAsync(ct);
        await _tokenRepository.SaveAsync(ct);

        _logger.LogInformation("Email confirmed for account {AccountId}", account.Id);
        return Unit.Value;
    }
}