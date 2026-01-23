using AuthService.Domain.Accounts;

namespace AuthService.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByEmailAsync(string email, CancellationToken ct);
        Task<Account?> GetByIdAsync(Guid id, CancellationToken ct);
        Task AddAsync(Account account, CancellationToken ct);
        Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    }
}