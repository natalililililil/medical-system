using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Doctor.GetDoctors;

public class GetDoctorsHandler(IProfilesDbContext context) : IRequestHandler<GetDoctorsQuery, List<DoctorDto>>
{
    public async Task<List<DoctorDto>> Handle(GetDoctorsQuery request, CancellationToken cancellationToken)
    {
        var query = context.DoctorProfiles.Include(d => d.Specialization).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var search = request.Name.ToLower();
            query = query.Where(d => (d.FirstName + " " + d.LastName + " " + (d.MiddleName ?? "")).ToLower().Contains(search));
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
            d.Specialization.Name,
            d.Experience,
            d.PhotoUrl,
            d.OfficeId
        )).ToListAsync(cancellationToken);

        if (doctors.Count == 0)
        {
            throw new NotFoundException("DOCTORS_NOT_FOUND", "No doctors found with the given filters");
        }

        return doctors;
    }
}