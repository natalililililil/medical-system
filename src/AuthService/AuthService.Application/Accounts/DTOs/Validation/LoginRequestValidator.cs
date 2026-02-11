using AuthService.Application.Common.Validation;
using FluentValidation;

namespace AuthService.Application.Accounts.DTOs.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).ApplyEmailRules();
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}