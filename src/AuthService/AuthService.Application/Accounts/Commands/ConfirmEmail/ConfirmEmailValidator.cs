using FluentValidation;

namespace AuthService.Application.Accounts.Commands.ConfirmEmail
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}
