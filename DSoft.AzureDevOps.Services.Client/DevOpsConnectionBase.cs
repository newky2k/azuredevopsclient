using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using DSoft.AzureDevOps.Services.Client.Exceptions;

namespace DSoft.AzureDevOps.Services.Client
{
    /// <summary>
    /// DevOps Connection Base.
    /// Implements the <see cref="IDisposable" />
    /// Implements the <see cref="DSoft.AzureDevOps.Services.Client.IDevOpsConnection" />
    /// </summary>
    /// <seealso cref="IDisposable" />
    /// <seealso cref="DSoft.AzureDevOps.Services.Client.IDevOpsConnection" />
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class DevOpsConnectionBase : IDisposable, IDevOpsConnection
    {
        #region Fields
        private string _personalAccessToken;
        private string _organisationName;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the organisation.
        /// </summary>
        /// <value>The name of the organisation.</value>
        internal string OrganisationName
        {
            get { return _organisationName; }
            private set { _organisationName = value; }
        }

        /// <summary>
        /// Gets the personal access token.
        /// </summary>
        /// <value>The personal access token.</value>
        internal string PersonalAccessToken
        {
            get { return _personalAccessToken; }
            private set { _personalAccessToken = value; }
        }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        protected abstract string BaseUrl { get; }

        /// <summary>
        /// Gets the common URL.
        /// </summary>
        /// <value>The common URL.</value>
        internal abstract string CommonUrl { get; }

        /// <summary>
        /// Gets the feeds API URL.
        /// </summary>
        /// <value>The feeds API URL.</value>
        internal abstract string FeedsApiUrl { get; }

        /// <summary>
        /// Gets the nuget packages API URL.
        /// </summary>
        /// <value>The nuget packages API URL.</value>
        internal abstract string NugetPackagesApiUrl { get; }


        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DevOpsConnectionBase"/> class.
        /// </summary>
        /// <param name="organisationName">Name of the organisation.</param>
        /// <param name="personalAccessToken">The personal access token.</param>
        public DevOpsConnectionBase(string organisationName, string personalAccessToken)
        {
            _personalAccessToken = personalAccessToken;
            _organisationName = organisationName;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        /// <exception cref="DSoft.AzureDevOps.Services.Client.Exceptions.ConnectionFailureException"></exception>
        public T GetClient<T>() where T : DevOpsHttpClientBase
        {
            var typ = typeof(T);

            var client = (T)Activator.CreateInstance(typ, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new object[] { this }, null);

            var result = client.TryConnect();

            if (result == false)
                throw new ConnectionFailureException();

            return client;

        }

        /// <summary>
        /// Get client as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task&lt;T&gt; representing the asynchronous operation.</returns>
        /// <exception cref="DSoft.AzureDevOps.Services.Client.Exceptions.ConnectionFailureException"></exception>
        public async Task<T> GetClientAsync<T>(CancellationToken cancellationToken = default) where T : DevOpsHttpClientBase
        {
            var typ = typeof(T);

            var client = (T)Activator.CreateInstance(typ, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, new object[] { this }, null);

            var result = await client.TryConnectAsync();

            if (result == false)
                throw new ConnectionFailureException();


            return client;

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {

        }
        #endregion
    }
}
