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

# Using IHttpClientFactory (optional)

By default each connection manages its own `HttpClient`. In a DI app you can instead
have the REST clients resolve a pooled `HttpClient` from `IHttpClientFactory`.

Register the named client (matches the built-in config — redirects disabled, 30s timeout):

    using LoDaTek.AzureDevOps.Services.Client.Extensions;

    services.AddAzureDevOpsHttpClient();

Then assign the factory to the connection. When `HttpClientFactory` is set, clients are
resolved from the factory; when it is `null` the built-in client is used, so existing
code keeps working unchanged.

    var connection = new AzureDevOpsCloudConnection(orgName, pat)
    {
        HttpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>()
    };