using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.TeamFoundation.Core.WebApi;

/// <summary>
/// Class TeamProjectReferenceExtension.
/// </summary>
public static class TeamProjectReferenceExtension
{
    /// <summary>
    /// Determines whether the specified project is empty, has an Id that is Guid.Empty
    /// </summary>
    /// <param name="project">The project.</param>
    /// <returns><c>true</c> if the specified project is empty; otherwise, <c>false</c>.</returns>
    public static bool IsEmpty(this TeamProjectReference project)
    {
        return project.Id.Equals(Guid.Empty);
    }
}
