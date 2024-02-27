using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using DSoft.AzureDevOps.Services.Client.Exceptions;

namespace DSoft.AzureDevOps.Services.Client
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class DevOpsConnectionBase : IDisposable, IDevOpsConnection
    {
        #region Fields
        private string _personalAccessToken;
        private string _organisationName;
        #endregion

        #region Properties

        internal string OrganisationName
        {
            get { return _organisationName; }
            private set { _organisationName = value; }
        }

        internal string PersonalAccessToken
        {
            get { return _personalAccessToken; }
            private set { _personalAccessToken = value; }
        }

        protected abstract string BaseUrl { get; }

        internal abstract string CommonUrl { get; }

        internal abstract string FeedsApiUrl { get; }

        internal abstract string NugetPackagesApiUrl { get; }


        #endregion

        public DevOpsConnectionBase(string organisationName, string personalAccessToken)
        {
            _personalAccessToken = personalAccessToken;
            _organisationName = organisationName;
        }

        public T GetClient<T>() where T : DevOpsHttpClientBase
        {
            var typ = typeof(T);

            var client = (T)Activator.CreateInstance(typ, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new object[] { this }, null);

            var result = client.TryConnect();

            if (result == false)
                throw new ConnectionFailureException();

            return client;

        }

        public async Task<T> GetClientAsync<T>(CancellationToken cancellationToken = default) where T : DevOpsHttpClientBase
        {
            var typ = typeof(T);

            var client = (T)Activator.CreateInstance(typ, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new object[] { this }, null);

            var result = await client.TryConnectAsync();

            if (result == false)
                throw new ConnectionFailureException();


            return client;

        }

        public virtual void Dispose()
        {

        }
    }
}
