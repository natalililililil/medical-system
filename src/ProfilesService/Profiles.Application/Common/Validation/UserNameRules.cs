using FluentValidation;

namespace Profiles.Application.Common.Validation;

public static class UserNameRulles
{
    public static IRuleBuilderOptions<T, string?> IsValidName<T>(this IRuleBuilder<T, string?> ruleBuilder, string fieldName)
    {
        return ruleBuilder
            .MaximumLength(50).WithMessage($"{fieldName} must be at most 50 characters");
    }
}
