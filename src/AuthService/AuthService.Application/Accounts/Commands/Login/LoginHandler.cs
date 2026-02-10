using AuthService.Application.Accounts.DTOs;
using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, AuthTokensResponse>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(IAccountRepository accountRepository, IRefreshTokenRepository refreshTokenRepository, IJwtTokenService jwtTokenService,
        ILogger<LoginHandler> logger)
    {
        _accountRepository = accountRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<AuthTokensResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash))
        { 
            _logger.LogWarning("Failed login attempt for email: {Email}", request.Email);
            throw new InvalidOperationException("Either an email or a password is incorrect");
        }

        if (!account.IsEmailVerified)
        {
            _logger.LogWarning("Login attempt with unverified email: {Email}", request.Email);
            throw new InvalidOperationException("Email is not confirmed");
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(account.Id, account.Email);
        var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();
        var refreshToken = new RefreshToken(account.Id, refreshTokenValue, DateTime.UtcNow.AddDays(7));

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await _refreshTokenRepository.SaveAsync(cancellationToken);

        _logger.LogInformation("User logged in successfully: {Email}", request.Email);
        return new AuthTokensResponse(accessToken, refreshTokenValue);
    }
}