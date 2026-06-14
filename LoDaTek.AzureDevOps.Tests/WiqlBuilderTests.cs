using System;
using LoDaTek.AzureDevOps.Client.Builders;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoDaTek.AzureDevOps.Tests;

/// <summary>
/// Verifies the WIQL string generation for the documented parameter combinations.
/// </summary>
[TestClass]
public class WiqlBuilderTests
{
    [TestMethod]
    public void BuildString_AllTypes_ExcludesClosedByDefault()
    {
        var wiql = WiqlBuilder.BuildString(null, "MyProject", includeClosed: false, changedSince: null);

        StringAssert.Contains(wiql, "Where [System.WorkItemType] <> ''");
        StringAssert.Contains(wiql, "And [System.TeamProject] = 'MyProject'");
        StringAssert.Contains(wiql, "AND NOT [System.State] IN ('Completed', 'Closed')");
        StringAssert.EndsWith(wiql, "Order By [Id] Asc");
    }

    [TestMethod]
    public void BuildString_IncludeClosed_OmitsStateFilter()
    {
        var wiql = WiqlBuilder.BuildString(null, "MyProject", includeClosed: true, changedSince: null);

        Assert.IsFalse(wiql.Contains("AND NOT [System.State]"), "Closed filter should be omitted when includeClosed is true.");
    }

    [TestMethod]
    public void BuildString_WithWorkItemType_FiltersByTypeName()
    {
        var type = new WorkItemType { Name = "Bug" };

        var wiql = WiqlBuilder.BuildString(type, "MyProject", includeClosed: true, changedSince: null);

        StringAssert.Contains(wiql, "Where [System.WorkItemType] IN ('Bug')");
    }

    [TestMethod]
    public void BuildString_WithChangedSince_AddsSortableDateFilter()
    {
        var since = new DateTime(2026, 6, 1, 13, 30, 0, DateTimeKind.Utc);

        var wiql = WiqlBuilder.BuildString(null, "MyProject", includeClosed: true, changedSince: since);

        StringAssert.Contains(wiql, "AND [System.ChangedDate] > '2026-06-01T13:30:00'");
    }

    [TestMethod]
    public void BuildQuery_WrapsBuildStringResult()
    {
        var query = WiqlBuilder.BuildQuery("MyProject", includeClosed: true, changedSince: null);
        var expected = WiqlBuilder.BuildString(null, "MyProject", includeClosed: true, changedSince: null);

        Assert.AreEqual(expected, query.Query);
    }
}
