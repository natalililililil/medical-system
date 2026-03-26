using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Entities.Accounts;
using AuthService.Domain.Entities.Tokens;
using MedicalSystem.Shared.Interfaces;
using MedicalSystem.Shared.Outbox;
using MedicalSystem.Shared.Outbox.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence;

public class AuthDbContext: DbContext, IAuthDbContext, IAppDbContext, IHasOutbox
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<EmailConfirmationToken> EmailConfirmationTokens => Set<EmailConfirmationToken>();
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}