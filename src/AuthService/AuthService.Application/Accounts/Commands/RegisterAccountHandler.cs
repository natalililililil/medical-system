using AuthService.Domain.Accounts;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands
{
    public class RegisterAccountHandler : IRequestHandler<RegisterAccountCommand>
    {
        private readonly IAccountRepository _repository;
        public RegisterAccountHandler(IAccountRepository repository) => _repository = repository;

        public async Task Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.GetByEmailAsync(request.Email, cancellationToken) != null)
            {
                throw new InvalidOperationException("An account with the given email already exists");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var account = new Account(request.Email, passwordHash);

            //email token

            _repository.Add(account);
            await _repository.SaveAsync(cancellationToken);
        }
    }
}
