using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;

namespace Profiles.Application.Features.Commands.Doctor.Update;

public class UpdateDoctorProfileHandler(IProfilesDbContext _context, ILogger<UpdateDoctorProfileHandler> _logger) : IRequestHandler<UpdateDoctorProfileCommand, Guid>
{
    public async Task<Guid> Handle(UpdateDoctorProfileCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Fetching doctor profile for update with AccountId: {AccountId}", request.AccountId);

        if (request.AccountId == Guid.Empty)
        {
            throw new UnauthorizedException("INVALID_ACCOUNT_ID", "Account ID is missing or invalid.");
        }

        var doctor = await _context.DoctorProfiles.FirstOrDefaultAsync(d => d.AccountId == request.AccountId, ct);

        if (doctor == null)
        {
            _logger.LogWarning("Update failed: Doctor profile not found for AccountId: {AccountId}", request.AccountId);
            throw new NotFoundException("DOCTOR_NOT_FOUND", "Doctor profile not found");
        }

        doctor.Update(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth,
           request.CareerStartYear, request.PhotoUrl, request.SpecializationId, request.OfficeId);

        _logger.LogInformation("Doctor profile successfully updated for AccountId: {AccountId}", request.AccountId);
        return doctor.AccountId;
    }
}