namespace Profiles.Domain.Entities;

public class DoctorProfile
{
    public Guid AccountId { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string? MiddleName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public int CareerStartYear { get; private set; }
    public DoctorStatus Status { get; private set; } = DoctorStatus.AtWork;
    public string? PhotoUrl { get; private set; }
    public Guid? SpecializationId { get; private set; }
    public Specialization? Specialization { get; private set; }
    public Guid OfficeId { get; private set; }

    private DoctorProfile() { }
    public DoctorProfile(Guid id, string firstName, string lastName, string? middleName, DateTime dateOfBirth, 
        int careerStartYear, string? photoUrl, Guid? specializationId, Guid officeId)
    {
        AccountId = id;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        CareerStartYear = careerStartYear;
        PhotoUrl = photoUrl;
        SpecializationId = specializationId;
        OfficeId = officeId;
    }

    public void Update(string firstName, string lastName, string? middleName, DateTime dateOfBirth,
        int careerStartYear, Guid? specializationId, Guid officeId, DoctorStatus status, string? photoUrl)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        CareerStartYear = careerStartYear;
        SpecializationId = specializationId;
        OfficeId = officeId;
        Status = status;
        PhotoUrl = photoUrl;
    }
}