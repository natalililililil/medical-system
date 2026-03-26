using MedicalSystem.Shared.Enums;

namespace AuthService.Domain.Entities.Accounts;

public class Account
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public Role Role { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Account() { }

    public Account(string email, string passwordHash, Role role)
    {
        Id = Guid.NewGuid();
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsEmailVerified = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void ConfirmEmail()
    {
        IsEmailVerified = true;
    }

    public void UpdateRole(Role newRole)
    {
        Role = newRole;
    }
}