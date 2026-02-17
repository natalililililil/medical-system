namespace AuthService.Application.Common.Exceptions;

public sealed class NotFoundException(string errorCode, string message) : BusinessException(errorCode, message) { }