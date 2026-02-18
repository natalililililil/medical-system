using AuthService.Application.Accounts.DTOs;
using AuthService.Application.Common.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthTokensResponse>;