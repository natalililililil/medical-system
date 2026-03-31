using FluentValidation;
using Profiles.Application.Common.Validation;

namespace Profiles.Application.Features.Commands.Doctor.Update;

public class UpdateDoctorProfileValidator : AbstractValidator<UpdateDoctorProfileCommand>
{
    public UpdateDoctorProfileValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account Id is required for profile creation");

        RuleFor(x => x.FirstName)
            .IsValidName("First name");

        RuleFor(x => x.LastName)
            .IsValidName("Last name");

        RuleFor(x => x.MiddleName)
            .IsValidName("Middle name");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now.AddYears(-18))
            .WithMessage("The doctor must be over 18 years old");

        RuleFor(x => x.CareerStartYear)
            .InclusiveBetween(1950, DateTime.Now.Year)
            .WithMessage("Incorrect career start year");

        RuleFor(x => x.SpecializationId).
            NotEmpty().WithMessage("Specialization Id is required");
    }
}