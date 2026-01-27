using AuthService.Application.Accounts.Commands.RegisterAccount;
using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Tokens;
using MediatR;

namespace AuthService.Application.Accounts.Commands
{
    public class RegisterAccountHandler : IRequestHandler<RegisterAccountCommand, Unit>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailConfirmationTokenRepository _emailTokenRepository;
        public RegisterAccountHandler(IAccountRepository accountRepository, IEmailConfirmationTokenRepository emailTokenRepository)
        {
            _accountRepository = accountRepository;
            _emailTokenRepository = emailTokenRepository;
        }

        public async Task<Unit> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            if (await _accountRepository.GetByEmailAsync(request.Email, cancellationToken) != null)
                throw new InvalidOperationException("Такой аккаунт уже существует");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var account = new Account(request.Email, passwordHash);

            _accountRepository.Add(account);
            await _accountRepository.SaveAsync(cancellationToken);

            var emailToken = new EmailConfirmationToken(account.Id);
            _emailTokenRepository.Add(emailToken);
            await _emailTokenRepository.SaveAsync(cancellationToken);

            Console.WriteLine($"Email confirmation link: http://localhost:5173/confirm-email?token={emailToken.Token}");

            return Unit.Value;
        }
    }
}
