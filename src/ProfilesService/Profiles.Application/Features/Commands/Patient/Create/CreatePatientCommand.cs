using MediatR;
using MedicalSystem.Shared.Interfaces;

namespace Profiles.Application.Features.Commands.Patient.Create;

public record CreatePatientCommand(
    Guid AccountId,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    string? Phone,
    string? PhotoUrl = null
) : ICommand<Guid>;