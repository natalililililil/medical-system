using MedicalSystem.Shared.Interfaces;

namespace Profiles.Application.Features.Commands.Patient.Update;

public record UpdateReceptionistProfileCommand(Guid AccountId, string FirstName, string LastName, string? MiddleName, 
    Guid OfficeId, string? Photo) : ICommand<Guid>;