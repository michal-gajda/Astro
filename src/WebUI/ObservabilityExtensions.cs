namespace Astro.WebUI;

using System.Diagnostics;
using AspNetCore.SignalR.OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

internal static class ObservabilityExtensions
{
    private const string SERVICE_VERSION = "1.0.0";
    private const string SERVICE_INSTANCE_ID = "1.0.0";

    public static void AddObservability(this WebApplicationBuilder builder, string serviceName, string serviceNamespace)
    {
        builder.Services.AddHealthChecks();

        builder.Services.AddSignalR().AddHubInstrumentation();

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName, serviceNamespace, SERVICE_VERSION, autoGenerateServiceInstanceId: false, SERVICE_INSTANCE_ID);

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            options.ParseStateValues = true;
            options.AddOtlpExporter();
        });

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSignalRInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddOtlpExporter());

        builder.Services.AddSingleton(new ActivitySource(serviceName, SERVICE_VERSION));
    }

    public static void UseObservability(this WebApplication app)
    {
        app.UseHealthChecks("/health");
    }
}
