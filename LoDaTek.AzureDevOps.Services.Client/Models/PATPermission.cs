using System;
using System.Collections.Generic;
using System.Text;

namespace LoDaTek.AzureDevOps.Services.Client.Models
{
    /// <summary>
    /// PAT Permission.
    /// </summary>
    public class PATPermission
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>The actions.</value>
        public List<PATPermissionAction> Actions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has readable permissions.
        /// </summary>
        /// <value><c>true</c> if this instance has readable permissions; otherwise, <c>false</c>.</value>
        public bool HasReadablePermissions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PATPermission"/> class.
        /// </summary>
        public PATPermission()
        {
            Actions = new List<PATPermissionAction>();
        }

    }

    /// <summary>
    /// PAT Permission Action.
    /// </summary>
    public class PATPermissionAction
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        public bool CanRead { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can write.
        /// </summary>
        /// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
        public bool CanWrite { get; set; }


    }
}
