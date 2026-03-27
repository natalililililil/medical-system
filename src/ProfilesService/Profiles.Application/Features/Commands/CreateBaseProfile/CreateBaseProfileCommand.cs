using MedicalSystem.Shared.Enums;
using MedicalSystem.Shared.Interfaces;

namespace Profiles.Application.Features.Commands.CreateBaseProfile;

public record CreateBaseProfileCommand(Guid AccountId, Role Role) : ICommand<Guid>;