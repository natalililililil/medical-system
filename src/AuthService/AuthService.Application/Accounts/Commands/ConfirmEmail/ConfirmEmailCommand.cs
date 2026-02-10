using MediatR;

namespace AuthService.Application.Accounts.Commands.ConfirmEmail;

public record ConfirmEmailCommand(string Token) : IRequest<Unit>;