
namespace LoDaTek.AzureDevOps.Services.Client.Enums;

/// <summary>
/// Agile Item Field Type
/// </summary>
public enum AgileItemFieldType
{
    /// <summary>
    /// String field type.
    /// </summary>
    String = 0,
    /// <summary>
    /// Integer field type.
    /// </summary>
    Integer = 1,
    /// <summary>
    /// Datetime field type.
    /// </summary>
    DateTime = 2,
    /// <summary>
    /// Plain text field type.
    /// </summary>
    PlainText = 3,
    /// <summary>
    /// HTML (Multiline) field type.
    /// </summary>
    Html = 4,
    /// <summary>
    /// Treepath field type.
    /// </summary>
    TreePath = 5,
    /// <summary>
    /// History field type.
    /// </summary>
    History = 6,
    /// <summary>
    /// Double field type.
    /// </summary>
    Double = 7,
    /// <summary>
    /// Guid field type.
    /// </summary>
    Guid = 8,
    /// <summary>
    /// Boolean field type.
    /// </summary>
    Boolean = 9,
    /// <summary>
    /// Identity field type.
    /// </summary>
    Identity = 10,
    /// <summary>
    /// String picklist field type. When creating a string picklist field from REST API, use "String" FieldType.
    /// </summary>
    PicklistString = 11,
    /// <summary>
    /// Integer picklist field type. When creating an integer picklist field from REST API, use "Integer" FieldType.
    /// </summary>
    PicklistInteger = 12,
    /// <summary>
    /// Double picklist field type. When creating a double picklist field from REST API, use "Double" FieldType.
    /// </summary>
    PicklistDouble = 13
}
