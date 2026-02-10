using AuthService.Application.Accounts.DTOs;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Accounts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands.RefreshTokenLogic;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthTokensResponse>
{
    private readonly IRefreshTokenRepository _refreshRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IJwtTokenService _jwt;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(IRefreshTokenRepository refreshRepo, IAccountRepository accountRepo, IJwtTokenService jwt, 
        ILogger<RefreshTokenHandler> logger)
    {
        _refreshRepository = refreshRepo;
        _accountRepository = accountRepo;
        _jwt = jwt;
        _logger = logger;
    }

    public async Task<AuthTokensResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var refresh = await _refreshRepository.GetByTokenAsync(request.RefreshToken, ct);

        if (refresh == null || !refresh.IsActive)
        {
            _logger.LogWarning("Invalid refresh token used");
            throw new InvalidOperationException("Invalid refresh token");
        }

        var account = await _accountRepository.GetByIdAsync(refresh.AccountId, ct);

        if (account == null)
            throw new InvalidOperationException("Account not found");

        refresh.Revoke();

        var newRefreshValue = _jwt.GenerateRefreshToken();
        var newRefresh = new RefreshToken(
            account.Id,
            newRefreshValue,
            DateTime.UtcNow.AddDays(7)
        );

        await _refreshRepository.AddAsync(newRefresh, ct);

        var access = _jwt.GenerateAccessToken(account.Id, account.Email);

        await _refreshRepository.SaveAsync(ct);

        _logger.LogInformation("Refresh token successfully used for account {AccountId}", account.Id);

        return new AuthTokensResponse(access, newRefreshValue);
    }
}
