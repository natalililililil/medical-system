using MediatR;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Doctors.GetDoctors;

public record GetDoctorsQuery(string? Name = null, Guid? SpecializationId = null, Guid? OfficeId = null) : IRequest<List<DoctorDto>>;