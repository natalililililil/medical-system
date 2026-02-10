namespace AuthService.Application.Accounts.DTOs;

public record AuthTokensResponse(string AccessToken, string RefreshToken);