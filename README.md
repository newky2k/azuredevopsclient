# LoDaTek Azure DevOps Client

# Introduction 
Client access library for the Azure DevOps Rest API, for the bits missing from the official library such as feeds and packages.

Works with:

- Azure DevOps Cloud
    - Use `AzureDevOpsCloudConnection`
- Azure DevOps Cloud (Legacy visualstudio.com URLs)
    - Use `AzureDevOpsLegacyCloudConnection`
- Azure DevOps Service (On-Premise)
    - Use `AzureDevOpsServerConnection`

# Example

    using LoDaTek.AzureDevOps.Services.Client;
    using LoDaTek.AzureDevOps.Services.Client.Connections;

    // create conection to server
    using (var cloudConnection = new AzureDevOpsCloudConnection(orgName, pat))
    {
        // get the feed client
        var feedClient = cloudConnection.GetClient<FeedManagmentHttpClient>();

        var feeds = await feedClient.GetFeedsAsync();

        var firstFeed = feeds.First();

        var packages = await feedClient.GetPackagesAsync(firstFeed.Id);

        var firsPack = packages.First();

        var firstVersion = firsPack.Versions.First();

        var output = Path.Combine("C:\\", $"{firsPack.Name}.{firstVersion.Version}.nupkg");

        // get the package management client
        var packageClient = cloudConnection.GetClient<PackageManagementHttpClient>();

        var baseUrl = await packageClient.GetNugetBasePathAsync(firstFeed.Name);

        await packageClient.DownloadNugetPackageAsync(baseUrl, firsPack.Name, firstVersion.Version, output);

        Console.WriteLine($"Cloud Feeds: {feedCount}");
    }