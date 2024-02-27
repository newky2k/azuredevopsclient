using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSoft.AzureDevOps.Services.Client;

namespace TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pat = "yotqwxqbu55j2my5f7w55alnhinnn5kkdu3u37emwti6bcvmyywa";
            var locPat = " ";
            
            var orgName = "humbatt";
            
            var locOrgName = "DefaultCollection";

            var instanceURl = "https://desktop-csvvhb7";


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
