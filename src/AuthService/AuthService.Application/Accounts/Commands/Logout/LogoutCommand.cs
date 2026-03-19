using AuthService.Application.Common.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.Logout;

public record LogoutCommand(string RefreshToken) : ICommand<Unit>;