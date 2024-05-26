using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LoDaTek.AzureDevOps.Services.Client;
using LoDaTek.AzureDevOps.Services.Client.Connections;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pat = "";            
            var orgName = "humbatt";
            
            using (var cloudConnection = new AzureDevOpsCloudConnection(orgName, pat))
            {
                var feedClient = cloudConnection.GetClient<FeedManagmentHttpClient>();

                var feeds = await feedClient.GetFeedsAsync();

                var feedCount = feeds.Count;

                var firstFeed = feeds.First();

                var packages = await feedClient.GetPackagesAsync(firstFeed.Id);

                var packCount = packages.Count;

                var firsPack = packages.First();

                var firstVersion = firsPack.Versions.First();

                var output = Path.Combine("C:\\", $"{firsPack.Name}.{firstVersion.Version}.nupkg");

                var packageClient = cloudConnection.GetClient<PackageManagementHttpClient>();

                var baseUrl = await packageClient.GetNugetBasePathAsync(firstFeed.Name);

                await packageClient.DownloadNugetPackageAsync(baseUrl, firsPack.Name, firstVersion.Version, output);

                Console.WriteLine($"Cloud Feeds: {feedCount}");
            }

            Console.ReadLine();
                
        }
    }
}
