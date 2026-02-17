namespace AuthService.Application.Common.Exceptions;

public sealed class UnauthorizedException(string errorCode, string message) : BusinessException(errorCode, message) { }
