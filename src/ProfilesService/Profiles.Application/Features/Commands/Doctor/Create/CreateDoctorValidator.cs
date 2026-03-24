using FluentValidation;
using Profiles.Application.Common.Validation;

namespace Profiles.Application.Features.Commands.Doctor.Create;

public class CreateDoctorValidator : AbstractValidator<CreateDoctorCommand>
{
    public CreateDoctorValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("Account Id is required");
        RuleFor(x => x.FirstName).IsValidName("First name");
        RuleFor(x => x.LastName).IsValidName("Last name");
        RuleFor(x => x.MiddleName).IsValidMiddleName();

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now.AddYears(-18))
            .WithMessage("The doctor must be over 18 years old");

        RuleFor(x => x.CareerStartYear)
            .InclusiveBetween(1950, DateTime.Now.Year)
            .WithMessage("Incorrect career start year");

        RuleFor(x => x.SpecializationId).NotEmpty().WithMessage("Specialization Id is required");
        RuleFor(x => x.OfficeId).NotEmpty().WithMessage("Office Id is required");
    }
}