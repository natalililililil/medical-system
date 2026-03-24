using MediatR;
using MedicalSystem.Shared.Interfaces;

namespace Profiles.Application.Features.Commands.Receptionist.Create;

public record CreateReceptionistCommand(
    Guid AccountId,
    string FirstName,
    string LastName,
    string? MiddleName,
    Guid OfficeId,
    string? PhotoUrl = null
) : ICommand<Guid>;