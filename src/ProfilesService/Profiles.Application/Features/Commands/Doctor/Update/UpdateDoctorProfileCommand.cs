using MedicalSystem.Shared.Interfaces;

namespace Profiles.Application.Features.Commands.Doctor.Update;

public record UpdateDoctorProfileCommand(Guid AccountId, string FirstName, string LastName, string? MiddleName, DateTime DateOfBirth,
    int CareerStartYear, Guid SpecializationId, Guid OfficeId, int Status, string? PhotoUrl = null) : ICommand<Guid>;