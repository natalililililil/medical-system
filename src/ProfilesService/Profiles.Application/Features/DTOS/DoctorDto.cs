namespace Profiles.Application.Features.DTOS;

public record DoctorDto(
    Guid AccountId, 
    string FullName, 
    string SpecializationName, 
    int Experience, 
    string? PhotoUrl, 
    Guid OfficeId);