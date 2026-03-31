namespace Profiles.API.Models.Requests;

public record UpdateDoctorRequest(string FirstName, string LastName, string? MiddleName, DateTime DateOfBirth,
        int CareerStartYear, string? PhotoUrl, Guid? SpecializationId, Guid OfficeId);