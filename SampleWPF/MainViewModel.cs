using LoDaTek.AzureDevOps.Client;
using LoDaTek.AzureDevOps.Services.Client.Connections;
using LoDaTek.AzureDevOps.Services.Client.Enums;
using Microsoft.VisualStudio.Services.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SampleWPF
{
	public class MainViewModel : ViewModel
    {
		private string _name;
        private string _pat;
        private IEnumerable<ServerType> _serverTypes;
        private ServerType _serverType;

        public string Name
		{
			get { return _name; }
			set 
			{
				if (_name != value)
				{
					_name = value;
					NotifyPropertyChanged(nameof(Name));

                    Properties.Settings.Default.OrganisationName = value;
					Properties.Settings.Default.Save();

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

                    Properties.Settings.Default.PAT = value;
                    Properties.Settings.Default.Save();
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

                    Properties.Settings.Default.ServerType = (int)value;
                    Properties.Settings.Default.Save();
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

								var project = await devOpsProvider.FindProjectAsync("");

								if (project != null)
								{
									var variableGroups = await devOpsProvider.GetVariableGroupsAsync(project);

									var pipelines = await devOpsProvider.FindBuildPipelinesAsync(project);

									var secureFiles = await devOpsProvider.GetSecureFileAsync(project);

									var secureFiles2 = await devOpsProvider.GetSecureFile2Async(project);
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
			_name = Properties.Settings.Default.OrganisationName;
			_pat = Properties.Settings.Default.PAT;

			var values = Enum.GetValues(typeof(ServerType)).Cast<ServerType>();

			var intValue = Properties.Settings.Default.ServerType;

			var selectedValue = values.FirstOrDefault(x => x.Equals(intValue));

			_serverTypes = values;

			ServerType = selectedValue;
        }
	}
}
