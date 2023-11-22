using System.Net.Http.Headers;
using BetterStack.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class BetterStackLoggerServiceExtensions
{
    /// <summary>
    /// Adds BetterStack logging to the application.
    /// </summary>
    /// <param name="sourceToken">The source token to use when sending log events.</param>
    public static ILoggingBuilder AddBetterStack( this ILoggingBuilder builder, string sourceToken )
        => builder.AddBetterStack( options => options.SourceToken = sourceToken );

    /// <summary>
    /// Adds BetterStack logging to the application.
    /// </summary>
    /// <param name="configure">A delegate to configure the <see cref="BetterStackLoggingOptions"/>.</param>
    /// <exception cref="InvalidOperationException">Thrown if the source token is not configured.</exception>
    public static ILoggingBuilder AddBetterStack( this ILoggingBuilder builder, Action<BetterStackLoggingOptions> configure )
    {
        builder.Services.TryAddEnumerable( ServiceDescriptor.Singleton<ILoggerProvider, BetterStackLoggerProvider>() );
        builder.Services.Configure( configure );

        builder.Services.AddHttpClient( BetterStackLoggingClient.ClientName, httpClient =>
        {
            var options = new BetterStackLoggingOptions();

            configure( options );

            /*
            The source token is required to send log events.
            */
            if ( options.SourceToken == null )
            {
                throw new InvalidOperationException( "Source token is required." );
            }

            /*
            The minimum send interval is 1 second.
            Anything less than that will be reset to the minimum.
            */
            if ( options.SendInterval < TimeSpan.FromSeconds( 1 ) )
            {
                options.SendInterval = TimeSpan.FromSeconds( 1 );
            }

            httpClient.BaseAddress = new Uri( options.Url );
            httpClient.DefaultRequestHeaders.Add( "Accept", "application/json" );
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", options.SourceToken );
        } );

        builder.Services.AddTransient<IBetterStackLoggingClient, BetterStackLoggingClient>();
        builder.Services.AddSingleton<ILogStack, LogStack>();
        builder.Services.AddHostedService<LoggerService>();

        builder.AddFilter( "BetterStack.Logging.*", LogLevel.Warning );
        builder.AddFilter( $"System.Net.Http.HttpClient.{BetterStackLoggingClient.ClientName}.*", LogLevel.Warning );
        
        return builder;
    }
}
