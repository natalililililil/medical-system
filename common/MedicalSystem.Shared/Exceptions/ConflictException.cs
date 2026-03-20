namespace MedicalSystem.Shared.Exceptions;

public sealed class ConflictException(string errorCode, string message) : BusinessException(errorCode, message) { }