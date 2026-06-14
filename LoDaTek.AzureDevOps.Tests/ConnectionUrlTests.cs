using LoDaTek.AzureDevOps.Services.Client.Connections;
using LoDaTek.AzureDevOps.Services.Client.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoDaTek.AzureDevOps.Tests;

/// <summary>
/// Verifies each connection subclass builds the correct URLs for its host shape.
/// </summary>
[TestClass]
public class ConnectionUrlTests
{
    private const string Org = "contoso";
    private const string Pat = "token";

    [TestMethod]
    public void CloudConnection_BuildsExpectedUrls()
    {
        var connection = new AzureDevOpsCloudConnection(Org, Pat);

        Assert.AreEqual(ServerType.AzureDevOps, connection.ServerType);
        Assert.AreEqual("https://dev.azure.com/contoso/", connection.CommonUrl);
        Assert.AreEqual("https://feeds.dev.azure.com/contoso/_apis/", connection.FeedsApiUrl);
        Assert.AreEqual("https://pkgs.dev.azure.com/contoso/", connection.NugetPackagesApiUrl);
    }

    [TestMethod]
    public void LegacyCloudConnection_BuildsExpectedUrls()
    {
        var connection = new AzureDevOpsLegacyCloudConnection(Org, Pat);

        Assert.AreEqual(ServerType.AzureDevOps, connection.ServerType);
        Assert.AreEqual("https://contoso.visualstudio.com/", connection.CommonUrl);
        Assert.AreEqual("https://contoso.feeds.visualstudio.com/_apis/", connection.FeedsApiUrl);
        Assert.AreEqual("https://contoso.pkgs.visualstudio.com/", connection.NugetPackagesApiUrl);
    }

    [TestMethod]
    public void ServerConnection_BuildsExpectedUrls()
    {
        var connection = new AzureDevOpsServerConnection("https://devops.local", "DefaultCollection", Pat);

        Assert.AreEqual(ServerType.DevOpsServer, connection.ServerType);
        Assert.AreEqual("https://devops.local/DefaultCollection/", connection.CommonUrl);
        Assert.AreEqual("https://devops.local/DefaultCollection//_apis/", connection.FeedsApiUrl);
    }

    [TestMethod]
    public void Connection_RetainsOrganisationAndToken()
    {
        var connection = new AzureDevOpsCloudConnection(Org, Pat);

        Assert.AreEqual(Org, connection.OrganisationName);
        Assert.AreEqual(Pat, connection.PersonalAccessToken);
    }
}
