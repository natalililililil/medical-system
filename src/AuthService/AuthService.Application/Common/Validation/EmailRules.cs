using FluentValidation;

namespace AuthService.Application.Common.Validation;

public static class EmailRules
{
    public static IRuleBuilder<T, string> ApplyEmailRules<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
    }
}