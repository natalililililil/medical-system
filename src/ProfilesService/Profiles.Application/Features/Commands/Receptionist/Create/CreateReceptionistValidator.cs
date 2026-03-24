using FluentValidation;
using Profiles.Application.Common.Validation;

namespace Profiles.Application.Features.Commands.Receptionist.Create;

public class CreateReceptionistValidator : AbstractValidator<CreateReceptionistCommand>
{
    public CreateReceptionistValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("Account Id is required");

        RuleFor(x => x.FirstName).IsValidName("First name");
        RuleFor(x => x.LastName).IsValidName("Last name");
        RuleFor(x => x.MiddleName).IsValidMiddleName();

        RuleFor(x => x.OfficeId).NotEmpty().WithMessage("Office Id is required");
    }
}