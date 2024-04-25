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
