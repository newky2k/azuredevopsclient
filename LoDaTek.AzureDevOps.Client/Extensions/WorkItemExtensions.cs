
namespace LoDaTek.AzureDevOps.Client.Extensions;

/// <summary>
/// WorkItem Extensions
/// </summary>
public static class WorkItemExtensions
{
    internal static string[] requiredFields = new string[] { "System.Id", "System.Title", "System.WorkItemType", "System.AreaPath", "System.State", "System.Reason", "System.AssignedTo"
                                                             , "System.ChangedDate", "System.CreatedDate", "System.ChangedDate", "System.ChangedBy", "System.CreatedBy", "System.Description", "Microsoft.VSTS.TCM.ReproSteps" };

    internal static string[] excludeFields = new string[] { "System.History" };

    /// <summary>
    /// Converts to local Agile Work Item
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="workItemFields">The work item fields.</param>
    /// <returns></returns>
    public static Task<AgileWorkItem> ToLocalAsync(this WorkItem target, Dictionary<string, string> fields, IEnumerable<WorkItemField> workItemFields)
    {
        var projectUrl = target.Url.Substring(0, target.Url.IndexOf("/_apis/wit"));

        var newItem = new AgileWorkItem()
        {
            Id = target.Id.Value.ToString(),
        };

        newItem.Title = target.Fields["System.Title"].ToString();
        newItem.Type = target.Fields["System.WorkItemType"].ToString();
        newItem.Category = target.Fields["System.AreaPath"].ToString();
        newItem.State = target.Fields["System.State"].ToString();
        newItem.Reason = target.Fields["System.Reason"].ToString();
        newItem.CreatedDate = DateTime.Parse(target.Fields["System.CreatedDate"].ToString());
        newItem.LastUpdated = DateTime.Parse(target.Fields["System.ChangedDate"].ToString());

        if (target.Fields.ContainsKey("System.AssignedTo"))
        {
            var assignedTo = target.Fields["System.AssignedTo"] as IdentityRef;

            if (assignedTo != null)
                newItem.AssignedTo = assignedTo.DisplayName;
        }

        if (target.Fields.ContainsKey("System.ChangedBy"))
        {
            var changedBy = target.Fields["System.ChangedBy"] as IdentityRef;

            if (changedBy != null)
                newItem.UpdatedBy = changedBy.DisplayName;
        }

        if (target.Fields.ContainsKey("System.CreatedBy"))
        {
            var createdBy = target.Fields["System.CreatedBy"] as IdentityRef;

            if (createdBy != null)
                newItem.CreatedBy = createdBy.DisplayName;
        }

        if (target.Fields.ContainsKey("System.Description"))
            newItem.Description = target.Fields["System.Description"].ToString();

        if (string.IsNullOrWhiteSpace(newItem.Description) && (target.Fields.ContainsKey("Microsoft.VSTS.TCM.ReproSteps")))
            newItem.Description = target.Fields["Microsoft.VSTS.TCM.ReproSteps"].ToString();

        foreach (var fieldName in target.Fields.Keys)
        {
            if (!requiredFields.Contains(fieldName) && !excludeFields.Contains(fieldName))
            {
                var field = workItemFields.FirstOrDefault(x => x.ReferenceName.Equals(fieldName, StringComparison.OrdinalIgnoreCase));

                if (field != null)
                {
                    var newField = new AgileItemField()
                    {
                        Name = fieldName,
                        Label = field.Name,
                        Value = target.Fields[fieldName].ToString(),
                        Type = (AgileItemFieldType)field.Type,
                    };

                    if (newField.Value.Equals("Microsoft.VisualStudio.Services.WebApi.IdentityRef", StringComparison.OrdinalIgnoreCase))
                    {
                        var idRefField = target.Fields[fieldName] as IdentityRef;

                        if (idRefField != null)
                            newField.Value = idRefField.DisplayName;
                        else
                            newField.Value = "Unknown";
                    }

                    newItem.Fields.Add(newField);
                }
            }
        }

        try
        {
            if (target.Relations != null)
            {
                var attachmentedFiles = target.Relations.Where(x => x.Rel.Equals("AttachedFile", StringComparison.OrdinalIgnoreCase));

                if (attachmentedFiles.Any())
                {
                    foreach (var attachment in attachmentedFiles)
                    {

                        var linkId = attachment.Attributes["id"].ToString();
                        var fileName = attachment.Attributes["name"].ToString();

                        var remoteId = attachment.Url.Substring(attachment.Url.LastIndexOf("/") + 1);

                        newItem.Attachments.Add(new AgileItemAttachment()
                        {
                            ParentItemId = newItem.Id,
                            RemoteId = remoteId,
                            Type = AgileItemAttachmentType.External,
                            FileName = $"{remoteId}_{fileName}",
                            Url = attachment.Url,
                        });
                    }
                }
            }
        }
        catch (Exception)
        {

            throw;
        }

        if (!string.IsNullOrWhiteSpace(newItem.Description) && newItem.Description.Contains("_apis/wit/attachments"))
        {
            var images = FindImages(newItem.Description);

            if (images.Any())
            {
                foreach (var imgUrl in images)
                {
                    var regEx = new Regex(@"_apis/wit/attachments/(?<fileId>.*)\?fileName=(?<fileName>.*)");

                    var matches = regEx.Matches(imgUrl);

                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            var fileId = match.Groups[1].Value;

                            var fileName = match.Groups[2].Value;

                            var fileUrl = $"{projectUrl}/_apis/wit/attachments/{fileId}?fileName={fileName}";

                            newItem.Attachments.Add(new AgileItemAttachment()
                            {
                                ParentItemId = newItem.Id,
                                RemoteId = fileId,
                                Type = AgileItemAttachmentType.Embedded,
                                FileName = $"{fileId}_{fileName}",
                                Url = fileUrl,
                            });
                        }
                    }
                }
            }



