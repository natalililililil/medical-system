namespace AuthService.Domain.Accounts
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public string Token { get; private set; } = null!;
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        private RefreshToken() { }

        public RefreshToken(Guid accountId, string token, DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            Token = token;
            ExpiresAt = expiresAt;
        }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => RevokedAt == null && !IsExpired;

        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }
    }
}