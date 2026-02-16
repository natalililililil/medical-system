using AuthService.Application.Accounts.DTOs;
using AuthService.Application.Common.Exceptions;
using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands.Login;

public class LoginHandler(IAuthDbContext context, IJwtTokenService jwtTokenService, ILogger<LoginHandler> logger) : IRequestHandler<LoginCommand, AuthTokensResponse>
{
    public async Task<AuthTokensResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Email == request.Email, ct);

        if (account == null || !BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash))
        { 
            logger.LogWarning("Failed login attempt for email: {Email}", request.Email);
            throw new UnauthorizedException("Either an email or a password is incorrect");
        }

        if (!account.IsEmailVerified)
        {
            logger.LogWarning("Login attempt with unverified email: {Email}", request.Email);
            throw new ConflictException("Email is not confirmed");
        }

        var oldTokens = await context.RefreshTokens.Where(t => t.AccountId == account.Id && t.RevokedAt == null).ToListAsync(ct);

        foreach (var token in oldTokens)
            token.Revoke();
        logger.LogInformation("Revoked {Count} old tokens for account {Email}", oldTokens.Count, request.Email);

        var accessToken = jwtTokenService.GenerateAccessToken(account.Id, account.Email);
        var newRefreshTokenValue = jwtTokenService.GenerateRefreshToken();
        var newRefreshToken = new RefreshToken(account.Id, newRefreshTokenValue, DateTime.UtcNow.AddDays(7));

        context.RefreshTokens.Add(newRefreshToken);

        logger.LogInformation("User logged in successfully: {Email}", request.Email);

        return new AuthTokensResponse(accessToken, newRefreshTokenValue);
    }
}