using AuthService.Application.Accounts.DTOs;
using MediatR;

namespace AuthService.Application.Accounts.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthTokensResponse>;