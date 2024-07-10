
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace BetterStack.Logging;

internal sealed class BetterStackLoggingClient : IBetterStackLoggingClient
{
    internal const string ClientName = "BetterStack.Client";

    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    private readonly HttpClient httpClient;
    private readonly ReadOnlyDictionary<string, string> metadata;

    public BetterStackLoggingClient( IHttpClientFactory httpClientFactory, IOptions<BetterStackLoggingOptions> optionsAccessor )
    {
        httpClient = httpClientFactory.CreateClient( ClientName );

        var options = optionsAccessor.Value;

        metadata = new ReadOnlyDictionary<string, string>( options.Metadata );
    }

    public async Task SendAsync( IEnumerable<LogEvent> logEvents, CancellationToken cancellationToken = default )
    {
        var logs = logEvents.Select( log =>
        {
            // include the client version with every log event
            log.Metadata["client.version"] = AppVersion.Value;

            foreach ( var kv in metadata )
            {
                if ( !log.Metadata.ContainsKey( kv.Key ) )
                {
                    log.Metadata[kv.Key] = kv.Value;
                }
            }

            return log;
        } )
        .ToArray();

        var content = new StringContent(
            JsonSerializer.Serialize( logs, jsonOptions ),
            Encoding.UTF8,
            "application/json"
        );

        try
        {
            _ = await httpClient.PostAsync( "/", content, cancellationToken );
        }
        catch ( Exception )
        {
            //
        }
    }
}
