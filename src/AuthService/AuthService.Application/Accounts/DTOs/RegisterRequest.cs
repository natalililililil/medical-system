
namespace AuthService.Application.Accounts.DTOs;

public record RegisterRequest(string Email, string Password, string ConfirmPassword, string? Role = null);