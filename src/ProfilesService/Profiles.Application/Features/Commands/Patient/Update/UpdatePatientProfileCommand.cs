using MedicalSystem.Shared.Interfaces;

namespace Profiles.Application.Features.Commands.Patient.Update;

public record UpdatePatientProfileCommand(Guid AccountId, string FirstName, string LastName, string? MiddleName, 
    DateTime DateOfBirth, string? Photo, string? Phone) : ICommand<Guid>;