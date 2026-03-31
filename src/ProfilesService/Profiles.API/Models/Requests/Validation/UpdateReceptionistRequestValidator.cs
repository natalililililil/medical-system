using FluentValidation;

namespace Profiles.API.Models.Requests.Validation;

public class UpdateReceptionistRequestValidator : AbstractValidator<UpdateReceptionistRequest>
{
    public UpdateReceptionistRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required");
    }
}
