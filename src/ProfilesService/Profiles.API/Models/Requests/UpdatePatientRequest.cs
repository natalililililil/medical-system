namespace Profiles.API.Models.Requests;

public record UpdatePatientRequest(string FirstName, string LastName, string? MiddleName,
    DateTime DateOfBirth, string? PhotoUrl, string? Phone);