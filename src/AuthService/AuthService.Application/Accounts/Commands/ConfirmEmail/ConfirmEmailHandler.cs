using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.ConfirmEmail;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Unit>
{
    private readonly IEmailConfirmationTokenRepository _tokenRepository;
    private readonly IAccountRepository _accountRepository;

    public ConfirmEmailHandler(IEmailConfirmationTokenRepository tokenRepository, IAccountRepository accountRepository)
    {
        _tokenRepository = tokenRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        var emailToken = await _tokenRepository.GetByTokenAsync(request.Token, ct);

        if (emailToken == null || emailToken.IsUsed || emailToken.IsExpired)
            throw new InvalidOperationException("Invalid or expired token");

        emailToken.Use();

        var account = await _accountRepository.GetByIdAsync(emailToken.AccountId, ct);
        account.ConfirmEmail();

        await _accountRepository.SaveAsync(ct);
        await _tokenRepository.SaveAsync(ct);

        return Unit.Value;
    }
}