using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

class JsInteropMessageHandler : HttpMessageHandler
{
    private readonly IJSUnmarshalledRuntime _jSRuntime;
    private readonly ILogger<JsInteropMessageHandler> _logger;

    public JsInteropMessageHandler(IJSRuntime jSRuntime, ILogger<JsInteropMessageHandler> logger)
    {
        Console.WriteLine("Created Handler");
        _jSRuntime = (IJSUnmarshalledRuntime)jSRuntime;
        _logger = logger;
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        try {
            using var ms = new MemoryStream();
            request?.Content?.CopyTo(ms, null, cancellationToken);
            ms.Position = 0;
            _jSRuntime.InvokeUnmarshalled<byte[], int>("sendExportRequest", ms.ToArray());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        return new HttpResponseMessage(HttpStatusCode.OK);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
