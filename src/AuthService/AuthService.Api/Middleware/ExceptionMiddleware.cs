using AuthService.Api.Contracts.Responses;
using AuthService.Application.Common.Exceptions;
using FluentValidation;

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
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error. Path: {Path}", context.Request.Path);

            await WriteBadRequest(context, ex.Message);
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, "Business error. Path: {Path}", context.Request.Path);

            await WriteBadRequest(context, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception. Path: {Path}", context.Request.Path);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new MessageResponse("Unexpected server error"));
        }
    }

    private static async Task WriteBadRequest(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new MessageResponse(message));
    }
}