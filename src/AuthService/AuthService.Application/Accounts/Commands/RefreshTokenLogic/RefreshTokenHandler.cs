using AuthService.Application.Accounts.DTOs;
using AuthService.Application.Common.Exceptions;
using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands.RefreshTokenLogic;

public class RefreshTokenHandler(AuthDbContext context, IJwtTokenService jwt, ILogger<RefreshTokenHandler> logger) : IRequestHandler<RefreshTokenCommand, AuthTokensResponse>
{
    public async Task<AuthTokensResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var refresh = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, ct);

        if (refresh == null || !refresh.IsActive)
        {
            logger.LogWarning("Invalid refresh token used");
            throw new ConflictException("Invalid refresh token");
        }

        if (refresh.ExpiresAt <= DateTime.UtcNow)
        {
            logger.LogWarning("Expired refresh token used");
            throw new ConflictException("Refresh token expired");
        }
        var account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == refresh.AccountId, ct);

        if (account == null)
            throw new UnauthorizedException("Account not found");

        await using var transaction = await context.Database.BeginTransactionAsync(ct);

        refresh.Revoke();

        var newRefreshValue = jwt.GenerateRefreshToken();
        var newRefresh = new RefreshToken(
            account.Id,
            newRefreshValue,
            DateTime.UtcNow.AddDays(7)
        );

        context.RefreshTokens.Add(newRefresh);

        var access = jwt.GenerateAccessToken(account.Id, account.Email);

        await context.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);

        logger.LogInformation("Refresh token successfully used for account {AccountId}", account.Id);

        return new AuthTokensResponse(access, newRefreshValue);
    }
}
