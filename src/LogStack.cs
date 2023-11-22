using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace BetterStack.Logging;

internal sealed class LogStack : ILogStack, IDisposable
{
    private readonly BlockingCollection<LogEvent> collection = new();
    
    public void Add( LogEvent logEvent )
        => collection.TryAdd( logEvent );

    public void Dispose()
    {
        collection.Dispose();
    }

    public bool TryTake( [NotNullWhen( true )] out LogEvent? logEvent )
        => collection.TryTake( out logEvent );
}
