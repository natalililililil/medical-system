using MedicalSystem.Shared.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Infrastructure.Persistence.Configurations;

public class OutboxMessagesConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Type)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.Content)
            .IsRequired();

        builder.Property(m => m.OccurredOnUtc)
            .IsRequired();

        builder.HasIndex(x => x.ProcessedOnUtc)
            .HasFilter("[ProcessedOnUtc] IS NULL");
    }
}
