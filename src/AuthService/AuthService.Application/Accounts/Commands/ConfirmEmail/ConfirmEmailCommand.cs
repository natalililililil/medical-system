using MedicalSystem.Shared.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.ConfirmEmail;

public record ConfirmEmailCommand(string Token) : ICommand<Unit>;