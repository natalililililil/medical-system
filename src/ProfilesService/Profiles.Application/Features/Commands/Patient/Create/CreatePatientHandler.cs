using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Commands.Patient.Create;

public class CreatePatientHandler(IProfilesDbContext context) : IRequestHandler<CreatePatientCommand, Guid>
{
    public async Task<Guid> Handle(CreatePatientCommand request, CancellationToken ct)
    {
        var existing = await context.PatientProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.AccountId == request.AccountId, ct);

        if (existing != null)
        {
            throw new ConflictException("PATIENT_EXISTS", "Patient with this account already exists");
        }

        var patient = new PatientProfile(
            request.AccountId, request.FirstName, request.LastName,
            request.MiddleName, request.DateOfBirth, request.PhotoUrl, request.Phone);

        context.PatientProfiles.Add(patient);
        return patient.AccountId;
    }
}