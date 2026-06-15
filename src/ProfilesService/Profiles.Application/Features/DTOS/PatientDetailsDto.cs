namespace Profiles.Application.Features.DTOS;

public record PatientDetailsDto(
    string LastName,
    string FirstName,
    string MiddleName,
    DateTime DateOfBirth,
    string? PhotoUrl,
    string? Phone);