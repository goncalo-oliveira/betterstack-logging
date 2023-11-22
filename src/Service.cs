using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BetterStack.Logging;

/// <summary>
/// A background service that sends log events to BetterStack in batches at a regular interval.
/// </summary>
internal sealed class LoggerService : BackgroundService
{
    private readonly IBetterStackLoggingClient client;
    private readonly BetterStackLoggingOptions options;
    private readonly ILogStack logStack;

    public LoggerService( IBetterStackLoggingClient client, ILogStack logStack, IOptions<BetterStackLoggingOptions> optionsAccessor )
    {
        this.client = client;
        this.logStack = logStack;

        // TODO: use options monitor instead
        options = optionsAccessor.Value;
    }

    protected override async Task ExecuteAsync( CancellationToken stoppingToken )
    {
        while ( !stoppingToken.IsCancellationRequested )
        {
            await Task.Delay( options.SendInterval, stoppingToken );

            var batch = new List<LogEvent>();

            while ( batch.Count < options.BatchLength && logStack.TryTake( out var logEvent ) )
            {
                batch.Add( logEvent );
            }

            if ( !batch.Any() )
            {
                continue;
            }

            try
            {
                await client.SendAsync( batch, CancellationToken.None );
            }
            catch ( Exception )
            {
                // TODO: log
            }
        }
    }
}
