using System;
using System.Collections.Generic;
using System.Text;

namespace LoDaTek.AzureDevOps.Services.Client.Enums
{
    /// <summary>
    /// Agile Item Field Type
    /// </summary>
    public enum AgileItemFieldType
    {
        /// <summary>
        /// String field type.
        /// </summary>
        String = 0,
        //
        // Summary:
        //     Integer field type.
        Integer = 1,
        //
        // Summary:
        //     Datetime field type.
        DateTime = 2,
        //
        // Summary:
        //     Plain text field type.
        PlainText = 3,
        //
        // Summary:
        //     HTML (Multiline) field type.
        Html = 4,
        //
        // Summary:
        //     Treepath field type.
        TreePath = 5,
        //
        // Summary:
        //     History field type.
        History = 6,
        //
        // Summary:
        //     Double field type.
        Double = 7,
        //
        // Summary:
        //     Guid field type.
        Guid = 8,
        //
        // Summary:
        //     Boolean field type.
        Boolean = 9,
        //
        // Summary:
        //     Identity field type.
        Identity = 10,
        //
        // Summary:
        //     String picklist field type. When creating a string picklist field from REST API,
        //     use "String" FieldType.
        PicklistString = 11,
        //
        // Summary:
        //     Integer picklist field type. When creating a integer picklist field from REST
        //     API, use "Integer" FieldType.
        PicklistInteger = 12,
        //
        // Summary:
        //     Double picklist field type. When creating a double picklist field from REST API,
        //     use "Double" FieldType.
        PicklistDouble = 13
    }
}
