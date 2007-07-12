/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

/*
 * Version history:
 * 
 * 0.2.2 - New entity model; added Storage property to FieldAttribute
 */

#region Namespace imports

using System;
using Microsoft.SharePoint;

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Mapping attribute for SharePoint list fields, applied on entity class properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FieldAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Creates a field mapping to the specified underlying list field in SharePoint.
        /// </summary>
        /// <param name="field">SharePoint list field where the property is mapped to.</param>
        /// <param name="fieldType">SharePoint type of the list field.</param>
        /// <remarks>The field name should not be XML-encoded. This will be done automatically if needed.</remarks>
        public FieldAttribute(string field, FieldType fieldType)
        {
            Field = field;
            FieldType = fieldType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the list field in SharePoint.
        /// </summary>
        /// <remarks>The field name should not be XML-encoded. This will be done automatically if needed.</remarks>
        public string Field
        {
            get;
            private set;
        }

        /// <summary>
        /// List field type in SharePoint.
        /// </summary>
        public FieldType FieldType
        {
            get;
            private set;
        }

        /// <summary>
        /// String-representation of the GUID that uniquely identifies the list field in SharePoint.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates that the field is mapped to the primary key field of the list.
        /// </summary>
        public bool PrimaryKey
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the field shown in a Lookup field.
        /// </summary>
        public string LookupDisplayField
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the list field in SharePoint is read-only.
        /// </summary>
        public bool ReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the list field in SharePoint is calculated.
        /// </summary>
        public bool Calculated
        {
            get;
            set;
        }

        /// <summary>
        /// Points to a string property in the entity type that contains the fill-in choice of a multi-choice list field in SharePoint.
        /// </summary>
        public string OtherChoice
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates that the field is unique across all list entries. Used for Lookup field references that require uniqueness enforcement.
        /// </summary>
        [Obsolete("Lookup field uniqueness is now enforced by the query parser, causing this property to be redundant.", false)]
        public bool IsUnique
        {
            get;
            set;
        }

        /// <summary>
        /// References the entity class field that holds the value of the list field.
        /// Used to set read-only fields and to bypass the property getters and setters in some cases.
        /// </summary>
        public string Storage
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// Attribute holds metadata about the underlying SharePoint list on an entity type class definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ListAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Creates an entity class mapping to the specified underlying SharePoint list.
        /// </summary>
        /// <param name="list">SharePoint list where the entity type is mapped to.</param>
        public ListAttribute(string list)
        {
            List = list;
        }

        #endregion

        #region Properties

        /// <summary>
        /// SharePoint list where the entity type is mapped to.
        /// </summary>
        public string List
        {
            get;
            private set;
        }

        /// <summary>
        /// String-representation of the GUID that uniquely identifies the SharePoint list.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Version number of the SharePoint list.
        /// </summary>
        public int Version
        {
            get;
            set;
        }

        /// <summary>
        /// Relative URL path to the SharePoint list on the server.
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// Mapping attribute for (multi-)choice values to enum fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ChoiceAttribute : Attribute
    {
        #region Constructors

        /// <summary>
        /// Creates an enum field mapping to the specified underlying SharePoint choice value.
        /// </summary>
        /// <param name="choice">SharePoint choice value where the field is mapped to.</param>
        /// <remarks>The choice value should not be XML-encoded. This will be done automatically if needed.</remarks>
        public ChoiceAttribute(string choice)
        {
            Choice = choice;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the choice value in SharePoint.
        /// </summary>
        /// <remarks>The choice value should not be XML-encoded. This will be done automatically if needed.</remarks>
        public string Choice
        {
            get;
            private set;
        }

        #endregion
    }

    /// <summary>
    /// Supported SharePoint field types for entity property mapping.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// Not used.
        /// </summary>
        None = SPFieldType.Invalid,

        /// <summary>
        /// Boolean field.
        /// </summary>
        Boolean = SPFieldType.Boolean,

        /// <summary>
        /// Calculated field.
        /// </summary>
        [Obsolete("Calculated fields are not directly supported; use the underlying type of the Calculated field.")]
        Calculated = SPFieldType.Calculated,

        /// <summary>
        /// Choice field.
        /// </summary>
        Choice = SPFieldType.Choice,

        /// <summary>
        /// Counter field. Used for primary key fields.
        /// </summary>
        Counter = SPFieldType.Counter,

        /// <summary>
        /// Currency field.
        /// </summary>
        Currency = SPFieldType.Currency,

        /// <summary>
        /// DateTime field.
        /// </summary>
        DateTime = SPFieldType.DateTime,

        /// <summary>
        /// Integer field.
        /// </summary>
        Integer = SPFieldType.Integer,

        /// <summary>
        /// Lookup field.
        /// </summary>
        Lookup = SPFieldType.Lookup,

        /// <summary>
        /// Multiple lookup field.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
        LookupMulti = 101,

        /// <summary>
        /// MultiChoice field.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Multi")]
        MultiChoice = SPFieldType.MultiChoice,

        /// <summary>
        /// Note field.
        /// </summary>
        Note = SPFieldType.Note,

        /// <summary>
        /// Number field.
        /// </summary>
        Number = SPFieldType.Number,

        /// <summary>
        /// Text field.
        /// </summary>
        Text = SPFieldType.Text,

        /// <summary>
        /// URL field.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased", MessageId = "URL")]
        URL = SPFieldType.URL
    }
}