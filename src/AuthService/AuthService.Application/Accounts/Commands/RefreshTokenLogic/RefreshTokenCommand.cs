using AuthService.Application.Accounts.DTOs;
using MedicalSystem.Shared.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.RefreshTokenLogic;

public record RefreshTokenCommand(string RefreshToken) : ICommand<AuthTokensResponse>;