using LoDaTek.AzureDevOps.Services.Client.Enums;

namespace LoDaTek.AzureDevOps.Services.Client.Models;

/// <summary>
/// Agile Item Field.
/// </summary>
public class AgileItemField
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type.</value>
    public AgileItemFieldType Type { get; set; }

    /// <summary>
    /// Gets or sets the label.
    /// </summary>
    /// <value>The label.</value>
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Value { get; set; }


}
