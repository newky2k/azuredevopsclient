using System;
using System.Linq;
using System.Threading.Tasks;
using LoDaTek.AzureDevOps.Services.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoDaTek.AzureDevOps.Tests.Integration;

/// <summary>
/// Read-only integration tests for the feed and package REST clients.
/// Gated on AZDO_ORG / AZDO_PAT; skipped (inconclusive) when not configured.
/// </summary>
[TestClass]
[TestCategory("Integration")]
public class FeedAndPackageIntegrationTests
{
    [TestMethod]
    public async Task GetFeedsAsync_ReturnsFeedList()
    {
        var connection = IntegrationConfig.RequireConnection();

        var client = await connection.GetClientAsync<FeedManagmentHttpClient>();
        var feeds = await client.GetFeedsAsync();

        Assert.IsNotNull(feeds, "Expected a (possibly empty) feed list, not null.");
    }

    [TestMethod]
    public async Task GetPackagesAsync_ForFirstFeed_ReturnsPackageList()
    {
        var connection = IntegrationConfig.RequireConnection();

        var client = await connection.GetClientAsync<FeedManagmentHttpClient>();
        var feeds = await client.GetFeedsAsync();

        if (feeds == null || feeds.Count == 0)
            Assert.Inconclusive("No feeds available in the organisation to query packages for.");

        var packages = await client.GetPackagesAsync(feeds[0].Id);

        Assert.IsNotNull(packages, "Expected a (possibly empty) package list, not null.");
    }

    [TestMethod]
    public async Task GetNugetBasePathAsync_ForNamedFeed_ReturnsBaseUrl()
    {
        var connection = IntegrationConfig.RequireConnection();
        var feedName = IntegrationConfig.RequireValue(IntegrationConfig.Feed, "AZDO_FEED");

        var client = await connection.GetClientAsync<PackageManagementHttpClient>();
        var baseUrl = await client.GetNugetBasePathAsync(feedName, IntegrationConfig.Project);

        Assert.IsFalse(string.IsNullOrWhiteSpace(baseUrl), "Expected a non-empty NuGet base path.");
        StringAssert.StartsWith(baseUrl, "http");
    }
}
