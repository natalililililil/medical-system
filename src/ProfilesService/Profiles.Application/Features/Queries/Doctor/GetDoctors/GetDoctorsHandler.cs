using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Doctor.GetDoctors;

public class GetDoctorsHandler(IProfilesDbContext context, ILogger<GetDoctorsHandler> _logger) : IRequestHandler<GetDoctorsQuery, List<DoctorDto>>
{
    public async Task<List<DoctorDto>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching for doctors with filters: Name={SearchName}, SpecializationId={SpecId}, OfficeId={OfficeId}",
            request.Name, request.SpecializationId, request.OfficeId);

        var query = context.DoctorProfiles.Include(d => d.Specialization).Where(d => d.Specialization != null).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var search = request.Name.ToLower();
            query = query.Where(d => 
                d.FirstName.ToLower().Contains(search) ||
                d.LastName.ToLower().Contains(search) ||
                (d.MiddleName != null && d.MiddleName.ToLower().Contains(search)));
        }

        if (request.SpecializationId.HasValue)
        {
            query = query.Where(d => d.SpecializationId == request.SpecializationId);
        }

        if (request.OfficeId.HasValue)
        {
            query = query.Where(d => d.OfficeId == request.OfficeId);
        }

        var doctors = await query.Select(d => new DoctorDto(
            $"{d.LastName} {d.FirstName} {d.MiddleName}".Trim(),
            d.Specialization != null ? d.Specialization.Name : "No specialization",
            d.Experience,
            d.PhotoUrl,
            d.Status,
            d.OfficeId
        )).ToListAsync(cancellationToken);

        if (doctors.Count == 0)
        {
            _logger.LogWarning("No doctors matched the provided search criteria");
            throw new NotFoundException("DOCTORS_NOT_FOUND", "No doctors found with the given filters");
        }

        _logger.LogInformation("Successfully retrieved {Count} doctors matching the criteria", doctors.Count);

        return doctors;
    }
}