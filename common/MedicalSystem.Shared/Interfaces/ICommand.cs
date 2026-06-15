using MediatR;

namespace MedicalSystem.Shared.Interfaces;

public interface ICommand<out TResponse> : IRequest<TResponse> { }