# Astro

```powershell
dotnet new sln --format slnx --name Astro
```

```powershell
dotnet new blazor --framework net9.0 --no-https --use-program-main --output src/WebUI --name Astro.WebUI
dotnet sln add src/WebUI
```

```powershell
dotnet add src/WebUI package OpenTelemetry.Extensions.Hosting
dotnet add src/WebUI package OpenTelemetry.Exporter.OpenTelemetryProtocol
dotnet add src/WebUI package OpenTelemetry.Instrumentation.AspNetCore
dotnet add src/WebUI package OpenTelemetry.Instrumentation.Http
dotnet add src/WebUI package OpenTelemetry.Instrumentation.Runtime
dotnet add src/WebUI package AspNetCore.SignalR.OpenTelemetry
```
