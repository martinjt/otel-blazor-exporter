using System.Diagnostics;
using BlazorOpenTelemetryExporter;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using sample;

var appBuilder = WebAssemblyHostBuilder.CreateDefault(args);
appBuilder.RootComponents.Add<App>("#app");
appBuilder.RootComponents.Add<HeadOutlet>("head::after");

appBuilder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(appBuilder.HostEnvironment.BaseAddress) });

appBuilder.Services.AddSingleton(sp =>
{
    return Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("blazor-wasm"))
        .AddSource(ActivityHelper.Source.Name)
        .AddJsInteropExporter(sp)
        .Build();
});

var host = appBuilder.Build();
var tracer = host.Services.GetRequiredService<TracerProvider>();

await host.RunAsync();

static class ActivityHelper
{
    public static ActivitySource Source = new ActivitySource("Blazor Source");
}