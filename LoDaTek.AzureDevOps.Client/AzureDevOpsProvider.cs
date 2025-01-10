using LoDaTek.AzureDevOps.Services.Client;
using LoDaTek.AzureDevOps.Services.Client.Connections;
using LoDaTek.AzureDevOps.Services.Client.Enums;
using LoDaTek.AzureDevOps.Services.Client.Models;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.Wiki.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.ExtensionManagement.WebApi;
using Microsoft.VisualStudio.Services.Gallery.WebApi;
using Microsoft.VisualStudio.Services.Organization;
using Microsoft.VisualStudio.Services.Organization.Client;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Clients;
using Microsoft.VisualStudio.Services.Security.Client;
using Microsoft.VisualStudio.Services.WebApi;
using System.Net;
using System.Net.Http.Headers;

namespace LoDaTek.AzureDevOps.Client
{
    public class AzureDevOpsProvider
    {
        #region Fields
        private DevOpsOrganisation _organisation;
        private string credentials => Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _organisation.PAT)));
        private VssConnection _connection;
        private GitHttpClient _gitClient;
        private TfvcHttpClient _tfvcClient;
        private WorkItemTrackingHttpClient _workItemClients;
        private WikiHttpClient _wikiClient;
        private BuildHttpClient _buildClient;
        private FeedManagmentHttpClient _feedClient;
        private PackageManagementHttpClient _packagesClient;

        private ReleaseHttpClient _releaseClient;
        private SecurityHttpClient _securityClient;
        private ExtensionManagementHttpClient _extensionClient;
        private GalleryHttpClient _galleryClient;

        IDevOpsConnection _restApiConnection;

        #endregion

        #region Properties

        private VssConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new VssConnection(new Uri(_organisation.Url), new VssBasicCredential(string.Empty, _organisation.PAT));

                return _connection;
            }

        }

        private IDevOpsConnection RestApiConnection
        {
            get
            {
                if (_restApiConnection == null)
                {
                    var components = _organisation.GetUrlComponents();


                    switch (_organisation.OrganisationType)
                    {
                        case ServerType.AzureDevOps:
                            {
                                _restApiConnection = new AzureDevOpsCloudConnection(components["name"], _organisation.PAT);
                            }
                            break;
                        case ServerType.DevOpsServer:
                            {
                                _restApiConnection = new AzureDevOpsServerConnection(components["url"], components["name"], _organisation.PAT);
                            }
                            break;
                    }
                }


                return _restApiConnection;
            }
        }

        /// <summary>
        /// Gets the git client.
        /// </summary>
        /// <value>The git client.</value>
        public GitHttpClient GitClient
        {
            get
            {
                if (_gitClient == null)
                    _gitClient = Connection.GetClient<GitHttpClient>();

                return _gitClient;
            }
        }

        /// <summary>
        /// Gets the TFVC client.
        /// </summary>
        /// <value>The TFVC client.</value>
        public TfvcHttpClient TfvcClient
        {
            get
            {
                if (_tfvcClient == null)
                    _tfvcClient = Connection.GetClient<TfvcHttpClient>();

                return _tfvcClient;
            }
        }

        /// <summary>
        /// Gets the work item client.
        /// </summary>
        /// <value>The work item client.</value>
        public WorkItemTrackingHttpClient WorkItemClient
        {
            get
            {
                if (_workItemClients == null)
                    _workItemClients = Connection.GetClient<WorkItemTrackingHttpClient>();

                return _workItemClients;
            }

        }

        /// <summary>
        /// Gets the wiki client.
        /// </summary>
        /// <value>The wiki client.</value>
        public WikiHttpClient WikiClient
        {
            get
            {
                if (_wikiClient == null)
                    _wikiClient = Connection.GetClient<WikiHttpClient>();

                return _wikiClient;
            }

        }

        /// <summary>
        /// Gets the pipeline client.
        /// </summary>
        /// <value>The pipeline client.</value>
        public BuildHttpClient PipelineClient
        {
            get
            {
                if (_buildClient == null)
                    _buildClient = Connection.GetClient<BuildHttpClient>();


                return _buildClient;
            }

        }

        /// <summary>
        /// Gets the feed client.
        /// </summary>
        /// <value>The feed client.</value>
        public FeedManagmentHttpClient FeedClient
        {
            get
            {
                if (_feedClient == null)
                    _feedClient = RestApiConnection.GetClient<FeedManagmentHttpClient>();

                return _feedClient;
            }
        }

        /// <summary>
        /// Gets the packages client.
        /// </summary>
        /// <value>The packages client.</value>
        public PackageManagementHttpClient PackagesClient
        {
            get
            {
                if (_packagesClient == null)
                    _packagesClient = RestApiConnection.GetClient<PackageManagementHttpClient>();

                return _packagesClient;
            }
        }

        /// <summary>
        /// Gets the security client.
        /// </summary>
        /// <value>The security client.</value>
        public SecurityHttpClient SecurityClient
        {
            get
            {
                if (_securityClient == null)
                    _securityClient = Connection.GetClient<SecurityHttpClient>();

                return _securityClient;
            }
        }

        /// <summary>
        /// Gets the release client.
        /// </summary>
        /// <value>The release client.</value>
        public ReleaseHttpClient ReleaseClient
        {
            get
            {
                if (_releaseClient == null)
                    _releaseClient = Connection.GetClient<ReleaseHttpClient>();


                return _releaseClient;
            }
        }

        /// <summary>
        /// Gets the extensions client.
        /// </summary>
        /// <value>The extensions client.</value>
        public ExtensionManagementHttpClient ExtensionsClient
        {
            get
            {
                if (_extensionClient == null)
                    _extensionClient = Connection.GetClient<ExtensionManagementHttpClient>();


                return _extensionClient;
            }
        }


        /// <summary>
        /// Gets the gallery net client.
        /// </summary>
        /// <value>The gallery net client.</value>
        protected HttpClient GalleryNetClient
        {
            get
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                return client;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsProvider"/> class.
        /// </summary>
        /// <param name="organisation">The organisation.</param>
        public AzureDevOpsProvider(DevOpsOrganisation organisation)
        {
            _organisation = organisation;
        }

        #endregion


        #region Download Functions



        public async Task<(List<Package> Packages, string BaseUrl)> FindFeedArtifactsAsync(string feedRemoteId, string feedName, string projectName)
        {

            try
            {
                var packages = await FeedClient.GetPackagesAsync(feedRemoteId);

                var baseUrl = await PackagesClient.GetNugetBasePathAsync(feedName, projectName);

                return (packages, baseUrl);

            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task DownloadPackageAsync(string packageUrl, string outPutFileName)
        {
            var tries = 0;
            var maxTries = 10;

            using (var wc = new WebClient()
            {
                Credentials = new NetworkCredential("username", _organisation.PAT),
            })
            {
                while (true)
                {
                    try
                    {
                        await wc.DownloadFileTaskAsync(new Uri(packageUrl), outPutFileName);

                        break;
                    }
                    catch (Exception ex)
                    {
                        if (tries > maxTries)
                            throw;

                        await Task.Delay(5000);

                        tries++;
                    }
                }
            }

        }


        public async Task DownloadGitRepoAsync(string repoRemoteId, string outputLocation, IEnumerable<string> branchenames = null, CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(outputLocation))
                Directory.CreateDirectory(outputLocation);

            foreach (var branch in branchenames)
            {
                var desc = new GitVersionDescriptor()
                {
                    Version = branch,
                    VersionOptions = GitVersionOptions.None,
                    VersionType = GitVersionType.Branch,
                };

                var timestamp = DateTime.Now;
                var filename = $"{branch}_{timestamp:yyMMdd}_{timestamp:hhmmss}.zip";

                var outPutPath = Path.Combine(outputLocation, filename);

                var repoStream = await GitClient.GetItemZipAsync(repoRemoteId, @"/", versionDescriptor: desc, resolveLfs: true, cancellationToken: cancellationToken);

                using (var fs = new FileStream(outPutPath, FileMode.Create))
                {
                    await repoStream.CopyToAsync(fs);
                }


            }
        }

        public async Task<Stream> DownloadGitRepoZipFile(string repoRemoteId, string branchName, string outPutPath, CancellationToken cancellationToken = default)
        {
            var desc = new GitVersionDescriptor()
            {
                Version = branchName,
                VersionOptions = GitVersionOptions.None,
                VersionType = GitVersionType.Branch,
            };

            return await GitClient.GetItemZipAsync(repoRemoteId, @"/", versionDescriptor: desc, resolveLfs: true, cancellationToken: cancellationToken);


        }

        public async Task<Stream> DownloadGitRepoTagZipFile(string repoRemoteId, string branchName, string outPutPath, CancellationToken cancellationToken = default)
        {
            var desc = new GitVersionDescriptor()
            {
                Version = branchName,
                VersionOptions = GitVersionOptions.None,
                VersionType = GitVersionType.Tag,
            };

            return await GitClient.GetItemZipAsync(repoRemoteId, @"/", versionDescriptor: desc, resolveLfs: true, cancellationToken: cancellationToken);


        }

        public async Task<Stream> GetAttachmentContentAsync(Guid guid, string fileName, bool download)
        {
            return await WorkItemClient.GetAttachmentContentAsync(guid, fileName: fileName, download: download);
        }

        #endregion
    }
}