            newItem.Description = HttpUtility.HtmlEncode(newItem.Description);
        }



        return Task.FromResult(newItem);
    }

    /// <summary>
    /// Converts to local.
    /// </summary>
    /// <param name="target">The target.</param>
    /// <param name="agileItem">The agile item.</param>
    /// <returns></returns>
    public static Task<AgileItemComment> ToLocal(this Comment target, AgileWorkItem agileItem)
    {
        var comment = new AgileItemComment()
        {
            Id = target.Id,
            Url = target.Url,
            CreateDate = target.CreatedDate,
            Text = target.Text,
            ModifiedDate = target.ModifiedDate,
        };

        if (target.CreatedBy != null)
            comment.CreatedBy = target.CreatedBy.DisplayName;

        if (target.ModifiedBy != null)
            comment.ModifiedBy = target.ModifiedBy.DisplayName;

        if (!string.IsNullOrWhiteSpace(comment.Text) && comment.Text.Contains("_apis/wit/attachments"))
        {
            var projectUrl = target.Url.Substring(0, target.Url.IndexOf("/_apis/wit"));

            var images = FindImages(comment.Text);

            if (images.Any())
            {
                foreach (var imgUrl in images)
                {
                    var regEx = new Regex(@"_apis/wit/attachments/(?<fileId>.*)\?fileName=(?<fileName>.*)");

                    var matches = regEx.Matches(imgUrl);

                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            var fileId = match.Groups[1].Value;

                            var fileName = match.Groups[2].Value;

                            var fileUrl = $"{projectUrl}/_apis/wit/attachments/{fileId}?fileName={fileName}";

                            agileItem.Attachments.Add(new AgileItemAttachment()
                            {
                                ParentItemId = comment.Id.ToString(),
                                RemoteId = fileId,
                                Type = AgileItemAttachmentType.Comment,
                                FileName = $"{fileId}_{fileName}",
                                Url = fileUrl,
                            });
                        }
                    }
                }
            }



            comment.Text = HttpUtility.HtmlEncode(comment.Text);
        }

        return Task.FromResult(comment);
    }

    /// <summary>
    /// Converts to localasync.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="fields">The fields.</param>
    /// <param name="workItemFields">The work item fields.</param>
    /// <returns></returns>
    public static async Task<List<AgileWorkItem>> ToLocalAsync(this IEnumerable<WorkItem> items, Dictionary<string, string> fields, IEnumerable<WorkItemField> workItemFields)
    {
        var results = new List<AgileWorkItem>();

        foreach (var item in items)
            results.Add(await item.ToLocalAsync(fields, workItemFields));

        return results;
    }

    /// <summary>
    /// Converts to localasync.
    /// </summary>
    /// <param name="items">The items.</param>
    /// <param name="agileItem">The agile item.</param>
    /// <returns></returns>
    public static async Task<List<AgileItemComment>> ToLocalAsync(this IEnumerable<Comment> items, AgileWorkItem agileItem)
    {
        var results = new List<AgileItemComment>();

        foreach (var item in items)
        {
            results.Add(await item.ToLocal(agileItem));
        }

        return results;

    }

    /// <summary>
    /// Finds the images.
    /// </summary>
    /// <param name="html">The html.</param>
    private static IEnumerable<string> FindImages(string html)
    {
        var document = new HtmlDocument();

        document.LoadHtml(html);

        return document.DocumentNode
            .SelectNodes("//img[@src]")
            ?.Select(x => x.GetAttributeValue("src", string.Empty))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            ?? Enumerable.Empty<string>();
    }
}
