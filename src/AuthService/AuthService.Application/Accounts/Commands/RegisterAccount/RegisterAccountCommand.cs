using MediatR;

namespace AuthService.Application.Accounts.Commands.RegisterAccount
{
    public record RegisterAccountCommand(string Email, string Password) : IRequest;
}
