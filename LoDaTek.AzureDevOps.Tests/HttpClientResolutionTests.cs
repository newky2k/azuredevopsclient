using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LoDaTek.AzureDevOps.Services.Client;
using LoDaTek.AzureDevOps.Services.Client.Connections;
using LoDaTek.AzureDevOps.Services.Client.Exceptions;
using LoDaTek.AzureDevOps.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoDaTek.AzureDevOps.Tests;

/// <summary>
/// Exercises connection / client behaviour offline by injecting a stub
/// <see cref="System.Net.Http.IHttpClientFactory"/> via the new factory support.
/// </summary>
[TestClass]
public class HttpClientResolutionTests
{
    private const string Org = "contoso";
    private const string Pat = "secret-pat";

    [TestMethod]
    public async Task GetClientAsync_SuccessStatus_ReturnsConnectedClient()
    {
        var handler = new StubHttpMessageHandler(HttpStatusCode.OK);
        var connection = new AzureDevOpsCloudConnection(Org, Pat)
        {
            HttpClientFactory = new StubHttpClientFactory(handler),
        };

        var client = await connection.GetClientAsync<SecureFilesHttpClient>();

        Assert.IsNotNull(client);
        Assert.IsNotNull(handler.LastRequest, "TryConnect should have issued a request.");
    }

    [TestMethod]
    public async Task GetClientAsync_NotFound_ThrowsConnectionFailure()
    {
        var handler = new StubHttpMessageHandler(HttpStatusCode.NotFound);
        var connection = new AzureDevOpsCloudConnection(Org, Pat)
        {
            HttpClientFactory = new StubHttpClientFactory(handler),
        };

        await Assert.ThrowsExceptionAsync<ConnectionFailureException>(
            () => connection.GetClientAsync<SecureFilesHttpClient>());
    }

    [TestMethod]
    public async Task GetClientAsync_AppliesBasicAuthHeaderFromPat()
    {
        var handler = new StubHttpMessageHandler(HttpStatusCode.OK);
        var connection = new AzureDevOpsCloudConnection(Org, Pat)
        {
            HttpClientFactory = new StubHttpClientFactory(handler),
        };

        await connection.GetClientAsync<SecureFilesHttpClient>();

        var auth = handler.LastRequest.Headers.Authorization;
        var expected = Convert.ToBase64String(Encoding.ASCII.GetBytes(":" + Pat));

        Assert.AreEqual("Basic", auth.Scheme);
        Assert.AreEqual(expected, auth.Parameter);
    }
}
