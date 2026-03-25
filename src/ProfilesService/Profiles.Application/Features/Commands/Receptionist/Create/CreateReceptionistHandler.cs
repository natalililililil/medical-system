using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Commands.Receptionist.Create;

public class CreateReceptionistHandler(IProfilesDbContext context, ILogger<CreateReceptionistHandler> _logger) : IRequestHandler<CreateReceptionistCommand, Guid>
{
    public async Task<Guid> Handle(CreateReceptionistCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Checking for existing receptionist profile for AccountId: {AccountId}", request.AccountId);

        var existing = await context.ReceptionistProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.AccountId == request.AccountId, ct);

        if (existing != null)
        {
            _logger.LogInformation("Checking for existing receptionist profile for AccountId: {AccountId}", request.AccountId);
            throw new ConflictException("RECEPTIONIST_EXISTS", "Receptionist with this account already exists");
        }

        var receptionist = new ReceptionistProfile(
            request.AccountId, request.FirstName, request.LastName,
            request.MiddleName, request.OfficeId ,request.PhotoUrl);

        context.ReceptionistProfiles.Add(receptionist);

        _logger.LogInformation("Receptionist profile successfully created for AccountId: {AccountId}", request.AccountId);

        return receptionist.AccountId;
    }
}