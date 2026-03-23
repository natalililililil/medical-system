using MediatR;

namespace Profiles.Application.Features.Commands.CreatePatient;

public record CreatePatientCommand(
    Guid AccountId,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    string Phone,
    string? PhotoUrl = null
) : IRequest<Guid>;