namespace Profiles.Application.Features.DTOS;

public record DoctorDetailsDto(
    string LastName, 
    string FirstName,
    string MiddleName,
    string SpecializationName, 
    int CareerStartYear, 
    string Status, 
    string? PhotoUrl, 
    Guid OfficeId, 
    DateTime DateOfBirth);