using FluentValidation;

namespace Profiles.Application.Features.Queries.Doctor.GetDoctorById;

public class GetDoctorByIdValidator : AbstractValidator<GetDoctorByIdQuery>
{
    public GetDoctorByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Doctor ID is required");
    }
}