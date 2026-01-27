using AuthService.Domain.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Interfaces
{
    public interface IEmailConfirmationTokenRepository
    {
        Task<EmailConfirmationToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
        void Add(EmailConfirmationToken token);
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
