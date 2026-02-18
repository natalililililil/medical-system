using AuthService.Application.Accounts.DTOs;
using AuthService.Application.Common.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.RefreshTokenLogic;

public record RefreshTokenCommand(string RefreshToken) : ICommand<AuthTokensResponse>;