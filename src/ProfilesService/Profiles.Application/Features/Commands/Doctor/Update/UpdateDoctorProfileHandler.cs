using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.Extensions;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Commands.Doctor.Update;

public class UpdateDoctorProfileHandler(IProfilesDbContext _context, ILogger<UpdateDoctorProfileHandler> _logger) : IRequestHandler<UpdateDoctorProfileCommand, Guid>
{
    public async Task<Guid> Handle(UpdateDoctorProfileCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Fetching doctor profile for update with AccountId: {AccountId}", request.AccountId);

        var doctor = await _context.DoctorProfiles.Include(d => d.Specialization).GetProfileOrThrowAsync(request.AccountId, _logger, ct);

        doctor.Update(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth,
           request.CareerStartYear, request.SpecializationId, request.OfficeId, (DoctorStatus)request.Status, request.PhotoUrl);

        _logger.LogInformation("Doctor profile successfully updated for AccountId: {AccountId}", request.AccountId);
        return doctor.AccountId;
    }
}