using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Behaviors;

public class LogginBehavior<TRequest, TResponse>
    (ILogger<LogginBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "[START] Handle request={Request} - Response={Response} - RequestData={RequestData}", 
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        // Create a stopwatch to measure the execution time of the request handler
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await next();

        stopwatch.Stop();
        var timeTaken = stopwatch.Elapsed;

        if (timeTaken > TimeSpan.FromSeconds(3))
        {
            logger.LogWarning(
                "[SLOW PERFORMANCE] The request {Request} took {TimeTaken} seconds", 
                typeof(TRequest).Name, timeTaken.Seconds);
        }
        
        logger.LogInformation(
            "[END] Handled {Request} with {Response} - TimeTaken={TimeTaken}ms", 
            typeof(TRequest).Name, typeof(TResponse).Name, timeTaken.TotalMilliseconds);
        
        return response;
    }
}
