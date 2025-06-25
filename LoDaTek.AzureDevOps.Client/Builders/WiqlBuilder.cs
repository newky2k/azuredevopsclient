using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Organization.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace LoDaTek.AzureDevOps.Client.Builders
{
    public static class WiqlBuilder
    {
        private static string[] excludedFeildTypes = new string[] { "LabelControl", "DeploymentsControl", "LinksControl", "WorkItemLogControl", "AttachmentsControl" };
        internal static string[] requiredFields = new string[] { "System.Id", "System.Title", "System.WorkItemType", "System.AreaPath", "System.AssignedTo", "System.State", "System.ChangedDate", "System.CreatedDate", "System.AttachedFileCount" };

        public static Wiql BuildQuery(WorkItemType workItemType, string projectName, bool includeClosed, DateTime? changedSince)
        {
            return new Wiql() { Query = BuildString(workItemType, projectName, includeClosed, changedSince) };
        }

        public static Wiql BuildQuery(string projectName, bool includeClosed, DateTime? changedSince)
        {
            return new Wiql() { Query = BuildString(null, projectName, includeClosed, changedSince) };
        }

        public static string BuildString(WorkItemType workItemType, string projectName, bool includeClosed, DateTime? changedSince)
        {

            var wiql = $"Select [System.Id],[Attached File Count]  From WorkItems ";

            wiql += BuildWhereType(workItemType);

            wiql += $"And [System.TeamProject] = '{projectName}' ";

            if (!includeClosed)
                wiql += "AND NOT [System.State] IN ('Completed', 'Closed') ";

            if (changedSince.HasValue)
                wiql += $"AND [System.ChangedDate] > '{changedSince.Value:s}' ";

            wiql += "Order By [Id] Asc";


            return wiql;

        }

        private static string BuildWhereType(WorkItemType workItemType)
        {
            if (workItemType == null)
            {
                return $"Where [System.WorkItemType] <> '' ";
            }
            else
            {
                return $"Where [System.WorkItemType] IN ('{workItemType.Name}') ";
            }

        }

        public static Dictionary<string, string> BuildFields(WorkItemType workItemType) => GetFormFields(workItemType.XmlForm);


        private static Dictionary<string, string> GetFormFields(string xmlForm)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlForm);

            var controls = xmlDoc.GetElementsByTagName("Control");

            var fields = new Dictionary<string, string>();

            foreach (XmlElement control in controls)
            {
                string fieldName = control.GetAttribute("FieldName");
                string fieldType = control.GetAttribute("Type");
                string label = control.GetAttribute("Label");

                if (string.IsNullOrWhiteSpace(fieldName))
                    continue;

                //exclude any fields that have already been added
                if (!fields.ContainsKey(fieldName) && !string.IsNullOrWhiteSpace(fieldType) && !excludedFeildTypes.Contains(fieldType, StringComparer.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(label))
                    {
                        var listPoint = fieldName.LastIndexOf(".");
                        var label2 = fieldName.Substring(fieldName.LastIndexOf(".") + 1);

                        label = label2;

                    }

                    if (!label.Contains(" "))
                    {
                        string[] split = Regex.Split(label, @"(?<!^)(?=[A-Z])");

                        if (split.Length > 1)
                            label = string.Join(" ", split);
                    }

                    label = label.Replace("&", "").Trim();

                    fields.Add(fieldName, label);
                }

            }

            //fields = fields.Encapsulate('[', ']');

            //var fieldList = string.Join(',', fields);

            return fields;
        }
    }
}
