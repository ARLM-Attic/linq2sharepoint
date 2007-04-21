﻿using System;
using Microsoft.SharePoint;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Mapping attribute for SharePoint list fields, applied on entity class properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FieldAttribute : Attribute
    {
        private string field;
        private FieldType fieldType;

        /// <summary>
        /// Creates a field mapping to the specified underlying list field in SharePoint.
        /// </summary>
        /// <param name="field">SharePoint list field where the property is mapped to.</param>
        /// <param name="fieldType">SharePoint type of the list field.</param>
        /// <remarks>The field name should not be XML-encoded. This will be done automatically if needed.</remarks>
        public FieldAttribute(string field, FieldType fieldType)
        {
            this.field = field;
            this.fieldType = fieldType;
        }

        /// <summary>
        /// Name of the list field in SharePoint.
        /// </summary>
        /// <remarks>The field name should not be XML-encoded. This will be done automatically if needed.</remarks>
        public string Field
        {
            get { return field; }
        }

        /// <summary>
        /// List field type in SharePoint.
        /// </summary>
        public FieldType FieldType
        {
            get { return fieldType; }
        }

        private string id;

        /// <summary>
        /// String-representation of the GUID that uniquely identifies the list field in SharePoint.
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private bool readOnly;

        /// <summary>
        /// Indicates whether the list field in SharePoint is read-only.
        /// </summary>
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        private bool calculated;

        /// <summary>
        /// Indicates whether the list field in SharePoint is calculated.
        /// </summary>
        public bool Calculated
        {
            get { return calculated; }
            set { calculated = value; }
        }

        private string otherChoice;

        /// <summary>
        /// Points to a string property in the entity type that contains the fill-in choice of a multi-choice list field in SharePoint.
        /// </summary>
        public string OtherChoice
        {
            get { return otherChoice; }
            set { otherChoice = value; }
        }
    }

    /// <summary>
    /// Attribute holds metadata about the underlying SharePoint list on an entity type class definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ListAttribute : Attribute
    {
        private string list;

        /// <summary>
        /// Creates an entity class mapping to the specified underlying SharePoint list.
        /// </summary>
        /// <param name="list">SharePoint list where the entity type is mapped to.</param>
        public ListAttribute(string list)
        {
            this.list = list;
        }

        /// <summary>
        /// SharePoint list where the entity type is mapped to.
        /// </summary>
        public string List
        {
            get { return list; }
        }

        private string id;

        /// <summary>
        /// String-representation of the GUID that uniquely identifies the SharePoint list.
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private int version;

        /// <summary>
        /// Version number of the SharePoint list.
        /// </summary>
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        private string path;

        /// <summary>
        /// Relative URL path to the SharePoint list on the server.
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
    }

    /// <summary>
    /// Mapping attribute for (multi-)choice values to enum fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ChoiceAttribute : Attribute
    {
        /// <summary>
        /// Creates an enum field mapping to the specified underlying SharePoint choice value.
        /// </summary>
        /// <param name="choice">SharePoint choice value where the field is mapped to.</param>
        /// <remarks>The choice value should not be XML-encoded. This will be done automatically if needed.</remarks>
        public ChoiceAttribute(string choice)
        {
            this.choice = choice;
        }

        private string choice;

        /// <summary>
        /// Name of the choice value in SharePoint.
        /// </summary>
        /// <remarks>The choice value should not be XML-encoded. This will be done automatically if needed.</remarks>
        public string Choice
        {
            get { return choice; }
        }
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
        /// Calculcated field.
        /// </summary>
        Calculated = SPFieldType.Calculated,

        /// <summary>
        /// Choice field.
        /// </summary>
        Choice = SPFieldType.Choice,

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