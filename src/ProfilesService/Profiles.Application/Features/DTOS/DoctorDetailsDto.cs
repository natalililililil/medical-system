namespace Profiles.Application.Features.DTOS;

public record DoctorDetailsDto(
    string FullName, 
    string SpecializationName, 
    int Experience, 
    string Status, 
    string? PhotoUrl, 
    Guid OfficeId, 
    DateTime DateOfBirth);