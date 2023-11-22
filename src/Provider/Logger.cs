using Microsoft.Extensions.Logging;

namespace BetterStack.Logging;

/// <summary>
/// A logger that logs to a <see cref="ILogStack"/> singleton.
/// </summary>
internal sealed class BetterStackLogger : ILogger
{
    private readonly string name;
    private readonly ILogStack stack;

    private static readonly string[] excluded = new[]
    {
        "BetterStack.Logging.",
        $"System.Net.Http.HttpClient.{BetterStackLoggingClient.ClientName}.",
    };

    public BetterStackLogger( string categoryName, ILogStack logStack )
    {
        name = categoryName;
        stack = logStack;
    }

    public IDisposable? BeginScope<TState>( TState state ) where TState : notnull
    {
        return default!;
    }

    public bool IsEnabled( LogLevel logLevel ) => logLevel > LogLevel.Trace;

    public void Log<TState>( LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter )
    {
        if ( !IsEnabled( logLevel ) )
        {
            return;
        }

        /*
        exclude the betterstack logging namespace
        */
        if ( excluded.Any( prefix => name.StartsWith( prefix ) ) )
        {
            return;
        }

        var message = formatter( state, exception );
        var metadata = new Dictionary<string, string>();

        /*
        include exception details if present
        */
        if ( exception != null )
        {
            metadata["exception.message"] = exception.Message;
            metadata["exception.stacktrace"] = exception.StackTrace ?? string.Empty;
        }

        stack.Add( new LogEvent
        {
            Category = name,
            Level = logLevel.ToString(),
            Message = message,
            Metadata = metadata,
        } );
    }
}
