using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LoDaTek.AzureDevOps.Tests.Helpers;

/// <summary>
/// Test message handler that returns a canned status code and records the last request,
/// so tests can exercise HTTP behaviour without a live server.
/// </summary>
public sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;

    /// <summary>
    /// Gets the most recent request seen by the handler.
    /// </summary>
    public HttpRequestMessage LastRequest { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StubHttpMessageHandler"/> class.
    /// </summary>
    /// <param name="statusCode">The status code to return for every request.</param>
    public StubHttpMessageHandler(HttpStatusCode statusCode)
    {
        _statusCode = statusCode;
    }

    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;

        return Task.FromResult(new HttpResponseMessage(_statusCode)
        {
            Content = new StringContent("{}"),
            RequestMessage = request,
        });
    }
}

/// <summary>
/// Minimal <see cref="IHttpClientFactory"/> that hands out clients backed by a supplied handler.
/// </summary>
public sealed class StubHttpClientFactory : IHttpClientFactory
{
    private readonly HttpMessageHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="StubHttpClientFactory"/> class.
    /// </summary>
    /// <param name="handler">The handler that backs every created client.</param>
    public StubHttpClientFactory(HttpMessageHandler handler)
    {
        _handler = handler;
    }

    /// <inheritdoc />
    public HttpClient CreateClient(string name) => new HttpClient(_handler);
}
