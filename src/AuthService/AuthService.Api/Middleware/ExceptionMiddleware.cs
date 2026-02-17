using AuthService.Api.Contracts.Responses;
using AuthService.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = MapException(ex);

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
            DbUpdateException => (500, "DATABASE_ERROR", "Internal server error", LogLevel.Error),
            _ => (500, "UNEXPECTED_ERROR", "Internal server error", LogLevel.Error)
        };
    }
}