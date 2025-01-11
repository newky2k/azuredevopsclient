using System;
using System.Collections.Generic;
using System.Text;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// Agile Work Item.
    /// </summary>
    public class AgileWorkItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the assigned to.
        /// </summary>
        /// <value>The assigned to.</value>
        public string AssignedTo { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        /// <value>The updated by.</value>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>The last updated.</value>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>The reason.</value>
        public string Reason { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>The fields.</value>
        public List<AgileItemField> Fields { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        /// <value>The attachments.</value>
        public List<AgileItemAttachment> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>The comments.</value>
        public List<AgileItemComment> Comments { get; set; }

        public AgileWorkItem()
        {
            Fields = new List<AgileItemField>();
            Attachments = new List<AgileItemAttachment>();
            Comments = new List<AgileItemComment>();

        }

        /// <summary>
        /// Updates the local path.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public void UpdateLocalPath(string filename)
        {
            foreach (var attachment in Attachments)
            {
                attachment.UpdateBasePath(filename);
            }
        }
    }
}
