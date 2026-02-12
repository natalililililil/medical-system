using AuthService.Application.Accounts.DTOs;
using AuthService.Application.Common.Exceptions;
using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands.Login;

public class LoginHandler(AuthDbContext context, IJwtTokenService jwtTokenService, ILogger<LoginHandler> logger) : IRequestHandler<LoginCommand, AuthTokensResponse>
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
        
        await using var transaction = await context.Database.BeginTransactionAsync(ct);

        var oldTokens = context.RefreshTokens.Where(t => t.AccountId == account.Id);
        context.RefreshTokens.RemoveRange(oldTokens);

        var accessToken = jwtTokenService.GenerateAccessToken(account.Id, account.Email);
        var refreshTokenValue = jwtTokenService.GenerateRefreshToken();
        var refreshToken = new RefreshToken(account.Id, refreshTokenValue, DateTime.UtcNow.AddDays(7));

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        logger.LogInformation("User logged in successfully: {Email}", request.Email);

        return new AuthTokensResponse(accessToken, refreshTokenValue);
    }
}