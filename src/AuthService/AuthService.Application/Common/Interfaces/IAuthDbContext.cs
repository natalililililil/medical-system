using AuthService.Domain.Accounts;
using AuthService.Domain.Tokens;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Common.Interfaces;

public interface IAuthDbContext
{
    DbSet<Account> Accounts { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<EmailConfirmationToken> EmailConfirmationTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}