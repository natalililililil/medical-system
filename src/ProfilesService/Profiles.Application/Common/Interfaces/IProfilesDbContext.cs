using Microsoft.EntityFrameworkCore;
using Profiles.Domain.Entities;

namespace Profiles.Application.Common.Interfaces;

public interface IProfilesDbContext
{
    DbSet<DoctorProfile> DoctorProfiles { get; }
    DbSet<PatientProfile> PatientProfiles { get; }
    DbSet<ReceptionistProfile> ReceptionistProfiles { get; }
    DbSet<Specialization> Specializations { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}
