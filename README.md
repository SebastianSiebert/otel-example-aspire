# otel-example

## Setup

Install .NET 8 SDK

Install Docker

Install the Aspire workload

    dotnet workload update
    dotnet workload install aspire
    dotnet workload list


Trust dev certificate, Aspire Dashboard will otherwise not run correctly

    dotnet dev-certs https --trust

Pre-Build Containers and pull docker images

    docker-compose build
    docker-compose pull

## Usage

Confirm that Docker is running

Start OTel.Example.AppHost with https

Create Manifest

   dotnet run --project .\OTel.Example.AppHost\OTel.Example.AppHost.csproj -- --publisher manifest --output-path ../aspire-manifest.json

In Deployment set ```OTEL_EXPORTER_OTLP_ENDPOINT``` to enable Open Telemetry export
```OTEL_SERVICE_NAME``` for names in traces/metrics

Using Docker-Compose

    docker-compose up --detach
    docker-compose stop
    docker-compose start
    docker-compose down


## Adding Tracs and Metrics

In OTel.Example.ServiceDefaults in  class Extensions and Method ConfigureOpenTelemetry to enable own creation of Metrics and Traces to the following:

Within With.Metrics add

    metrics.SetResourceBuilder(ResourceBuilder.CreateDefault())
                    .AddMeter("MeterName1", "MeterName2", ...);

Within WithTracing add

    tracing.SetResourceBuilder(ResourceBuilder.CreateDefault())
                    .AddSource("SourceName1", "SourceName2", ...);

### For Metrics in Project:

- Create new class
    - Add Meters as properties or fields
    - Inject IMeterFactory in Constructor
        - Create Meter from Factory, MeterName is the same as in .AddMeter(...) above
        - Initialize Meters
    - Write Methods use Meters
- Register class as Singleton in Program.cs
- Inject class in Service and call methods to use

Example Metrics class
```
public class MyMetrics
{
    private Counter<int> AddedCounter { get; }
    private Counter<int> DeletedCounter { get; }
    private UpDownCounter<int> TotalsPostUpDownCounter { get; }
    
    public PostMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("MeterName");

        PostAddedCounter = meter.CreateCounter<int>("x-added", "Unit");
        PostDeletedCounter = meter.CreateCounter<int>("x-deleted", "Unit");
        TotalsPostUpDownCounter = meter.CreateUpDownCounter<int>("total-x", "Unit");
    }

    public void AddX() => AddedCounter.Add(1);
    public void DeleteX() => DeletedCounter.Add(1);
    public void IncreaseTotalX() => TotalsPostUpDownCounter.Add(1);
    public void DecreaseTotalX() => TotalsPostUpDownCounter.Add(-1);
}
```

### For Traces in Project

- Create Static class
    - Add ActivitySource, Same name as in .AddSource(...)
- Use ActivitySource to create Trace
- Optional: Add Events, Tags, etc.

Example
```
public static class MySource
{
    public static readonly ActivitySource MyActivitySource = new("SourceName");
}

using var activity = MySource.MyActivitySource.StartActivity("My Trace");
activity?.SetTag("x.id", id);
activity?.AddEvent(new("My Event"));
```
