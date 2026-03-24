using MedicalSystem.Shared.Interfaces;
using MediatR;

namespace AuthService.Application.Accounts.Commands.RegisterAccount;

public record RegisterAccountCommand(string Email, string Password, string ConfirmPassword) : ICommand<Unit>;