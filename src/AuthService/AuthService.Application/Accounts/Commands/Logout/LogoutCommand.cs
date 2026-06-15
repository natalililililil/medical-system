using MedicalSystem.Shared.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.Logout;

public record LogoutCommand(string RefreshToken) : ICommand<Unit>;