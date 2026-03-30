using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Persistence.Configurations;

public sealed class DoctorProfileConfiguration : IEntityTypeConfiguration<DoctorProfile>
{
    public void Configure(EntityTypeBuilder<DoctorProfile> builder)
    {
        builder.ToTable("DoctorProfiles");
        builder.HasKey(d => d.AccountId);

        builder.Property(d => d.AccountId)
            .ValueGeneratedNever();

        builder.Property(d => d.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.MiddleName)
            .HasMaxLength(50);

        builder.Property(d => d.DateOfBirth)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(d => d.CareerStartYear)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasConversion<string>();

        builder.Property(d => d.PhotoUrl);

        builder.Property(d => d.OfficeId)
            .IsRequired();

        builder.Ignore(x => x.Experience);

        builder.HasOne(d => d.Specialization)
            .WithMany()
            .HasForeignKey(d => d.SpecializationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.SpecializationId);
    }
}