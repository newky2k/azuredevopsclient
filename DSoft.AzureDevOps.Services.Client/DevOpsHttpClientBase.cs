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
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class DevOpsHttpClientBase
    {
        private DevOpsConnectionBase _connection;
        private ApiType _apiType;

        private string credentials => Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _connection.PersonalAccessToken)));

        internal abstract string TestUrl {get;}

        protected DevOpsConnectionBase Connection => _connection;

        protected HttpClient Client
        {
            get
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(ApiUrl);  //url of your organization
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                return client;
            }
        }

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

        internal DevOpsHttpClientBase(DevOpsConnectionBase connection, ApiType apiType)
        {
            _connection = connection;
            _apiType = apiType;
        }

        internal bool TryConnect()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(TestUrl);
                request.Timeout = 3000;
                request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                request.Method = "Get";
                request.Headers.Add("Authorization", "Basic " + credentials);

                using (var response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        internal async Task<bool> TryConnectAsync()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(TestUrl);
                request.Timeout = 3000;
                request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                request.Method = "Get";
                request.Headers.Add("Authorization", "Basic " + credentials);

                using (var response = await request.GetResponseAsync())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private HttpClient BuildAuthenticationClient(string url)
        {
            var client = new HttpClient();

            client.BaseAddress = new Uri(url);  //url of your organization
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            return client;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
