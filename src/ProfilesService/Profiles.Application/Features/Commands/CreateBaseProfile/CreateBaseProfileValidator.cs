using FluentValidation;

namespace Profiles.Application.Features.Commands.CreateBaseProfile;

public class CreateBaseProfileValidator : AbstractValidator<CreateBaseProfileCommand>
{
    public CreateBaseProfileValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.Role).IsInEnum();
    }
}
