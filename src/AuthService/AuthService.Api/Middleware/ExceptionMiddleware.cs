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
            var (statusCode, message, logLevel) = ex switch
            {
                ValidationException => (400, ex.Message, LogLevel.Warning),
                UnauthorizedException => (401, ex.Message, LogLevel.Warning),
                NotFoundException => (404, "Resource not found", LogLevel.Warning),
                ConflictException => (409, "Operation cannot be completed", LogLevel.Warning),
                BusinessException => (400, "Business logic error", LogLevel.Warning),
                DbUpdateException => (500, "Database update error", LogLevel.Error),
                _ => (500, "Unexpected server error", LogLevel.Error)
            };

            _logger.Log(logLevel, ex, "Error. Path: {Path}", context.Request.Path);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new MessageResponse(message));
        }
    }
}