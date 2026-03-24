using MediatR;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Doctor.GetDoctorById;

public class GetDoctorByIdHandler(IProfilesDbContext context) : IRequestHandler<GetDoctorByIdQuery, DoctorDetailsDto?>
{
    public async Task<DoctorDetailsDto?> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        var doctor = await context.DoctorProfiles.Include(d => d.Specialization).FirstOrDefaultAsync(d => d.AccountId == request.Id, cancellationToken);

        return new DoctorDetailsDto(
            $"{doctor.LastName} {doctor.FirstName} {doctor.MiddleName}".Trim(),
            doctor.Specialization.Name,
            doctor.Experience,
            doctor.Status.ToString(),
            doctor.PhotoUrl,
            doctor.OfficeId,
            doctor.DateOfBirth);
    }
}
