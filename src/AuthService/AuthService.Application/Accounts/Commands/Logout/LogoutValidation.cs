using FluentValidation;

namespace AuthService.Application.Accounts.Commands.Logout;

public class LogoutValidation : AbstractValidator<LogoutCommand>
{
    public LogoutValidation()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required");
    }
}