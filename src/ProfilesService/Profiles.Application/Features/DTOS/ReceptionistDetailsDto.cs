namespace Profiles.Application.Features.DTOS;

public record ReceptionistDetailsDto(
    string FirstName,
    string LastName,
    string? MiddleName,
    Guid OfficeId,
    string? PhotoUrl);