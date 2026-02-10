namespace AuthService.Application.Common.Exceptions;

public sealed class NotFoundException(string message) : BusinessException(message) { }