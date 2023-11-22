using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BetterStack.Logging;

/// <summary>
/// Represents a logger provider that creates <see cref="BetterStackLogger"/> instances.
/// </summary>
internal sealed class BetterStackLoggerProvider : ILoggerProvider
{
    private readonly ILogStack logStack;
    private readonly ConcurrentDictionary<string, ILogger> loggers = new();

    public BetterStackLoggerProvider( IServiceProvider serviceProvider )
    {
        logStack = serviceProvider.GetRequiredService<ILogStack>();
    }

    public ILogger CreateLogger( string categoryName )
        => loggers.GetOrAdd( categoryName, name => new BetterStackLogger( name, logStack ) );

    public void Dispose()
    {
        loggers.Clear();
    }
}
