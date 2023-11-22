using System.Diagnostics.CodeAnalysis;

namespace BetterStack.Logging;

/// <summary>
/// Represents a stack of log events.
/// </summary>
public interface ILogStack
{
    /// <summary>
    /// Adds a log event to the stack.
    /// </summary>
    void Add( LogEvent logEvent );

    /// <summary>
    /// Attempts to take a log event from the stack.
    /// </summary>
    /// <param name="logEvent">The log event taken, or null if no log event was taken.</param>
    /// <returns>True if a log event was taken, false otherwise.</returns>
    bool TryTake( [NotNullWhen( true )] out LogEvent? logEvent );
}
