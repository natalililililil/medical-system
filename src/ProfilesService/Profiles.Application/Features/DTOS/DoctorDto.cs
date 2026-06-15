using Profiles.Domain.Entities;

namespace Profiles.Application.Features.DTOS;

public record DoctorDto(
    string FullName, 
    Guid? SpecializationId, 
    int CareerStartYear, 
    string? PhotoUrl,
    DoctorStatus Status,
    Guid OfficeId);