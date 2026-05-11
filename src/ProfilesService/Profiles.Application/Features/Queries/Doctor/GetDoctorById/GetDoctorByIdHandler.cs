using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Doctor.GetDoctorById;

public class GetDoctorByIdHandler(IProfilesDbContext context, ILogger<GetDoctorByIdHandler> _logger) : IRequestHandler<GetDoctorByIdQuery, DoctorDetailsDto?>
{
    public async Task<DoctorDetailsDto?> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving doctor profile with ID: {DoctorId}", request.Id);
        var doctor = await context.DoctorProfiles.Include(d => d.Specialization).FirstOrDefaultAsync(d => d.AccountId == request.Id, cancellationToken);

        if (doctor == null)
        {
            _logger.LogWarning("Doctor lookup failed: No profile found for ID {DoctorId}", request.Id);
            throw new NotFoundException("DOCTOR_NOT_FOUND", $"Doctor with ID {request.Id} not found");
        }

        _logger.LogInformation("Successfully mapped profile for doctor: {LastName} (ID: {DoctorId})", doctor.LastName, request.Id);
        return new DoctorDetailsDto(
            doctor.LastName,
            doctor.FirstName,
            doctor.MiddleName,
            doctor.Specialization.Name,
            doctor.CareerStartYear,
            doctor.Status.ToString(),
            doctor.PhotoUrl,
            doctor.OfficeId,
            doctor.DateOfBirth);
    }
}