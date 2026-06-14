using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using LoDaTek.AzureDevOps.Services.Client.Enums;

namespace LoDaTek.AzureDevOps.Services.Client.Bases;

/// <summary>
/// Class DevOpsHttpClientBase.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class DevOpsHttpClientBase
{
    #region Fields

    private DevOpsConnectionBase _connection;
    private ApiType _apiType;
    private HttpClient _client;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the credentials.
    /// </summary>
    /// <value>The credentials.</value>
    private string credentials => Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _connection.PersonalAccessToken)));

    /// <summary>
    /// Gets the test URL.
    /// </summary>
    /// <value>The test URL.</value>
    internal abstract string TestUrl { get; }

    /// <summary>
    /// Gets the connection.
    /// </summary>
    /// <value>The connection.</value>
    protected DevOpsConnectionBase Connection => _connection;

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <value>The client.</value>
    protected HttpClient Client
    {
        get
        {
            var factory = _connection.HttpClientFactory;

            // Factory clients are pooled and short-lived: resolve a fresh one each
            // time and never cache or dispose it (the factory owns the handler).
            if (factory != null)
                return ConfigureClient(factory.CreateClient(DevOpsConnectionBase.HttpClientName));

            return _client ??= BuildAuthenticationClient(ApiUrl);
        }
    }

    /// <summary>
    /// Gets the API URL.
    /// </summary>
    /// <value>The API URL.</value>
    protected string ApiUrl
    {
        get
        {
            switch (_apiType)
            {
                case ApiType.Feeds:
                    return _connection.FeedsApiUrl;
                case ApiType.Nuget:
                    return _connection.NugetPackagesApiUrl;
                default:
                    return _connection.CommonUrl;
            }
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="DevOpsHttpClientBase"/> class.
    /// </summary>
    /// <param name="connection">The connection.</param>
    /// <param name="apiType">Type of the API.</param>
    internal DevOpsHttpClientBase(DevOpsConnectionBase connection, ApiType apiType)
    {
        _connection = connection;
        _apiType = apiType;
    }


    #endregion

    #region Methods

    /// <summary>
    /// Tries the connect.
    /// </summary>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    internal bool TryConnect() => TryConnectAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Try connect as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    internal async Task<bool> TryConnectAsync()
    {
        try
        {
            var response = await Client.GetAsync(TestUrl);

            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return false;
                default:
                    return true;
            }
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Applies the base address (when unset), accept and authorization headers.
    /// Used for both self-built and factory-provided clients.
    /// </summary>
    /// <param name="client">The client to configure.</param>
    /// <returns>HttpClient.</returns>
    private HttpClient ConfigureClient(HttpClient client)
    {
        if (client.BaseAddress == null)
            client.BaseAddress = new Uri(ApiUrl);

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

        return client;
    }

    /// <summary>
    /// Builds a self-managed authenticated HTTP client (used when no factory is set).
    /// </summary>
    /// <param name="url">The base URL.</param>
    /// <returns>HttpClient.</returns>
    private HttpClient BuildAuthenticationClient(string url)
    {
        var handler = new HttpClientHandler { AllowAutoRedirect = false };

        var client = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
        client.BaseAddress = new Uri(url);

        return ConfigureClient(client);
    }

    /// <summary>
    /// Disposes this instance.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void Dispose()
    {
        _client?.Dispose();
        _client = null;
    }

    #endregion
}
