using System;
using System.Collections.Generic;
using System.Text;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// Class AgileItemAttachment.
    /// </summary>
    public class AgileItemAttachment
    {
        private string _basePath;

        /// <summary>
        /// Gets or sets the parent item identifier.
        /// </summary>
        /// <value>The parent item identifier.</value>
        public string ParentItemId { get; set; }

        /// <summary>
        /// Gets or sets the remote identifier.
        /// </summary>
        /// <value>The remote identifier.</value>
        public string RemoteId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public AgileItemAttachmentType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets the localised path.
        /// </summary>
        /// <value>The localised path.</value>
        public string LocalisedPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_basePath))
                    return $"{ParentItemId}_{FileName}";

                return $"{_basePath}_{FileName}";
            }
        }

        /// <summary>
        /// Updates the base path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void UpdateBasePath(string fileName)
        {
            _basePath = fileName;
        }
    }
}
