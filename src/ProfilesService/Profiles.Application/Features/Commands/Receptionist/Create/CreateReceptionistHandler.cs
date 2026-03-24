using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Commands.Receptionist.Create;

public class CreateReceptionistHandler(IProfilesDbContext context) : IRequestHandler<CreateReceptionistCommand, Guid>
{
    public async Task<Guid> Handle(CreateReceptionistCommand request, CancellationToken ct)
    {
        var existing = await context.ReceptionistProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.AccountId == request.AccountId, ct);

        if (existing != null)
        {
            throw new ConflictException("RECEPTIONIST_EXISTS", "Receptionist with this account already exists");
        }

        var receptionist = new ReceptionistProfile(
            request.AccountId, request.FirstName, request.LastName,
            request.MiddleName, request.OfficeId ,request.PhotoUrl);

        context.ReceptionistProfiles.Add(receptionist);
        return receptionist.AccountId;
    }
}