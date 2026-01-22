namespace AuthService.Domain.Accounts
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public Role Role { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Account() { }

        public Account(string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            Role = Role.Patient;
            IsEmailVerified = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void ConfirmEmail()
        {
            IsEmailVerified = true;
        }
    }
}