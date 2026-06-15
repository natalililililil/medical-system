using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.DTOS;
using System.Numerics;

namespace Profiles.Application.Features.Queries.Patient.GetPatientById;

public class GetPatientByIdHadler(IProfilesDbContext context, ILogger<GetPatientByIdHadler> _logger) : IRequestHandler<GetPatientByIdQuery, PatientDetailsDto?>
{
    public async Task<PatientDetailsDto?> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving patient profile with ID: {PatientId}", request.Id);

        var patient = await context.PatientProfiles.FirstOrDefaultAsync(p => p.AccountId == request.Id, cancellationToken);

        if (patient == null)
        {
            _logger.LogWarning("Patient profile with ID: {PatientId} not found", request.Id);
            throw new NotFoundException("PATIENT_NOT_FOUND", $"Patient with ID {request.Id} not found");
        }

        _logger.LogInformation("Successfully mapped profile for patient: {LastName} (ID: {PatientId})", patient.LastName, request.Id);

        return new PatientDetailsDto(
            patient.LastName,
            patient.FirstName,
            patient.MiddleName,
            patient.DateOfBirth,
            patient.PhotoUrl,
            patient.Phone);
    }
}
