namespace BetterStack.Logging;

public sealed class BetterStackLoggingOptions
{
    /// <summary>
    /// The number of log events to send in a single batch. Defaults to 1000.
    /// </summary>
    public int BatchLength { get; set; } = 1000;

    /// <summary>
    /// Metadata to include with every log event.
    /// </summary>
    public Dictionary<string, string> Metadata { get; set; } = new();

    /// <summary>
    /// The interval at which to send batches of log events (minimum 1 second). Defaults to 2 seconds.
    /// </summary>
    public TimeSpan SendInterval { get; set; } = TimeSpan.FromSeconds( 2 );

    /// <summary>
    /// The source token to use when sending log events.
    /// </summary>
    public string? SourceToken { get; set; }

    /// <summary>
    /// The URL to send log events to. Defaults to https://in.logs.betterstack.com.
    /// </summary>
    public string Url { get; set; } = "https://in.logs.betterstack.com";



}
