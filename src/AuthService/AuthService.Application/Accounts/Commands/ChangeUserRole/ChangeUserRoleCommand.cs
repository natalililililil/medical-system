using MedicalSystem.Shared.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.ChangeUserRole;

public record ChangeUserRoleCommand(Guid UserId, string Role) : ICommand<Unit>;