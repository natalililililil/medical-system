using MediatR;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Doctors.GetDoctorById;

public record GetDoctorByIdQuery(Guid Id) : IRequest<DoctorDetailsDto?>;