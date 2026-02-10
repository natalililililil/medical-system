using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AuthDbContext _authDbContext;
    public AccountRepository(AuthDbContext authDbContext) => _authDbContext = authDbContext;

    public async Task<Account?> GetByEmailAsync(string email, CancellationToken ct)
        => await _authDbContext.Accounts.FirstOrDefaultAsync(x => x.Email == email, ct);
    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _authDbContext.Accounts.FirstOrDefaultAsync(x => x.Id == id, ct);
    public async Task AddAsync(Account account)
        => await _authDbContext.Accounts.AddAsync(account);
    public async Task SaveAsync(CancellationToken ct)
        => await _authDbContext.SaveChangesAsync(ct);
}