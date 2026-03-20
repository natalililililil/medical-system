using MedicalSystem.Shared.Contracts.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MedicalSystem.Shared.Exceptions;
using MedicalSystem.Shared.Interfaces;
using FluentValidation;

namespace MedicalSystem.Shared.Middleware;

public class ExceptionMiddleware(RequestDelegate _next, ILogger<ExceptionMiddleware> _logger)
{
    public async Task InvokeAsync(HttpContext context, IAuthCookieCleaner? _cookieCleaner = null)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = MapException(ex);

            if (ex is UnauthorizedException ue && ue.ErrorCode == "INVALID_REFRESH_TOKEN")
            {
                _cookieCleaner?.Clear(context.Response);
            }

            _logger.Log(response.LogLevel, ex, "Error. Code: {Code}. Path: {Path}", response.Code, context.Request.Path);

            context.Response.StatusCode = response.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new ErrorResponse(response.Code, response.Message));
        }
    }

    private static (int StatusCode, string Code, string Message, LogLevel LogLevel) MapException(Exception ex)
    {
        return ex switch
        {
            ValidationException => (400, "VALIDATION_ERROR", ex.Message, LogLevel.Warning),
            UnauthorizedException ue => (401, ue.ErrorCode, "Unauthorized", LogLevel.Warning),
            NotFoundException ne => (404, ne.ErrorCode, "Resource not found", LogLevel.Warning),
            ConflictException ce => (409, ce.ErrorCode, "Operation cannot be completed", LogLevel.Warning),
            _ => (500, "UNEXPECTED_ERROR", "Internal server error", LogLevel.Error)
        };
    }
}