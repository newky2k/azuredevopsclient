
namespace LoDaTek.AzureDevOps.Client.Builders;

/// <summary>
/// Builder for WIQL (Work Item Query Language) queries.
/// </summary>
public static class WiqlBuilder
{
    /// <summary>
    /// Field types excluded from form field extraction.
    /// </summary>
    private static string[] excludedFeildTypes = new string[] { "LabelControl", "DeploymentsControl", "LinksControl", "WorkItemLogControl", "AttachmentsControl" };

    /// <summary>
    /// Required system fields always included in work item queries.
    /// </summary>
    internal static string[] requiredFields = new string[] { "System.Id", "System.Title", "System.WorkItemType", "System.AreaPath", "System.AssignedTo", "System.State", "System.ChangedDate", "System.CreatedDate", "System.AttachedFileCount" };

    /// <summary>
    /// Builds a WIQL query for the specified work item type and project.
    /// </summary>
    /// <param name="workItemType">Type of the work item.</param>
    /// <param name="projectName">Name of the project.</param>
    /// <param name="includeClosed">if set to <c>true</c> includes closed work items.</param>
    /// <param name="changedSince">If specified, only includes items changed after this date.</param>
    /// <returns>Wiql.</returns>
    public static Wiql BuildQuery(WorkItemType workItemType, string projectName, bool includeClosed, DateTime? changedSince)
    {
        return new Wiql() { Query = BuildString(workItemType, projectName, includeClosed, changedSince) };
    }

    /// <summary>
    /// Builds a WIQL query for all work item types in the specified project.
    /// </summary>
    /// <param name="projectName">Name of the project.</param>
    /// <param name="includeClosed">if set to <c>true</c> includes closed work items.</param>
    /// <param name="changedSince">If specified, only includes items changed after this date.</param>
    /// <returns>Wiql.</returns>
    public static Wiql BuildQuery(string projectName, bool includeClosed, DateTime? changedSince)
    {
        return new Wiql() { Query = BuildString(null, projectName, includeClosed, changedSince) };
    }

    /// <summary>
    /// Builds the WIQL query string for the specified parameters.
    /// </summary>
    /// <param name="workItemType">Type of the work item.</param>
    /// <param name="projectName">Name of the project.</param>
    /// <param name="includeClosed">if set to <c>true</c> includes closed work items.</param>
    /// <param name="changedSince">If specified, only includes items changed after this date.</param>
    /// <returns>System.String.</returns>
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

    /// <summary>
    /// Builds the WHERE clause for the work item type filter.
    /// </summary>
    /// <param name="workItemType">Type of the work item, or <c>null</c> to match all types.</param>
    /// <returns>System.String.</returns>
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

    /// <summary>
    /// Builds a dictionary of field reference names to labels from the work item type's XML form.
    /// </summary>
    /// <param name="workItemType">Type of the work item.</param>
    /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
    public static Dictionary<string, string> BuildFields(WorkItemType workItemType) => GetFormFields(workItemType.XmlForm);

    /// <summary>
    /// Extracts form field definitions from the work item type XML form definition.
    /// </summary>
    /// <param name="xmlForm">The XML form definition string.</param>
    /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
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
