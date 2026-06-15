namespace MedicalSystem.Shared.Exceptions;

public sealed class UnauthorizedException(string errorCode, string message) : BusinessException(errorCode, message) { }
