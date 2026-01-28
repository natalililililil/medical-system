using AuthService.Domain.Accounts;

namespace AuthService.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        void Add(RefreshToken token, CancellationToken ct);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct);
        void Revoke(RefreshToken token, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);
    }
}
