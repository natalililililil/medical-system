using FluentValidation;
using MedicalSystem.Shared.Enums;

namespace AuthService.Application.Accounts.Commands.ChangeUserRole;

public class ChangeUserRoleValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("New role is required")
            .Must(role => Enum.TryParse<Role>(role, true, out _)).WithMessage("Invalid role");
    }
}