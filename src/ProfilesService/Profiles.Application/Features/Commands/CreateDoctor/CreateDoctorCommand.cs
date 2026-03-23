using MediatR;

namespace Profiles.Application.Features.Commands.CreateDoctor;

public record CreateDoctorCommand(
    Guid AccountId,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    int CareerStartYear,
    Guid SpecializationId,
    Guid OfficeId,
    string? PhotoUrl = null
) : IRequest<Guid>;