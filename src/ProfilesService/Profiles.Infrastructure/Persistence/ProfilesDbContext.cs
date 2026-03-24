using MedicalSystem.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;

namespace Profiles.Infrastructure.Persistence;

public class ProfilesDbContext: DbContext, IProfilesDbContext, IAppDbContext
{
    public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : base(options) { }

    public DbSet<DoctorProfile> DoctorProfiles => Set<DoctorProfile>();
    public DbSet<PatientProfile> PatientProfiles => Set<PatientProfile>();
    public DbSet<ReceptionistProfile> ReceptionistProfiles => Set<ReceptionistProfile>();
    public DbSet<Specialization> Specializations => Set<Specialization>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfilesDbContext).Assembly);
        modelBuilder.Entity<DoctorProfile>();
    }
}