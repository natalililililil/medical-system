using FluentValidation;

namespace Profiles.Application.Common.Validation;

public static class UserNameRulles
{
    public static IRuleBuilderOptions<T, string> IsValidName<T>(this IRuleBuilder<T, string> ruleBuilder, string fieldName)
    {
        return ruleBuilder
            .NotEmpty().WithMessage($"{fieldName} is required")
            .MaximumLength(50).WithMessage($"{fieldName} must be at most 50 characters");
    }

    public static IRuleBuilderOptions<T, string?> IsValidMiddleName<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(50).WithMessage("Middle name must be at most 50 characters");
    }
}
