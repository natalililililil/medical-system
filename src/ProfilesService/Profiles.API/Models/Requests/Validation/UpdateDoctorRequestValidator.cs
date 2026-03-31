using FluentValidation;
using Profiles.Application.Common.Validation;

namespace Profiles.API.Models.Requests.Validation;

public class UpdateDoctorRequestValidator : AbstractValidator<UpdateDoctorRequest>
{
    public UpdateDoctorRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .IsValidName("First name");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .IsValidName("Last name");

        RuleFor(x => x.MiddleName)
            .IsValidMiddleName();

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required");

        RuleFor(x => x.OfficeId)
            .NotEmpty().WithMessage("Office must be selected");

        RuleFor(x => x.PhotoUrl)
            .MaximumLength(500).WithMessage("Photo URL is too long");
    }
}
