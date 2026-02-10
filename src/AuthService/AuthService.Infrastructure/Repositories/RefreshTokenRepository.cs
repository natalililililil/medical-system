using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AuthDbContext _dbContext;

    public RefreshTokenRepository(AuthDbContext dbContext) => _dbContext = dbContext;

    public async Task AddAsync(RefreshToken token, CancellationToken ct)
        => await _dbContext.RefreshTokens.AddAsync(token);

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct)
        => await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token, ct);

    public void Revoke(RefreshToken token)
        => token.Revoke();
    public async Task SaveAsync(CancellationToken ct)
        => await _dbContext.SaveChangesAsync(ct);
}