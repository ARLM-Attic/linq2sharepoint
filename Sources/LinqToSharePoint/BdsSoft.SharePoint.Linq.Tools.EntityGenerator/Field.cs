/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.CodeDom;

namespace BdsSoft.SharePoint.Linq.Tools.EntityGenerator
{
    public class Field
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string SharePointType { get; set; }
        public bool IsHidden { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsCalculated { get; set; }
        public bool IsRequired { get; set; }
        public Type RuntimeType { get; set; }
        public FieldType FieldType { get; set; }
        public string LookupList { get; set; }
        public string LookupField { get; set; }
        public bool FillInChoiceEnabled { get; set; }
        public List<string> Choices { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldDefinition">SharePoint list field definition.</param>
        /// <returns></returns>
        public static Field FromCaml(XmlNode fieldDefinition)
        {
            //
            // Field object.
            //
            Field field = new Field();

            //
            // Field ID (GUID).
            //
            XmlAttribute aID = fieldDefinition.Attributes["ID"];
            if (aID != null)
                field.Id = new Guid(aID.Value);

            //
            // Field name.
            //
            field.Name = (string)fieldDefinition.Attributes["Name"].Value;
            field.DisplayName = (string)fieldDefinition.Attributes["DisplayName"].Value;

            //
            // Field description.
            //
            XmlAttribute aDescription = fieldDefinition.Attributes["Description"];
            if (aDescription != null)
                field.Description = (string)aDescription.Value;

            //
            // Field hidden?
            //
            XmlAttribute aHidden = fieldDefinition.Attributes["Hidden"];
            field.IsHidden = false;
            if (aHidden != null)
                field.IsHidden = bool.Parse(aHidden.Value);

            //
            // Field read-only?
            //
            XmlAttribute aReadOnly = fieldDefinition.Attributes["ReadOnly"];
            field.IsReadOnly = false;
            if (aReadOnly != null)
                field.IsReadOnly = bool.Parse(aReadOnly.Value);

            //
            // Field type.
            //
            string type = (string)fieldDefinition.Attributes["Type"].Value;
            field.IsCalculated = false;

            //
            // Calculated field. Use underlying type for mapping.
            //
            if (type == "Calculated")
            {
                field.SharePointType = (string)fieldDefinition.Attributes["ResultType"].Value;
                field.IsCalculated = true;
            }
            else
                field.SharePointType = type;

            //
            // Primary key field should be imported as well.
            //
            XmlAttribute primaryKey = fieldDefinition.Attributes["PrimaryKey"];
            field.IsPrimaryKey = false;
            if (primaryKey != null)
                field.IsPrimaryKey = bool.Parse(primaryKey.Value);

            //
            // Populate type information.
            //
            GetType(fieldDefinition, field);

            //
            // Return field definition object.
            //
            return field;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldDefinition">SharePoint list field definition.</param>
        /// <param name="field"></param>
        private static void GetType(XmlNode fieldDefinition, Field field)
        {
            //
            // Default case: no additional helper field needed.
            //
            //additional = false;

            //
            // Find SharePoint-type of the field.
            //
            string type = (string)fieldDefinition.Attributes["Type"].Value;

            //
            // Check whether the field is required or not. Will be used to make value types nullable if not required.
            //
            XmlAttribute aRequired = fieldDefinition.Attributes["Required"];
            bool required = false;
            if (aRequired != null)
                required = bool.Parse(aRequired.Value);
            field.IsRequired = required;

            //
            // Convert calculated type to underlying result type for representation in the entity type.
            //
            if (type == "Calculated")
                type = (string)fieldDefinition.Attributes["ResultType"].Value;

            //
            // Analyze the types.
            //
            switch (type)
            {
                //
                // Boolean == bool or bool?
                //
                case "Boolean":
                    field.FieldType = FieldType.Boolean;
                    field.RuntimeType = required ? typeof(bool) : typeof(bool?);
                    break;
                //
                // Text == string
                //
                case "Text":
                    field.FieldType = FieldType.Text;
                    field.RuntimeType = typeof(string);
                    break;
                //
                // Note == string
                //
                case "Note":
                    field.FieldType = FieldType.Note;
                    field.RuntimeType = typeof(string);
                    break;
                //
                // DateTime == System.DateTime
                //
                case "DateTime":
                    field.FieldType = FieldType.DateTime;
                    field.RuntimeType = required ? typeof(DateTime) : typeof(DateTime?);
                    break;
                //
                // Number == double or double?
                //
                case "Number":
                    field.FieldType = FieldType.Number;
                    field.RuntimeType = required ? typeof(double) : typeof(double?);
                    break;
                //
                // Currency == double or double?
                //
                case "Currency":
                    field.FieldType = FieldType.Currency;
                    field.RuntimeType = required ? typeof(double) : typeof(double?);
                    break;
                //
                // Counter == int (used for primary key field)
                //
                case "Counter":
                    field.FieldType = FieldType.Counter;
                    field.RuntimeType = typeof(int);
                    break;
                //
                // URL == Url (helper object from LINQ to SharePoint)
                //
                case "URL":
                    field.FieldType = FieldType.URL;
                    field.RuntimeType = typeof(Url);
                    break;
                //
                // Choice and MultiChoice are mapped on helper enums, (optionally) together with a helper string field for fill-in choices.
                // The type is made nullable if a value is not required.
                //
                case "Choice":
                case "MultiChoice":
                    field.FieldType = (type == "Choice" ? FieldType.Choice : FieldType.MultiChoice);

                    //
                    // Get choices.
                    //
                    field.Choices = new List<string>();
                    foreach (XmlNode choice in fieldDefinition["CHOICES"])
                        field.Choices.Add(choice.InnerText);

                    //
                    // Additional fill-in field needed if FillInChoice is set.
                    //
                    XmlAttribute fillInChoice = fieldDefinition.Attributes["FillInChoice"];
                    field.FillInChoiceEnabled = fillInChoice != null && bool.Parse((string)fillInChoice.Value);
                    break;
                //
                // Lookup fields require the generation of another linked entity type.
                //
                case "Lookup":
                    field.FieldType = FieldType.Lookup;
                    field.LookupList = (string)fieldDefinition.Attributes["List"].Value;
                    field.LookupField = (string)fieldDefinition.Attributes["ShowField"].Value;
                    break;
                //
                // LookupMulti fields require the generation of another linked entity type.
                //
                case "LookupMulti":
                    field.FieldType = FieldType.LookupMulti;
                    field.LookupList = (string)fieldDefinition.Attributes["List"].Value;
                    field.LookupField = (string)fieldDefinition.Attributes["ShowField"].Value;
                    break;
                //
                // Currently no support for User and UserMulti fields.
                //
                case "User":
                case "UserMulti":
                    break;
                //
                // Ignore other types too.
                //
                default:
                    field.FieldType = FieldType.None;
                    field.RuntimeType = null;
                    break;
            }
        }
    }
}
