using MediatR;

namespace AuthService.Application.Accounts.Commands
{
    public record RegisterAccountCommand(string Email, string Password) : IRequest;
}
