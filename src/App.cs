using System.Reflection;

namespace BetterStack.Logging;

internal static class App
{
    private static readonly Lazy<string> version = new(
        () => Assembly.GetExecutingAssembly().GetName().Version?.ToString( 3 ) ?? "0.0.0"
    );

    public static string Version => version.Value;
}
