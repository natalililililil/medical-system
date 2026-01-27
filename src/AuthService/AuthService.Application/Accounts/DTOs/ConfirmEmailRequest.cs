namespace AuthService.Application.Accounts.DTOs
{
    public class ConfirmEmailRequest
    {
        public string Token { get; set; } = null!;
    }
}
