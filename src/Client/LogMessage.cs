using System.Text.Json.Serialization;

namespace BetterStack.Logging;

/// <summary>
/// Represents a log event.
/// </summary>
public sealed class LogEvent
{
    [JsonPropertyName( "dt" )]
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// The category of the log event.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// The level of the log event.
    /// </summary>
    public string? Level { get; set; }

    /// <summary>
    /// The message of the log event.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Metadata associated with the log event.
    /// </summary>
    public IDictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );
}
