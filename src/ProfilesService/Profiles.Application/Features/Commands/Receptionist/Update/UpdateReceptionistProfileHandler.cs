using MediatR;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.Extensions;

namespace Profiles.Application.Features.Commands.Patient.Update;

public class UpdateReceptionistProfileHandler(IProfilesDbContext _context, ILogger<UpdateReceptionistProfileHandler> _logger) : IRequestHandler<UpdateReceptionistProfileCommand, Guid>
{
    public async Task<Guid> Handle(UpdateReceptionistProfileCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Fetching receptionist profile for update with AccountId: {AccountId}", request.AccountId);

        var patient = await _context.ReceptionistProfiles.GetProfileOrThrowAsync(request.AccountId, _logger, ct);

        patient.Update(request.FirstName, request.LastName, request.MiddleName, request.OfficeId, request.PhotoUrl);

        _logger.LogInformation("Receptionist profile successfully updated for AccountId: {AccountId}", request.AccountId);
        return patient.AccountId;
    }
}
