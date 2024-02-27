using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DSoft.AzureDevOps.Services.Client
{
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
        internal abstract string TestUrl {get;}

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
                var client = BuildAuthenticationClient(ApiUrl);

                return client;
            }
        }

        /// <summary>
        /// Gets the web client.
        /// </summary>
        /// <value>The web client.</value>
        protected WebClient WebClient
        {
            get
            {
                return new WebClient()
                {
                    Credentials = new NetworkCredential("username", _connection.PersonalAccessToken),
                };
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
        internal bool TryConnect()
        {
            try
            {
                var client = BuildAuthenticationClient();
                client.Timeout = TimeSpan.FromSeconds(3000);

                var task = Task.Run(() => client.GetAsync(TestUrl));
                task.Wait();
                var response = task.Result;

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Try connect as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
        internal async Task<bool> TryConnectAsync()
        {
            try
            {
                var client = BuildAuthenticationClient();
                client.Timeout = TimeSpan.FromSeconds(3000);

                var result = await client.GetAsync(TestUrl);

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                return true;
 
            }
            catch
            {
                return false;
            }
        }

        private HttpClient BuildAuthenticationClient()
        {
            var handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;

            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            return client;
        }

        /// <summary>
        /// Builds the authentication client.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>HttpClient.</returns>
        private HttpClient BuildAuthenticationClient(string url)
        {
            var client = BuildAuthenticationClient();

            client.BaseAddress = new Uri(url);
           
            return client;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Dispose()
        {
            
        }

        #endregion
    }
}
