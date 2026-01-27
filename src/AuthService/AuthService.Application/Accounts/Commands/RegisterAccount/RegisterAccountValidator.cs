using FluentValidation;

namespace AuthService.Application.Accounts.Commands.RegisterAccount
{
    public class RegisterAccountValidator : AbstractValidator<RegisterAccountCommand>
    {
        public RegisterAccountValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Пароль должен быть минимум 6 символов");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Пароли не совпадают");
        }
    }
}
