using MediatR;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Doctor.GetSpecializations;

public record GetSpecializationsQuery() : IRequest<List<SpecializationDto>>;