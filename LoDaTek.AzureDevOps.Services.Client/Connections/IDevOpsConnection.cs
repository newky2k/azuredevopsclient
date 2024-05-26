using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LoDaTek.AzureDevOps.Services.Client.Bases;

namespace LoDaTek.AzureDevOps.Services.Client.Connections
{
    /// <summary>
    /// DevOps Connection Interface
    /// </summary>
    public interface IDevOpsConnection
    {
        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        T GetClient<T>() where T : DevOpsHttpClientBase;

        /// <summary>
        /// Gets the client asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> GetClientAsync<T>(CancellationToken cancellationToken = default) where T : DevOpsHttpClientBase;
    }
}
