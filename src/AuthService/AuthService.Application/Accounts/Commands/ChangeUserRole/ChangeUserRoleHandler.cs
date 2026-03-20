using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Accounts;
using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Accounts.Commands.ChangeUserRole;

public class ChangeUserRoleHandler(IAuthDbContext context, ILogger<ChangeUserRoleHandler> logger) : IRequestHandler<ChangeUserRoleCommand, Unit>
{
    public async Task<Unit> Handle(ChangeUserRoleCommand request, CancellationToken ct)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Id == request.UserId, ct);
        if (account == null)
        {
            logger.LogWarning("Account with id {AccountId} not found", request.UserId);
            throw new NotFoundException("ACCOUNT_NOT_FOUND", "Account not found");
        }

        if (!Enum.TryParse<Role>(request.Role, true, out var role))
        {
            throw new ConflictException("INVALID_ROLE", "Invalid role");
        }

        if (account.Role == role)
        {
            logger.LogWarning("Account {AccountId} already has role {Role}", account.Id, request.Role);
            throw new ConflictException("ROLE_ALREADY_ASSIGNED", "Account already has this role");
        }

        account.UpdateRole(role);

        logger.LogInformation("Changed role for account {AccountId} to {NewRole}", account.Id, request.Role);
        return Unit.Value;
    }
}
