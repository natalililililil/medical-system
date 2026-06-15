using FluentValidation;
using Profiles.Application.Common.Validation;

namespace Profiles.Application.Features.Commands.Patient.Update;

public class UpdatePatientProfileValidator : AbstractValidator<UpdatePatientProfileCommand>
{
    public UpdatePatientProfileValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account Id is required for profile creation");

        RuleFor(x => x.FirstName).
            IsValidName("First name");

        RuleFor(x => x.LastName)
            .IsValidName("Last name");

        RuleFor(x => x.MiddleName)
            .IsValidName("Middle name");

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Invalid phone number format");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now);
    }
}
