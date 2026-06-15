namespace MedicalSystem.Shared.Exceptions;

public sealed class NotFoundException(string errorCode, string message) : BusinessException(errorCode, message) { }