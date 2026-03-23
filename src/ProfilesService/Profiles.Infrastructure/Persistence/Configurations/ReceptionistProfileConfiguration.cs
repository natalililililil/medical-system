using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Persistence.Configurations;

public sealed class ReceptionistProfileConfiguration : IEntityTypeConfiguration<ReceptionistProfile>
{
    public void Configure(EntityTypeBuilder<ReceptionistProfile> builder)
    {
        builder.ToTable("ReceptionistProfiles");

        builder.HasKey(rp => rp.AccoundId);
        builder.Property(rp => rp.AccoundId)
            .ValueGeneratedNever();

        builder.Property(rp => rp.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(rp => rp.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(rp => rp.MiddleName)
            .HasMaxLength(50);

        builder.Property(rp => rp.OfficeId)
            .IsRequired();

        builder.Property(rp => rp.PhotoUrl);
    }
}