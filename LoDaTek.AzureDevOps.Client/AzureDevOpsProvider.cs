using LoDaTek.AzureDevOps.Client.Builders;
using LoDaTek.AzureDevOps.Client.Extensions;
using LoDaTek.AzureDevOps.Services.Client;
using LoDaTek.AzureDevOps.Services.Client.Bases;
using LoDaTek.AzureDevOps.Services.Client.Connections;
using LoDaTek.AzureDevOps.Services.Client.Enums;
using LoDaTek.AzureDevOps.Services.Client.Exceptions;
using LoDaTek.AzureDevOps.Services.Client.Models;
using Microsoft.Azure.Pipelines.WebApi;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.Wiki.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.ExtensionManagement.WebApi;
using Microsoft.VisualStudio.Services.Gallery.WebApi;
using Microsoft.VisualStudio.Services.Organization;
using Microsoft.VisualStudio.Services.Organization.Client;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Clients;
using Microsoft.VisualStudio.Services.Security;
using Microsoft.VisualStudio.Services.Security.Client;
using Microsoft.VisualStudio.Services.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;

namespace LoDaTek.AzureDevOps.Client
{
    /// <summary>
    /// Azure DevOps Provider
    /// </summary>
    public class AzureDevOpsProvider
    {
        #region Fields
        private DevOpsConnectionBase _devopsConnection;
        private string credentials => Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _devopsConnection.PersonalAccessToken)));
        private VssConnection _connection;
        private GitHttpClient _gitClient;
        private TfvcHttpClient _tfvcClient;
        private WorkItemTrackingHttpClient _workItemClients;
        private WikiHttpClient _wikiClient;
        private BuildHttpClient _buildClient;
        private FeedManagmentHttpClient _feedClient;
        private PackageManagementHttpClient _packagesClient;
        private SecureFilesHttpClient _secureFilesClient;

        private ReleaseHttpClient _releaseClient;
        private SecurityHttpClient _securityClient;
        private ExtensionManagementHttpClient _extensionClient;
        private GalleryHttpClient _galleryClient;
        private TaskAgentHttpClient _taskAgentHttpClient;
        private PipelinesHttpClient _pipelinesHttpClient;

        IDevOpsConnection _restApiConnection;

        #endregion

        #region Properties

        private VssConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new VssConnection(new Uri(_devopsConnection.CommonUrl), new VssBasicCredential(string.Empty, _devopsConnection.PersonalAccessToken));

                return _connection;
            }

        }

        /// <summary>
        /// Gets the rest API connection.
        /// </summary>
        /// <value>
        /// The rest API connection.
        /// </value>
        private IDevOpsConnection RestApiConnection => _devopsConnection;

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
        /// Gets the build pipeline client.
        /// </summary>
        /// <value>The pipeline client.</value>
        public BuildHttpClient BuildClient
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

        /// <summary>
        /// Gets the Task Agent http client.
        /// </summary>
        /// <value>The extensions client.</value>
        public TaskAgentHttpClient TaskAgentClient
        {
            get
            {
                if (_taskAgentHttpClient == null)
                    _taskAgentHttpClient = Connection.GetClient<TaskAgentHttpClient>();

                return _taskAgentHttpClient;
            }
        }

        /// <summary>
        /// Gets the packages client.
        /// </summary>
        /// <value>The packages client.</value>
        public SecureFilesHttpClient SecureFilesClient
        {
            get
            {
                if (_secureFilesClient == null)
                    _secureFilesClient = RestApiConnection.GetClient<SecureFilesHttpClient>();

                return _secureFilesClient;
            }
        }

        /// <summary>
        /// Gets the pipelines client.
        /// </summary>
        /// <value>The pipelines client.</value>
        public PipelinesHttpClient PipelinesClient
        {
            get
            {
                if (_pipelinesHttpClient == null)
                    _pipelinesHttpClient = Connection.GetClient<PipelinesHttpClient>();

                return _pipelinesHttpClient;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsProvider" /> class.
        /// </summary>
        /// <param name="connection">The DevOps server connection.</param>
        public AzureDevOpsProvider(DevOpsConnectionBase connection)
        {
            _devopsConnection = connection;
        }

        #endregion

        #region Projects

        /// <summary>
        /// Get the projects
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LoDaTek.AzureDevOps.Services.Client.Exceptions.PATPermissionDeniedException">Projects - Read</exception>
        public async Task<List<TeamProjectReference>> GetProjectsAsync()
        {
            try
            {
                using (var projClient = await Connection.GetClientAsync<ProjectHttpClient>())
                {
                    var projects = await projClient.GetProjects();

                    return projects.ToList();
                }
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Projects", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Projects", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get a specific project
        /// </summary>
        /// <param name="projectNameOrId">The project name or identifier.</param>
        /// <returns>A Task&lt;TeamProjectReference&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Projects - Read</exception>
        public async Task<TeamProjectReference> GetProjectAsync(string projectNameOrId)
        {
            try
            {            

                using (var projClient = await Connection.GetClientAsync<ProjectHttpClient>())
                {
                    var project = await projClient.GetProject(projectNameOrId);

                    return project;
                }
            }
            catch (ProjectDoesNotExistWithNameException e)
            {
                // if project not found then just return null
                return null;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Projects", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Projects", "Read", e);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Git Repos

        /// <summary>
        /// Get the specified git repostitory for the specified project
        /// </summary>
        /// <param name="project">The project remote identifier.</param>
        /// <param name="name">Name of the repo.</param>
        /// <returns></returns>
        /// <exception cref="LoDaTek.AzureDevOps.Services.Client.Exceptions.PATPermissionDeniedException">Git Repositories - Read</exception>
        public async Task<GitRepository> GetRepositoryAsync(TeamProjectReference project, string name)
        {
            try
            {
                var repos = await GitClient.GetRepositoriesAsync(project.Id.ToString());

                var first = repos.FirstOrDefault(x => x.Name == name);

                return first;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repositories", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repositories", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get the git repostitories for the specified project
        /// </summary>
        /// <param name="project">The project remote identifier.</param>
        /// <returns></returns>
        /// <exception cref="LoDaTek.AzureDevOps.Services.Client.Exceptions.PATPermissionDeniedException">Git Repositories - Read</exception>
        public async Task<List<GitRepository>> GetRepositoriesAsync(TeamProjectReference project)
        {
            try
            {
                var repos = await GitClient.GetRepositoriesAsync(project.Id.ToString());

                return repos;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repositories", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repositories", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get the git repostitories for the specified projects
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <returns></returns>
        /// <exception cref="LoDaTek.AzureDevOps.Services.Client.Exceptions.PATPermissionDeniedException">Git Repositories - Read</exception>
        public async Task<Dictionary<Guid, List<GitRepository>>> GetRepositoriesAsync(IEnumerable<TeamProjectReference> projects)
        {
            try
            {

                Dictionary<Guid, List <GitRepository >> result = [];

                foreach (var project in projects)
                {
                    var repos = await GetRepositoriesAsync(project);

                    result[project.Id] = repos;
                }

                return result;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repositories", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repositories", "Read", e);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Finds the git tags asynchronous.
        /// </summary>
        /// <param name="repository">repository.</param>
        /// <returns></returns>
        /// <exception cref="LoDaTek.AzureDevOps.Services.Client.Exceptions.PATPermissionDeniedException">Git Repository Branches - Read</exception>
        public async Task<List<GitRef>> GetGitTagsAsync(GitRepository repository)
        {
            try
            {
                var tags = await GitClient.GetRefsAsync(repository.Id, filter: "tags", null, null, null, null, true, null, null);

                return tags;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repository Branches", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repository Branches", "Read", e);
            }
            catch (VssServiceResponseException ex)
            {
                if (ex.Message.Contains("Cannot find any branches"))
                    return new List<GitRef>();

                throw;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Finds the git branches asynchronous.
        /// </summary>
        /// <param name="repository">The repo remote identifier.</param>
        /// <returns></returns>
        /// <exception cref="LoDaTek.AzureDevOps.Services.Client.Exceptions.PATPermissionDeniedException">Git Repository Branches - Read</exception>
        public async Task<List<GitBranchStats>> GetBranchesAsync(GitRepository repository)
        {
            try
            {
                var branches = await GitClient.GetBranchesAsync(repository.Id);

                return branches;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repository Branches", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Git Repository Branches", "Read", e);
            }
            catch (VssServiceResponseException ex)
            {
                if (ex.Message.Contains("Cannot find any branches"))
                    return new List<GitBranchStats>();

                throw;

            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion

        #region Work Item 

        /// <summary>
        /// Find work item types as an asynchronous operation.
        /// </summary>
        /// <param name="projectRemoteId">The project remote identifier.</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Work Item Types - Read</exception>
        public async Task<List<WorkItemType>> FindWorkItemTypesAsync(Guid projectRemoteId)
        {
            try
            {
                var workItemTypes = await WorkItemClient.GetWorkItemTypesAsync(projectRemoteId);

                return workItemTypes;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Work Item Types", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Work Item Types", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Find work item fields as an asynchronous operation.
        /// </summary>
        /// <param name="projectRemoteId">The project remote identifier.</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Work Item Fields - Read</exception>
        public async Task<List<WorkItemField>> FindWorkItemFieldsAsync(Guid projectRemoteId)
        {
            try
            {
                var fields = await WorkItemClient.GetFieldsAsync(projectRemoteId);

                return fields;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Work Item Fields", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Work Item Fields", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }


        }

        //public async Task<List<AgileWorkItem>> FindWorkItemsAsync(IWorkItemType workItemType, IProject project, IOrganisation organisation)
        //{

        //    try
        //    {
        //        var workItemFields = await FindWorkItemFieldsAsync(project);

        //        var attachments = new Dictionary<int, List<WorkItemAttachment>>();

        //        var wiql = WiqlBuilder.BuildQuery(workItemType, project, organisation);

        //        var formFields = WiqlBuilder.BuildFields(workItemType);

        //        var result = await WorkItemClient.QueryByWiqlAsync(wiql, project.RemoteId, timePrecision: true);
        //        var ids = result.WorkItems.Select(item => item.Id).ToArray();

        //        if (!ids.Any())
        //            return new List<AgileWorkItem>();

        //        var itemsExtendedItm = await WorkItemClient.GetWorkItemsAsync(ids, asOf: result.AsOf, expand: WorkItemExpand.All);

        //        var agileItems = itemsExtendedItm.ToLocal(formFields, workItemFields);

        //        return agileItems;
        //    }
        //    catch (VssUnauthorizedException e)
        //    {
        //        throw new PATPermissionDeniedException(_organisation, "Work Items", "Read", e);
        //    }
        //    catch (Exception e) when (e.InnerException is VssUnauthorizedException)
        //    {
        //        throw new PATPermissionDeniedException(_organisation, "Work Items", "Read", e);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }



        //}

        public async Task<List<AgileWorkItem>> FindWorkItemsAsync(IEnumerable<WorkItemType> workItemTypes, Guid projectRemoteId, string projectName, bool includeClosed, DateTime? changedSince)
        {
            try
            {
                var workItemFields = await FindWorkItemFieldsAsync(projectRemoteId);

                //var attachments = new Dictionary<int, List<WorkItemAttachment>>();

                var wiql = WiqlBuilder.BuildQuery(projectName, includeClosed, changedSince);

                var result = await WorkItemClient.QueryByWiqlAsync(wiql, projectRemoteId, timePrecision: true);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                if (!ids.Any())
                    return new List<AgileWorkItem>();

                var itemsExtendedItm = await WorkItemClient.GetWorkItemsAsync(ids, asOf: result.AsOf, expand: WorkItemExpand.All);


                var results = new List<AgileWorkItem>();

                foreach (var itemType in workItemTypes)
                {
                    var typeItems = itemsExtendedItm.Where(x => x.Fields["System.WorkItemType"].ToString().Equals(itemType.Name));

                    if (typeItems.Any())
                    {

                        var formFields = WiqlBuilder.BuildFields(itemType);

                        var agileItems = await typeItems.ToLocalAsync(formFields, workItemFields);

                        results.AddRange(agileItems);

                    }
                }

                foreach (var agileItem in results)
                {
                    var comments = await WorkItemClient.GetCommentsAsync(projectRemoteId, int.Parse(agileItem.Id));

                    agileItem.Comments = await comments.Comments.ToLocalAsync(agileItem);
                }


                return results;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Work Items", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Work Items", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }



        }

        /// <summary>
        /// Get queries as an asynchronous operation.
        /// </summary>
        /// <param name="projectRemoteId">The project remote identifier.</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Work Items - Read</exception>
        public async Task<List<QueryHierarchyItem>> GetQueriesAsync(Guid projectRemoteId)
        {
            try
            {
                var queries = await WorkItemClient.GetQueriesAsync(projectRemoteId, QueryExpand.All, depth: 2);


                return queries;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Work Items", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Work Items", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Wikis

        /// <summary>
        /// Find wikis as an asynchronous operation.
        /// </summary>
        /// <param name="projectRemoteId">The project remote identifier.</param>
        /// <param name="projectOnly">if set to <c>true</c> [project only].</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Wikis - Read</exception>
        public async Task<List<WikiV2>> FindWikisAsync(Guid projectRemoteId, bool projectOnly = true)
        {

            try
            {
                var results = await WikiClient.GetAllWikisAsync(projectRemoteId);

                return results;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Wikis", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Wikis", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }


        }

        /// <summary>
        /// Get wiki page zip as an asynchronous operation.
        /// </summary>
        /// <param name="projectRemoteId">The project remote identifier.</param>
        /// <param name="remoteId">The remote identifier.</param>
        /// <param name="path">The path.</param>
        /// <returns>A Task&lt;Stream&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Wikis - Read</exception>
        public async Task<Stream> GetWikiPageZipAsync(Guid projectRemoteId, Guid remoteId, string path = null)
        {
            try
            {
                var wikiZip = await WikiClient.GetPageZipAsync(projectRemoteId, remoteId, path: path, recursionLevel: VersionControlRecursionType.Full, includeContent: true);

                return wikiZip;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Wikis", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Wikis", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Pipelines

        /// <summary>
        /// Get the build pipelines with the specified name for thje specified project
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="pipelineName">Name of the pipeline.</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Pipelines - Read</exception>
        public async Task<BuildDefinition> GetBuildPipelineAsync(TeamProjectReference project, string pipelineName)
        {
            try
            {
                var results = await GetBuildPipelinesAsync(project);

                var first = results.FirstOrDefault(x => x.Name == pipelineName);

                return first;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Get the build pipelines for the specified project
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Pipelines - Read</exception>
        public async Task<List<BuildDefinition>> GetBuildPipelinesAsync(TeamProjectReference project)
        {
            try
            {
                var results = await BuildClient.GetFullDefinitionsAsync(project.Id);

                return results;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Get the release pipelines for the specified project
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Pipelines - Read</exception>
        public async Task<List<ReleaseDefinition>> GetReleasePipelinesAsync(TeamProjectReference project)
        {
            try
            {
                var results = await ReleaseClient.GetReleaseDefinitionsAsync(project.Id);

                return results;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Get pipeline json as an asynchronous operation.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="pipelineRemoteId">The pipeline remote identifier.</param>
        /// <param name="isReleasePipeline">if set to <c>true</c> [is release pipeline].</param>
        /// <returns>A Task&lt;System.String&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Pipelines - Read</exception>
        public async Task<string> GetPipelineJSONAsync(TeamProjectReference project, int pipelineRemoteId, bool isReleasePipeline)
        {
            try
            {
                var jsonSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = new List<JsonConverter> { new Newtonsoft.Json.Converters.StringEnumConverter(new CamelCaseNamingStrategy()) },
                };

                if (isReleasePipeline == true)
                {
                    var data = await ReleaseClient.GetReleaseDefinitionAsync(project.Id, pipelineRemoteId);

                    if (data == null)
                        return null;

                    var output = JsonConvert.SerializeObject(data, Formatting.Indented, jsonSettings);

                    return output;

                }
                else
                {
                    var data = await BuildClient.GetDefinitionAsync(project.Id, pipelineRemoteId);

                    if (data == null)
                        return null;

                    //var output = await Client.GetStringAsync(new Uri(data.Url));
                    var output = JsonConvert.SerializeObject(data, Formatting.Indented, jsonSettings);

                    return output;
                }
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Runs a pipeline asynchronous.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="branch">The branch.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        public async Task RunPipelineAsync(TeamProjectReference project, BuildDefinition pipeline, GitBranchStats branch, CancellationToken cancellationToken = default)
        {
            try
            {
                var refName = $"refs/heads/{branch.Name}";
                RunPipelineParameters pars = new()
                {
                    Resources = new RunResourcesParameters(),
                };

                pars.Resources.Repositories.Add("self", new RepositoryResourceParameters()
                {
                    RefName = refName,
                });

                var result = await PipelinesClient.RunPipelineAsync(pars, project.Id, pipeline.Id, cancellationToken: cancellationToken);

                if (result.State == RunState.InProgress)
                {

                }
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Pipelines", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Security


        /// <summary>
        /// List pat permission as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Permissions - Read</exception>
        public async Task<List<PATPermission>> ListPATPermissionAsync()
        {

            try
            {

                var results = new List<PATPermission>();

                var namespaces = await SecurityClient.QuerySecurityNamespacesAsync(Guid.Empty, false);


                foreach (SecurityNamespaceDescription ns in namespaces)
                {
                    var acsl = await SecurityClient.QueryAccessControlListsAsync(ns.NamespaceId, string.Empty, null, false, false);
                    var aclstokens = acsl.Select(x => x.Token);

                    if (!acsl.Any())
                        continue;


                    var newPerm = new PATPermission()
                    {
                        Id = ns.NamespaceId,
                        DisplayName = ns.DisplayName ?? ns.Name,
                    };



                    foreach (ActionDefinition actionDef in ns.Actions)
                    {

                        var pers = await SecurityClient.HasPermissionsAsync(ns.NamespaceId, aclstokens, actionDef.Bit, false);

                        if (pers.Any(x => x.Equals(true)))
                        {
                            var action = new PATPermissionAction()
                            {

                                DisplayName = actionDef.DisplayName ?? actionDef.Name,
                                CanRead = actionDef.Bit == ns.ReadPermission,
                                CanWrite = actionDef.Bit == ns.WritePermission,
                            };

                            if (action.CanRead)
                                newPerm.HasReadablePermissions = true;

                            newPerm.Actions.Add(action);

                            //if permission granted then skip
                            //break;
                        }

                    }


                    results.Add(newPerm);

                }

                return results;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Permissions", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Permissions", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion

        #region Feeds

        /// <summary>
        /// Fetch available feeds as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
        /// <exception cref="PATPermissionDeniedException">Arifiact Feeds - Read</exception>
        public async Task<List<Feed>> FetchAvailableFeedsAsync()
        {
            try
            {
                var feeds = await FeedClient.GetFeedsAsync();

                return feeds;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Arifiact Feeds", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Arifiact Feeds", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// Find feed artifacts as an asynchronous operation.
        /// </summary>
        /// <param name="feedRemoteId">The feed remote identifier.</param>
        /// <param name="feedName">Name of the feed.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
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

        #endregion

        #region Download Functions

        /// <summary>
        /// Download package as an asynchronous operation.
        /// </summary>
        /// <param name="packageUrl">The package URL.</param>
        /// <param name="outPutFileName">Name of the out put file.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task DownloadPackageAsync(string packageUrl, string outPutFileName)
        {
            var tries = 0;
            var maxTries = 10;


            var credentials = new NetworkCredential("username", _devopsConnection.PersonalAccessToken);
            var handler = new HttpClientHandler { Credentials = credentials, PreAuthenticate = true };

            using (var client = new HttpClient(handler))
            {
                while (true)
                {
                    try
                    {

                        using (var s = await client.GetStreamAsync(new Uri(packageUrl)))
                        {
                            using (var fs = new FileStream(outPutFileName, FileMode.CreateNew))
                            {
                                await s.CopyToAsync(fs);
                            }
                        }

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


        /// <summary>
        /// Download git repo as an asynchronous operation.
        /// </summary>
        /// <param name="repoRemoteId">The repo remote identifier.</param>
        /// <param name="outputLocation">The output location.</param>
        /// <param name="branchenames">The branchenames.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Downloads the git repo zip file.
        /// </summary>
        /// <param name="repoRemoteId">The repo remote identifier.</param>
        /// <param name="branchName">Name of the branch.</param>
        /// <param name="outPutPath">The out put path.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Stream.</returns>
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

        /// <summary>
        /// Downloads the git repo tag zip file.
        /// </summary>
        /// <param name="repoRemoteId">The repo remote identifier.</param>
        /// <param name="branchName">Name of the branch.</param>
        /// <param name="outPutPath">The out put path.</param>
        /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Stream.</returns>
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

        /// <summary>
        /// Get attachment content as an asynchronous operation.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="download">if set to <c>true</c> [download].</param>
        /// <returns>A Task&lt;Stream&gt; representing the asynchronous operation.</returns>
        public async Task<Stream> GetAttachmentContentAsync(Guid guid, string fileName, bool download)
        {
            return await WorkItemClient.GetAttachmentContentAsync(guid, fileName: fileName, download: download);
        }

        #endregion

        #region Variable groups

        /// <summary>
        /// Gets the variable groups for the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>System.Threading.Tasks.Task&lt;System.Collections.Generic.List&lt;Microsoft.TeamFoundation.DistributedTask.WebApi.VariableGroup&gt;&gt;.</returns>
        public async Task<List<Microsoft.TeamFoundation.DistributedTask.WebApi.VariableGroup>> GetVariableGroupsAsync(TeamProjectReference project)
        {
            try
            {
                var results = await TaskAgentClient.GetVariableGroupsAsync(project.Id);

                return results;
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Variable Groups", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "Variable Groups", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Secure Files groups

        /// <summary>
        /// Gets the secure files for the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="useMicrosoftsImplementation">The use microsofts implementation.</param>
        /// <returns>System.Threading.Tasks.Task&lt;System.Collections.Generic.List&lt;Microsoft.TeamFoundation.DistributedTask.WebApi.SecureFile&gt;&gt;.</returns>
        public async Task<List<Microsoft.TeamFoundation.DistributedTask.WebApi.SecureFile>> GetSecureFileAsync(TeamProjectReference project, bool useMicrosoftsImplementation = false)
        {
            try
            {
                if (useMicrosoftsImplementation)
                {
                    var results = await TaskAgentClient.GetSecureFilesAsync(project.Id);

                    return results;
                }
                else
                {
                    var results = await SecureFilesClient.GetAllAsync(project);

                    return results;
                }
            }
            catch (VssUnauthorizedException e)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "SecureFile", "Read", e);
            }
            catch (Exception e) when (e.InnerException is VssUnauthorizedException)
            {
                throw new PATPermissionDeniedException(_devopsConnection.OrganisationName, "SecureFile", "Read", e);
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion
    }
}
