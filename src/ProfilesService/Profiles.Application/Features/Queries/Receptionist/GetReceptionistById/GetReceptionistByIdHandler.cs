using Confluent.Kafka;
using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Receptionist.GetReceptionistById;

public class GetReceptionistByIdHandler(IProfilesDbContext context, ILogger<GetReceptionistByIdHandler> _logger) : IRequestHandler<GetReceptionistByIdQuery, ReceptionistDetailsDto?>
{
    public async Task<ReceptionistDetailsDto?> Handle(GetReceptionistByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving receptionist profile with ID: {ReceptionistId}", request.Id);

        var receptionist = context.ReceptionistProfiles.FirstOrDefault(p => p.AccountId == request.Id);

        if (receptionist == null)
        {
            _logger.LogWarning("Receptionist profile with ID: {ReceptionistId} not found", request.Id);
            throw new NotFoundException("RECEPTIONIST_NOT_FOUND", $"Receptionist with ID {request.Id} not found");
        }

        _logger.LogInformation("Successfully mapped profile for receptionist: {LastName} (ID: {ReceptionistId})", receptionist.LastName, request.Id);

        return new ReceptionistDetailsDto(
            receptionist.LastName,
            receptionist.FirstName,
            receptionist.MiddleName,
            receptionist.OfficeId,
            receptionist.PhotoUrl);
    }
}
