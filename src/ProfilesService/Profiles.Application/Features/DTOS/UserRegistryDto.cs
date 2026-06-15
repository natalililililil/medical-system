namespace Profiles.Application.Features.DTOS;

public record UserRegistryDto(
    Guid AccountId,
    string FirstName,
    string LastName,
    string? MiddleName,
    string Role,
    string? PhotoUrl,
    Guid? OfficeId
);