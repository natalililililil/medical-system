namespace Profiles.Domain.Entities;

public class Specialization
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    private Specialization() { }
    public Specialization(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}