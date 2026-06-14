using System;
using System.Net.Http;
using LoDaTek.AzureDevOps.Services.Client.Bases;
using LoDaTek.AzureDevOps.Services.Client.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoDaTek.AzureDevOps.Tests;

/// <summary>
/// Verifies the DI registration helper wires up the named client used by the REST clients.
/// </summary>
[TestClass]
public class ServiceCollectionExtensionsTests
{
    [TestMethod]
    public void AddAzureDevOpsHttpClient_RegistersNamedClientWithConfiguredTimeout()
    {
        var provider = new ServiceCollection()
            .AddAzureDevOpsHttpClient()
            .BuildServiceProvider();

        var factory = provider.GetService<IHttpClientFactory>();
        Assert.IsNotNull(factory, "IHttpClientFactory should be registered.");

        var client = factory.CreateClient(DevOpsConnectionBase.HttpClientName);
        Assert.AreEqual(TimeSpan.FromSeconds(30), client.Timeout);
    }

    [TestMethod]
    public void AddAzureDevOpsHttpClient_ReturnsSameCollectionForChaining()
    {
        var services = new ServiceCollection();

        var result = services.AddAzureDevOpsHttpClient();

        Assert.AreSame(services, result);
    }
}
