using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;

namespace AuthService.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public Task AddAsync(Account account, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetByEmailAsync(string email, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
