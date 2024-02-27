using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSoft.AzureDevOps.Services.Client
{
    public interface IDevOpsConnection
    {
        T GetClient<T>() where T : DevOpsHttpClientBase;

        Task<T> GetClientAsync<T>(CancellationToken cancellationToken = default) where T : DevOpsHttpClientBase;
    }
}
