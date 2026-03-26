using AuthService.Domain.Entities.Accounts;
using AuthService.Domain.Entities.Tokens;
using MedicalSystem.Shared.Outbox;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Common.Interfaces;

public interface IAuthDbContext
{
    DbSet<Account> Accounts { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<EmailConfirmationToken> EmailConfirmationTokens { get; }
    DbSet<OutboxMessage> OutboxMessages { get; set; }

    Task<int> SaveChangesAsync(CancellationToken ct);
}