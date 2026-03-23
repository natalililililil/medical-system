namespace Profiles.Domain.Entities;

public class PatientProfile
{
    public Guid AccountId { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string? MiddleName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? PhotoUrl { get; private set; }
    public string? Phone { get; private set; }

    private PatientProfile() { }
    public PatientProfile(Guid id, string firstName, string lastName, string? middleName, DateTime dateOfBirth, string? photo, string? phone)
    {
        AccountId = id;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        PhotoUrl = photo;
        Phone = phone;
    }
}