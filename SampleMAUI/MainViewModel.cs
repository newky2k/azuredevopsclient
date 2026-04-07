using LoDaTek.AzureDevOps.Client;
using LoDaTek.AzureDevOps.Services.Client.Connections;
using LoDaTek.AzureDevOps.Services.Client.Enums;
using Microsoft.TeamFoundation.Core.WebApi;
using System.Mvvm;
using System.Windows.Input;

namespace SampleMAUI;

public class MainViewModel : ViewModel
{
    private string _name;
    private string _pat;
    private IEnumerable<ServerType> _serverTypes;
    private ServerType _serverType;
    private string _projectName;
    private string _repoName;
    private string _pipelineName;

    private const string kOrganisationName = nameof(kOrganisationName);
    private const string kPAT = nameof(kPAT);
    private const string kPipelineName = nameof(kPipelineName);
    private const string kProjectName = nameof(kProjectName);
    private const string kRepoName = nameof(kRepoName);
    private const string kServerType = nameof(kServerType);

    public string Name
    {
        get { return _name; }
        set
        {
            if (_name != value)
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));

                Preferences.Set(kOrganisationName, value);

            }
        }
    }

    public string PAT
    {
        get { return _pat; }
        set
        {
            if (_pat != value)
            {
                _pat = value;

                NotifyPropertyChanged(nameof(PAT));

                Preferences.Set(kPAT, value);
            }

        }
    }

    public string PipelineName
    {
        get { return _pipelineName; }
        set
        {
            if (_pipelineName != value)
            {
                _pipelineName = value;

                NotifyPropertyChanged(nameof(PipelineName));

                Preferences.Set(kPipelineName, value);
            }

        }
    }

    public string ProjectName
    {
        get { return _projectName; }
        set
        {
            if (_projectName != value)
            {
                _projectName = value;

                NotifyPropertyChanged(nameof(ProjectName));

                Preferences.Set(kProjectName, value);
            }

        }
    }

    public string RepoName
    {
        get { return _repoName; }
        set
        {
            if (_repoName != value)
            {
                _repoName = value;

                NotifyPropertyChanged(nameof(RepoName));

                Preferences.Set(kRepoName, value);
            }

        }
    }

    public IEnumerable<ServerType> ServerTypes
    {
        get { return _serverTypes; }
        set { _serverTypes = value; NotifyPropertyChanged(nameof(ServerTypes)); }
    }

    public ServerType ServerType
    {
        get { return _serverType; }
        set
        {
            if (_serverType != value)
            {
                _serverType = value;
                NotifyPropertyChanged(nameof(ServerType));

                Preferences.Set(kServerType, (int)value);
            }

        }
    }

    public ICommand ConnectCommand
    {
        get
        {
            return new DelegateCommand(async () =>
            {
                try
                {

                    if (ServerType == ServerType.AzureDevOps)
                    {
                        IsBusy = true;

                        using (var cloudConnection = new AzureDevOpsCloudConnection(Name, PAT))
                        {
                            var devOpsProvider = new AzureDevOpsProvider(cloudConnection);

                            var project = await devOpsProvider.GetProjectAsync(ProjectName);

                            if (project != null && !project.IsEmpty())
                            {
                                var variableGroups = await devOpsProvider.GetVariableGroupsAsync(project);

                                var pipeline = await devOpsProvider.GetBuildPipelineAsync(project, PipelineName);

                                if (pipeline != null)
                                {
                                    var repo = await devOpsProvider.GetRepositoryAsync(project, RepoName);

                                    if (repo != null)
                                    {
                                        var branches = await devOpsProvider.GetBranchesAsync(repo);

                                        if (branches.Any())
                                        {
                                            var branch = branches.First();

                                            await devOpsProvider.RunPipelineAsync(project, pipeline, branch);
                                        }
                                    }
                                }

                            }
                        }

                        IsBusy = false;

                    }

                }
                catch (Exception ex)
                {
                    IsBusy = false;

                    NotifyErrorOccurred(ex);
                }
            }, (obj) =>
            {
                if (string.IsNullOrWhiteSpace(PAT) || string.IsNullOrWhiteSpace(Name))
                    return false;

                return true;
            });
        }

    }

    public MainViewModel()
    {
        _name = Preferences.Get(kOrganisationName, ""); 
        _pat = Preferences.Get(kPAT, ""); 
        _projectName = Preferences.Get(kProjectName, "");
        _repoName = Preferences.Get(kRepoName, "");
        _pipelineName = Preferences.Get(kPipelineName, "");

        var values = Enum.GetValues(typeof(ServerType)).Cast<ServerType>();

        int intValue = Preferences.Get(kServerType, 0);

        var selectedValue = values.FirstOrDefault(x => x.Equals(intValue));

        _serverTypes = values;

        ServerType = selectedValue;
    }
}
