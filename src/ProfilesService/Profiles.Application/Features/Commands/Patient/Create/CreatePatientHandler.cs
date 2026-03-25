using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Commands.Patient.Create;

public class CreatePatientHandler(IProfilesDbContext context, ILogger<CreatePatientHandler> _logger) : IRequestHandler<CreatePatientCommand, Guid>
{
    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Checking for existing patient profile for AccountId: {AccountId}", request.AccountId);

        var existing = await context.PatientProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.AccountId == request.AccountId, ct);

        if (existing != null)
        {
            _logger.LogWarning("Creation failed: Patient profile already exists for AccountId: {AccountId}", request.AccountId);
            throw new ConflictException("PATIENT_EXISTS", "Patient with this account already exists");
        }

        var patient = new PatientProfile(
            request.AccountId, request.FirstName, request.LastName,
            request.MiddleName, request.DateOfBirth, request.PhotoUrl, request.Phone);

        context.PatientProfiles.Add(patient);

        _logger.LogInformation("Patient profile successfully created for AccountId: {AccountId}", request.AccountId);

        return patient.AccountId;
    }
}