using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Receptionist.GetAllProfiles;

public class GetAllProfilesHandler(IProfilesDbContext context, ILogger<GetAllProfilesHandler> _logger) : IRequestHandler<GetAllProfilesQuery, List<UserRegistryDto>>
{
    public async Task<List<UserRegistryDto>> Handle(GetAllProfilesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receptionist access: Fetching all system users for the registry");

        var doctors = await context.DoctorProfiles
            .AsNoTracking()
            .Select(d => new UserRegistryDto(
                d.AccountId, d.FirstName, d.LastName, d.MiddleName, "Doctor", d.PhotoUrl, d.OfficeId))
            .ToListAsync(cancellationToken);

        var patients = await context.PatientProfiles
            .AsNoTracking()
            .Select(p => new UserRegistryDto(
                p.AccountId, p.FirstName, p.LastName, p.MiddleName, "Patient", p.PhotoUrl, null))
            .ToListAsync(cancellationToken);

        var receptionists = await context.ReceptionistProfiles
            .AsNoTracking()
            .Select(r => new UserRegistryDto(
                r.AccountId, r.FirstName, r.LastName, r.MiddleName, "Receptionist", r.PhotoUrl, r.OfficeId))
            .ToListAsync(cancellationToken);

        var allUsers = doctors
            .Concat(patients)
            .Concat(receptionists)
            .OrderBy(u => u.LastName)
            .ToList();

        _logger.LogInformation("Total users found: {Count}", allUsers.Count);

        return allUsers;
    }
}
