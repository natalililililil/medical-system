using MediatR;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.Extensions;

namespace Profiles.Application.Features.Commands.Doctor.Update;

public class UpdateDoctorProfileHandler(IProfilesDbContext _context, ILogger<UpdateDoctorProfileHandler> _logger) : IRequestHandler<UpdateDoctorProfileCommand, Guid>
{
    public async Task<Guid> Handle(UpdateDoctorProfileCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Fetching doctor profile for update with AccountId: {AccountId}", request.AccountId);

        var doctor = await _context.DoctorProfiles.GetProfileOrThrowAsync(request.AccountId, _logger, ct);

        doctor.Update(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth,
           request.CareerStartYear, request.PhotoUrl, request.SpecializationId, request.OfficeId);

        _logger.LogInformation("Doctor profile successfully updated for AccountId: {AccountId}", request.AccountId);
        return doctor.AccountId;
    }
}