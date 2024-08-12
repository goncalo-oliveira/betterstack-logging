using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BetterStack.Logging;

/// <summary>
/// Represents a logger provider that creates <see cref="BetterStackLogger"/> instances.
/// </summary>
internal sealed class BetterStackLoggerProvider : ILoggerProvider, ISupportExternalScope
{
    private readonly ILogStack logStack;
    private readonly ConcurrentDictionary<string, ILogger> loggers = new();

    public BetterStackLoggerProvider( IServiceProvider serviceProvider )
    {
        logStack = serviceProvider.GetRequiredService<ILogStack>();
    }

    public IExternalScopeProvider? ScopeProvider { get; private set; }

    public ILogger CreateLogger( string categoryName )
        => loggers.GetOrAdd( categoryName, name => new BetterStackLogger( name, logStack, ScopeProvider ) );

    public void Dispose()
    {
        loggers.Clear();
    }

    public void SetScopeProvider( IExternalScopeProvider scopeProvider )
        => ScopeProvider = scopeProvider;
}
