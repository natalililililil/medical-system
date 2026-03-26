using Microsoft.EntityFrameworkCore;

namespace MedicalSystem.Shared.Outbox.Interfaces;

public interface IHasOutbox
{
    DbSet<OutboxMessage> OutboxMessages { get; set; }
}