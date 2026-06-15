using MedicalSystem.Shared.Enums;

namespace MedicalSystem.Shared.Contracts.Events;

public class AccountCreatedEvent
{
    public Guid AccountId { get; set; }
    public Role Role { get; set; }
}