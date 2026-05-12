using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;


namespace SampleWPF.Properties
{
    public class Settings : ApplicationSettingsBase
    {
        static Settings _defaultInstance = (Settings)Synchronized(new Settings());

        public static Settings Default { get => _defaultInstance; }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Save();
            base.OnPropertyChanged(sender, e);
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string OrganisationName
        {
            get
            {
                return ((string)(this[nameof(OrganisationName)]));
            }
            set
            {
                this[nameof(OrganisationName)] = value;
            }
        }


        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string PAT
        {
            get
            {
                return ((string)(this[nameof(PAT)]));
            }
            set
            {
                this[nameof(PAT)] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int ServerType
        {
            get
            {
                return ((int)(this[nameof(ServerType)]));
            }
            set
            {
                this[nameof(ServerType)] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ProjectName
        {
            get
            {
                return ((string)(this[nameof(ProjectName)]));
            }
            set
            {
                this[nameof(ProjectName)] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string RepoName
        {
            get
            {
                return ((string)(this[nameof(RepoName)]));
            }
            set
            {
                this[nameof(RepoName)] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string PipelineName
        {
            get
            {
                return ((string)(this[nameof(PipelineName)]));
            }
            set
            {
                this[nameof(PipelineName)] = value;
            }
        }
    }
}

