using MediatR;
using Profiles.Application.Common.Interfaces;
using Profiles.Domain.Entities;

namespace Profiles.Application.Features.Commands.CreateReceptionist;

public class CreateReceptionistHandler(IProfilesDbContext context) : IRequestHandler<CreateReceptionistCommand, Guid>
{
    public async Task<Guid> Handle(CreateReceptionistCommand request, CancellationToken ct)
    {
        var receptionist = new ReceptionistProfile(
            request.AccountId, request.FirstName, request.LastName,
            request.MiddleName, request.OfficeId ,request.PhotoUrl);

        context.ReceptionistProfiles.Add(receptionist);
        await context.SaveChangesAsync(ct);
        return receptionist.AccountId;
    }
}