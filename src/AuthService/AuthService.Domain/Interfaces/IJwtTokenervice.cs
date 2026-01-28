namespace AuthService.Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(Guid userId, string email);
        string GenerateRefreshToken();
    }
}
