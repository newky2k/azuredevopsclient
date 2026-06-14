using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoDaTek.AzureDevOps.Tests.Integration;

/// <summary>
/// Read-only integration tests for <see cref="LoDaTek.AzureDevOps.Client.AzureDevOpsProvider"/>.
/// Gated on AZDO_ORG / AZDO_PAT; skipped (inconclusive) when not configured.
/// </summary>
[TestClass]
[TestCategory("Integration")]
public class ProviderIntegrationTests
{
    [TestMethod]
    public async Task GetProjectsAsync_ReturnsProjects()
    {
        var provider = IntegrationConfig.RequireProvider();

        var projects = await provider.GetProjectsAsync();

        Assert.IsNotNull(projects, "Expected a (possibly empty) project list, not null.");
    }

    [TestMethod]
    public async Task GetProjectAsync_ForNamedProject_ReturnsProject()
    {
        var provider = IntegrationConfig.RequireProvider();
        var projectName = IntegrationConfig.RequireValue(IntegrationConfig.Project, "AZDO_PROJECT");

        var project = await provider.GetProjectAsync(projectName);

        Assert.IsNotNull(project, $"Project '{projectName}' was not found.");
        Assert.AreEqual(projectName, project.Name);
    }

    [TestMethod]
    public async Task FetchAvailableFeedsAsync_ReturnsFeeds()
    {
        var provider = IntegrationConfig.RequireProvider();

        var feeds = await provider.FetchAvailableFeedsAsync();

        Assert.IsNotNull(feeds, "Expected a (possibly empty) feed list, not null.");
    }

    [TestMethod]
    public async Task SecureFiles_ForFirstProject_ReturnsList()
    {
        var provider = IntegrationConfig.RequireProvider();

        var projects = await provider.GetProjectsAsync();

        if (projects == null || projects.Count == 0)
            Assert.Inconclusive("No projects available to query secure files for.");

        var project = string.IsNullOrWhiteSpace(IntegrationConfig.Project)
            ? projects.First()
            : projects.First(p => p.Name == IntegrationConfig.Project);

        var secureFiles = await provider.SecureFilesClient.GetAllAsync(project);

        Assert.IsNotNull(secureFiles, "Expected a (possibly empty) secure file list, not null.");
    }
}
