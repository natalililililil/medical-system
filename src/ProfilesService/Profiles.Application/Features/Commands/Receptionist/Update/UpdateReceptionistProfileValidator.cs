using FluentValidation;
using Profiles.Application.Common.Validation;

namespace Profiles.Application.Features.Commands.Patient.Update;

public class UpdateReceptionistProfileValidator : AbstractValidator<UpdateReceptionistProfileCommand>
{
    public UpdateReceptionistProfileValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account Id is required for profile creation");

        RuleFor(x => x.FirstName).
            IsValidName("First name");

        RuleFor(x => x.LastName)
            .IsValidName("Last name");

        RuleFor(x => x.MiddleName)
            .IsValidName("Middle name");
    }
}
