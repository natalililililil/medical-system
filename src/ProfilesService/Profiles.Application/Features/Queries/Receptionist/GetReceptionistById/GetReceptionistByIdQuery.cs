using MediatR;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Receptionist.GetReceptionistById;

public record GetReceptionistByIdQuery(Guid Id) : IRequest<ReceptionistDetailsDto?>;
