using MediatR;
using MedicalSystem.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profiles.Application.Common.Interfaces;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Doctor.GetSpecializations;

public class GetSpecializationsHandler(IProfilesDbContext context, ILogger<GetSpecializationsHandler> _logger) : IRequestHandler<GetSpecializationsQuery, List<SpecializationDto>>
{
    public async Task<List<SpecializationDto>> Handle(GetSpecializationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all specializations");

        var specializations = await context.Specializations.AsNoTracking().Where(s => s.IsActive).ToListAsync(cancellationToken);

        if (specializations == null || !specializations.Any())
        {
            _logger.LogWarning("No specializations found");
            throw new NotFoundException("SPECIALIZATIONS_NOT_FOUND", "No specializations found");
        }

        _logger.LogInformation("Found {Count} specializations", specializations.Count);

        return specializations.Select(s => new SpecializationDto(s.Id, s.Name)).ToList();
    }
}