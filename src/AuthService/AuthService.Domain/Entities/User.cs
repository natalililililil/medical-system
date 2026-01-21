using Users.Domain.Enums;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public bool EmailConfirmed { get; private set; }
        public string? EmailConfirmationToken { get; private set; }
        public Role Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User() { }
        public User(string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            Role = Role.User;
            EmailConfirmed = false;
            EmailConfirmationToken = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
            EmailConfirmationToken = null;
        }
    }
}
