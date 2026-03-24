using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace MedicalSystem.Shared.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> _validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators is not null)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = new List<ValidationResult>();

            foreach(var validator in _validators)
            {
                var result = await validator.ValidateAsync(context, cancellationToken).ConfigureAwait(false);
                validationResults.Add(result);
            }

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));
                throw new ValidationException($"Validation error: {errorMessages}");
            }
        }

        return await next();
    }
}