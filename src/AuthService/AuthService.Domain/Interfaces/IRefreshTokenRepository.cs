using AuthService.Domain.Accounts;

namespace AuthService.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token, CancellationToken ct);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct);
        void Revoke(RefreshToken token);
        Task SaveAsync(CancellationToken ct);
    }
}
