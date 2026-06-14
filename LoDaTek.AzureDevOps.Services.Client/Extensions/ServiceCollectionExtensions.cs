using System;
using System.Net.Http;
using LoDaTek.AzureDevOps.Services.Client.Bases;
using Microsoft.Extensions.DependencyInjection;

namespace LoDaTek.AzureDevOps.Services.Client.Extensions;

/// <summary>
/// Dependency injection helpers for the Azure DevOps REST clients.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the named <see cref="HttpClient"/> (<see cref="DevOpsConnectionBase.HttpClientName"/>)
    /// used by the REST clients, configured to match the built-in client
    /// (redirects disabled, 30 second timeout).
    /// Assign the resolved <c>IHttpClientFactory</c> to
    /// <see cref="DevOpsConnectionBase.HttpClientFactory"/> on each connection to use it.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection, for chaining.</returns>
    public static IServiceCollection AddAzureDevOpsHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(DevOpsConnectionBase.HttpClientName)
            .ConfigureHttpClient(client => client.Timeout = TimeSpan.FromSeconds(30))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });

        return services;
    }
}
