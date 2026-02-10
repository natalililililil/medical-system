using AuthService.Application.Accounts.DTOs;
using MediatR;

namespace AuthService.Application.Accounts.Commands.RefreshTokenLogic;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthTokensResponse>;