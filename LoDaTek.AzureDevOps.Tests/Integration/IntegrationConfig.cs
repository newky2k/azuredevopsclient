using System;
using LoDaTek.AzureDevOps.Client;
using LoDaTek.AzureDevOps.Services.Client.Connections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoDaTek.AzureDevOps.Tests.Integration;

/// <summary>
/// Reads Azure DevOps connection details from environment variables and builds
/// connections / providers for the integration tests. When the required variables
/// are absent the helpers raise <see cref="Assert.Inconclusive(string)"/> so the
/// tests are skipped rather than failed on machines without credentials.
/// </summary>
/// <remarks>
/// Required: <c>AZDO_ORG</c>, <c>AZDO_PAT</c>.
/// Optional: <c>AZDO_PROJECT</c> (project name or id), <c>AZDO_FEED</c> (feed name or id).
/// </remarks>
internal static class IntegrationConfig
{
    /// <summary>Gets the organisation name (<c>AZDO_ORG</c>).</summary>
    public static string Organisation => Environment.GetEnvironmentVariable("AZDO_ORG");

    /// <summary>Gets the personal access token (<c>AZDO_PAT</c>).</summary>
    public static string PersonalAccessToken => Environment.GetEnvironmentVariable("AZDO_PAT");

    /// <summary>Gets the optional project name or id (<c>AZDO_PROJECT</c>).</summary>
    public static string Project => Environment.GetEnvironmentVariable("AZDO_PROJECT");

    /// <summary>Gets the optional feed name or id (<c>AZDO_FEED</c>).</summary>
    public static string Feed => Environment.GetEnvironmentVariable("AZDO_FEED");

    /// <summary>
    /// Gets a value indicating whether the required credentials are present.
    /// </summary>
    public static bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Organisation) && !string.IsNullOrWhiteSpace(PersonalAccessToken);

    /// <summary>
    /// Builds a cloud connection, or skips the test when credentials are missing.
    /// </summary>
    /// <returns>A configured <see cref="AzureDevOpsCloudConnection"/>.</returns>
    public static AzureDevOpsCloudConnection RequireConnection()
    {
        if (!IsConfigured)
            Assert.Inconclusive("Integration tests skipped: set AZDO_ORG and AZDO_PAT to run them.");

        return new AzureDevOpsCloudConnection(Organisation, PersonalAccessToken);
    }

    /// <summary>
    /// Builds a provider over a cloud connection, or skips the test when credentials are missing.
    /// </summary>
    /// <returns>A configured <see cref="AzureDevOpsProvider"/>.</returns>
    public static AzureDevOpsProvider RequireProvider() => new AzureDevOpsProvider(RequireConnection());

    /// <summary>
    /// Skips the current test when the named variable is absent.
    /// </summary>
    /// <param name="value">The variable value.</param>
    /// <param name="name">The variable name, for the skip message.</param>
    /// <returns>The value when present.</returns>
    public static string RequireValue(string value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            Assert.Inconclusive($"Test skipped: set {name} to run it.");

        return value;
    }
}
