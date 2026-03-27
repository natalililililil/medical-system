using MediatR;
using MedicalSystem.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Commands.CreateBaseProfile;

public class CreateBaseProfileHandler(IProfilesDbContext _dbContext, ILogger<CreateBaseProfileHandler> _logger) : IRequestHandler<CreateBaseProfileCommand, Guid>
{
    public async Task<Guid> Handle(CreateBaseProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating initial profile for AccountId: {AccountId}, Role: {Role}", request.AccountId, request.Role);

        if (await ProfileExists(request.AccountId, request.Role, cancellationToken))
        {
            _logger.LogWarning("Profile already exists for AccountId: {AccountId}", request.AccountId);
            return request.AccountId;
        }

        switch (request.Role)
        {
            case Role.Doctor:
                _dbContext.DoctorProfiles.Add(new DoctorProfile(request.AccountId, "", "", null, DateTime.MinValue, DateTime.UtcNow.Year, null, Guid.Empty, Guid.Empty));
                break;
            case Role.Patient:
                _dbContext.PatientProfiles.Add(new PatientProfile(request.AccountId, "", "", null, DateTime.MinValue, null, null));
                break;
            case Role.Receptionist:
                _dbContext.ReceptionistProfiles.Add(new ReceptionistProfile(request.AccountId, "", "", null, Guid.Empty, null));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(request.Role), "Unsupported role");
        }

        _logger.LogInformation("Profile created successfully for {AccountId}", request.AccountId);
        return request.AccountId;
    }

    private async Task<bool> ProfileExists(Guid accountId, Role role, CancellationToken ct)
    {
        return role switch
        {
            Role.Doctor => await _dbContext.DoctorProfiles.AnyAsync(p => p.AccountId == accountId, ct),
            Role.Patient => await _dbContext.PatientProfiles.AnyAsync(p => p.AccountId == accountId, ct),
            Role.Receptionist => await _dbContext.ReceptionistProfiles.AnyAsync(p => p.AccountId == accountId, ct),
            _ => false
        };
    }
}
