using AuthService.Application.Common.Exceptions;
using AuthService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands.Logout;

public class LogoutHandler(IAuthDbContext context, ILogger<LogoutHandler> logger) : IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken ct)
    {
        var token = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == request.RefreshToken, ct);
        if (token == null || !token.IsActive)
        {
            logger.LogWarning("Logout attempt with invalid refresh token");
            throw new NotFoundException("INVALID_REFRESH_TOKEN", "The provided refresh token is invalid");
        }

        token.Revoke();

        logger.LogInformation("Refresh token revoked successfully for account ID: {AccountId}", token.AccountId);
        return Unit.Value;
    }
}
