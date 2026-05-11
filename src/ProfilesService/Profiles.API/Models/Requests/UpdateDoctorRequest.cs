namespace Profiles.API.Models.Requests;

public record UpdateDoctorRequest(string FirstName, string LastName, string? MiddleName, DateTime DateOfBirth,
        int CareerStartYear, string? PhotoUrl, string? SpecializationName, Guid OfficeId, int Status);