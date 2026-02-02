using AuthService.Application.Accounts.DTOs;
using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthTokensResponse>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginHandler(IAccountRepository accountRepository, IRefreshTokenRepository refreshTokenRepository, IJwtTokenService jwtTokenService)
        {
            _accountRepository = accountRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthTokensResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash))
                throw new InvalidOperationException("Either an email or a password is incorrect");

            if (!account.IsEmailVerified)
                throw new InvalidOperationException("Email is not confirmed");

            var accessToken = _jwtTokenService.GenerateAccessToken(account.Id, account.Email);
            var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken(account.Id, refreshTokenValue, DateTime.UtcNow.AddDays(7));

            await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await _refreshTokenRepository.SaveAsync(cancellationToken);

            return new AuthTokensResponse(accessToken, refreshTokenValue);
        }
    }
}