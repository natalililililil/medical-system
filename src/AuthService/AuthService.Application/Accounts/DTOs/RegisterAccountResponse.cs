namespace AuthService.Application.Accounts.DTOs
{
    public record RegisterAccountResponse(Guid Id, string Email, string Role);
}
