using AuthService.Domain.Interfaces;
using AuthService.Domain.Tokens;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repositories
{
    internal class EmailConfirmationTokenRepository : IEmailConfirmationTokenRepository
    {
        private readonly AuthDbContext _context;
        public EmailConfirmationTokenRepository(AuthDbContext context) => _context = context;

        public async Task<EmailConfirmationToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
            => await _context.EmailConfirmationTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        public void Add(EmailConfirmationToken token)
            => _context.EmailConfirmationTokens.Add(token);

        public async Task SaveAsync(CancellationToken cancellationToken)
            => await _context.SaveChangesAsync(cancellationToken);
    }
}
