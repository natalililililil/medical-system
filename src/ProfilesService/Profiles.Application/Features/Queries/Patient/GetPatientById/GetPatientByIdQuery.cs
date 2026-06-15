using MediatR;
using Profiles.Application.Features.DTOS;

namespace Profiles.Application.Features.Queries.Patient.GetPatientById;

public record GetPatientByIdQuery(Guid Id) : IRequest<PatientDetailsDto?>;