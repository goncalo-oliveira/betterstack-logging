namespace BetterStack.Logging;

/// <summary>
/// A client for sending log events to BetterStack.
/// </summary>
public interface IBetterStackLoggingClient
{
    /// <summary>
    /// Sends the given log events to BetterStack.
    /// </summary>
    /// <param name="logEvents">The log events to send.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task SendAsync( IEnumerable<LogEvent> logEvents, CancellationToken cancellationToken = default );
}
