using FluentValidation;

namespace AuthService.Application.Accounts.Commands.Login;

public class LoginValidator: AbstractValidator<LoginCommand>
{
    public LoginValidator() {

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .MaximumLength(15).WithMessage("Password must be less than 16 characters long");
    }
}