using MediatR;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.Extensions;

namespace Profiles.Application.Features.Commands.Patient.Update;

public class UpdatePatientProfileHandler(IProfilesDbContext _context, ILogger<UpdatePatientProfileHandler> _logger) : IRequestHandler<UpdatePatientProfileCommand, Guid>
{
    public async Task<Guid> Handle(UpdatePatientProfileCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Fetching patient profile for update with AccountId: {AccountId}", request.AccountId);

        var patient = await _context.PatientProfiles.GetProfileOrThrowAsync(request.AccountId, _logger, ct);

        patient.Update(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth, request.Photo, request.Phone);

        _logger.LogInformation("Patient profile successfully updated for AccountId: {AccountId}", request.AccountId);
        return patient.AccountId;
    }
}
