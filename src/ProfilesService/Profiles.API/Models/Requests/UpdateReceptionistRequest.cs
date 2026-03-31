namespace Profiles.API.Models.Requests;

public record UpdateReceptionistRequest(string FirstName, string LastName, string? MiddleName, Guid OfficeId, string? Photo);
