using Profiles.Domain.Entities;

namespace Profiles.Application.Features.DTOS;

public record DoctorDto(
    string FullName, 
    string SpecializationName, 
    int Experience, 
    string? PhotoUrl,
    DoctorStatus Status,
    Guid OfficeId);