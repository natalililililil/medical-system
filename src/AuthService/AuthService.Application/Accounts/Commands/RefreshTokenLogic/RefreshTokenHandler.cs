using AuthService.Application.Accounts.DTOs;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Accounts;
using MediatR;

namespace AuthService.Application.Accounts.Commands.RefreshTokenLogic
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthTokensResponse>
    {
        private readonly IRefreshTokenRepository _refreshRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtTokenService _jwt;

        public RefreshTokenHandler(IRefreshTokenRepository refreshRepo, IAccountRepository accountRepo, IJwtTokenService jwt)
        {
            _refreshRepository = refreshRepo;
            _accountRepository = accountRepo;
            _jwt = jwt;
        }

        public async Task<AuthTokensResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var refresh = await _refreshRepository.GetByTokenAsync(request.RefreshToken, ct);

            if (refresh == null || !refresh.IsActive)
                throw new InvalidOperationException("Invalid refresh token");

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

            return new AuthTokensResponse(access, newRefreshValue);
        }
    }
}
