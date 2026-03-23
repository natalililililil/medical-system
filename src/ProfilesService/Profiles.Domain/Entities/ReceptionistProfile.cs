namespace Profiles.Domain.Entities;

public class ReceptionistProfile
{
    public Guid AccoundId { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string? MiddleName { get; private set; }
    public Guid OfficeId { get; private set; }
    public string? PhotoUrl { get; private set; }
    private ReceptionistProfile() { }
    public ReceptionistProfile(Guid id, string firstName, string lastName, string? middleName, Guid officeId, string? photoUrl)
    {
        AccoundId = id;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        OfficeId = officeId;
        PhotoUrl = photoUrl;
    }
}