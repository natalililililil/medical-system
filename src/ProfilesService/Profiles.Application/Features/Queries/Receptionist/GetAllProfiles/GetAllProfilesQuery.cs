using MediatR;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Receptionist.GetAllProfiles;

public class GetAllProfilesQuery : IRequest<List<UserRegistryDto>>;
