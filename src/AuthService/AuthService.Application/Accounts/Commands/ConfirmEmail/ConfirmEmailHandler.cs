using AuthService.Application.Common.Exceptions;
using AuthService.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands.ConfirmEmail;

public class ConfirmEmailHandler(IAuthDbContext context, ILogger<ConfirmEmailHandler> logger) : IRequestHandler<ConfirmEmailCommand, Unit>
{
    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        var emailToken = await context.EmailConfirmationTokens.FirstOrDefaultAsync(x => x.Token == request.Token, ct);

        if (emailToken == null || emailToken.IsUsed || emailToken.IsExpired)
        {
            logger.LogWarning("Invalid, used or expired email confirmation token");
            throw new ConflictException("Invalid, used or expired email confirmation token");
        }    

        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Id == emailToken.AccountId, ct);

        if (account == null)
        {
            logger.LogWarning("Account not found for email confirmation token");
            throw new NotFoundException("Account not found for email confirmation token");
        }

        if (account.IsEmailVerified)
        {
            logger.LogInformation("Email already confirmed for account {Id}", account.Id);
            throw new ConflictException("Email already confirmed");
        }

        emailToken.Use();
        account.ConfirmEmail();

        logger.LogInformation("Email confirmed for account {AccountId}", account.Id);
        return Unit.Value;
    }
}