namespace AuthService.Application.Accounts.DTOs
{
    public record AccountDto(Guid Id, string Email, string Role, bool IsEmailVerified);
}
