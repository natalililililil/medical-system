using FluentValidation;
using Profiles.Application.Common.Validation;

namespace Profiles.Application.Features.Commands.Patient.Create;

public class CreatePatientValidator : AbstractValidator<CreatePatientCommand>
{
    public CreatePatientValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("Account Id is required");
        RuleFor(x => x.FirstName).IsValidName("First name");
        RuleFor(x => x.LastName).IsValidName("Last name");
        RuleFor(x => x.MiddleName).IsValidMiddleName();

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Invalid phone number format");

        RuleFor(x => x.DateOfBirth).LessThan(DateTime.Now);
    }
}