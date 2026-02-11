using FluentValidation;

namespace AuthService.Application.Common.Validation;

public static class PasswordRules
{
    public static IRuleBuilderOptions<T, string> ApplyPasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .MaximumLength(15).WithMessage("Password must be less than 16 characters long");
    }
}
