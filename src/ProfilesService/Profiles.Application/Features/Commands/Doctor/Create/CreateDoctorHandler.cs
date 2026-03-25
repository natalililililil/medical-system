using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Commands.Doctor.Create;

public class CreateDoctorHandler(IProfilesDbContext context, ILogger<CreateDoctorHandler> _logger) : IRequestHandler<CreateDoctorCommand, Guid>
{
    public async Task<Guid> Handle(CreateDoctorCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Checking for existing doctor profile for AccountId: {AccountId}", request.AccountId);

        var existing = await context.DoctorProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.AccountId == request.AccountId, ct);

        if (existing != null)
        {
            _logger.LogWarning("Creation failed: Doctor profile already exists for AccountId: {AccountId}", request.AccountId);
            throw new ConflictException("DOCTOR_EXISTS", "Doctor with this account already exists");
        }

        var doctor = new DoctorProfile(
            request.AccountId, request.FirstName, request.LastName, request.MiddleName,
            request.DateOfBirth, request.CareerStartYear, request.PhotoUrl, request.SpecializationId,
            request.OfficeId);

        context.DoctorProfiles.Add(doctor);

        _logger.LogInformation("Doctor profile successfully created for AccountId: {AccountId}", request.AccountId);

        return doctor.AccountId;
    }
}