using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace BlazorOpenTelemetryExporter;
public static class TracerProviderExtensions
{
    public static TracerProviderBuilder AddJsInteropExporter(this TracerProviderBuilder builder, IServiceProvider serviceProvider)
    {
        builder.AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
            otlpOptions.ExportProcessorType = ExportProcessorType.Simple;
            otlpOptions.HttpClientFactory = () => {
                return new HttpClient(
                    new JsInteropMessageHandler(
                        serviceProvider.GetRequiredService<IJSRuntime>(),
                        serviceProvider.GetRequiredService<ILogger<JsInteropMessageHandler>>()
                ), false) { BaseAddress = new Uri("http://localhost") };
            };
        });
        return builder;
    }
}
