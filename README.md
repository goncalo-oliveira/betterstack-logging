# BetterStack Logger

This project contains a simple logger for the [BetterStack](https://betterstack.com/) logging system.

## Installation

The package can be installed from NuGet:

```
dotnet add package Faactory.BetterStack.Logging
```

## Usage

The logger can be used as any other logger in .NET Core.

```csharp
IServiceCollection services = ...;

services.AddLogging( logging =>
{
    logging.AddBetterStack( configuration["SourceToken"] );
} );
```

As a minimum, you need to provide the source token for your application. You can also customize the logger by providing a delegate to configure the logger options.

```csharp
IServiceCollection services = ...;

services.AddLogging( logging =>
{
    logging.AddBetterStack( options =>
    {
        options.SourceToken = configuration["SourceToken"];

        /*
        The values below are the default values. Showing them here for reference.
        */
        options.BatchLength = 1000;
        options.SendInterval = TimeSpan.FromSeconds( 2 );

        /*
        You can also provide custom metadata to be sent with each log event.
        */
        options.Metadata.Add( "app.version", "1.0" );
    } );
} );
```
