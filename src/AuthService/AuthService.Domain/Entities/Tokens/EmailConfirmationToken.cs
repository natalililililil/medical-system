namespace AuthService.Domain.Entities.Tokens;

public class EmailConfirmationToken
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public string Token { get; private set; } = null!;
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }

    private EmailConfirmationToken() { }

    public EmailConfirmationToken(Guid accountId)
    {
        Id = Guid.NewGuid();
        AccountId = accountId;
        Token = Guid.NewGuid().ToString();
        ExpiresAt = DateTime.UtcNow.AddHours(24);
        IsUsed = false;
    }

    public void Use() => IsUsed = true;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}