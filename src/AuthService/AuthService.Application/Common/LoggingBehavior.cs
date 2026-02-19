using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AuthService.Application.Common;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> _logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Handling {RequestName}", requestName);

        var response = await next();
        stopwatch.Stop();

        _logger.LogInformation("Handled {RequestName} in {Elapsed}ms", requestName, stopwatch.ElapsedMilliseconds);

        return response;
    }
}
