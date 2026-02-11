using AuthService.Application.Common.Validation;
using FluentValidation;

namespace AuthService.Application.Accounts.DTOs.Validation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).ApplyEmailRules();
        RuleFor(x => x.Password).ApplyPasswordRules();
    }
}