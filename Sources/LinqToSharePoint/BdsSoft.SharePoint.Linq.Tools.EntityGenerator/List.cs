/*
 * LINQ to SharePoint
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
 * 0.2.1 - Introduction of List class
 * 0.2.3 - SPML conversions
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.EntityGenerator
{
    /// <summary>
    /// Represents a SharePoint list with all the information required by LINQ to SharePoint.
    /// </summary>
    public class List
    {
        #region Properties

        /// <summary>
        /// List identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// List name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Version of the list, retrieved at entity generation time.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Relative path to the list on the SharePoint site.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// List of exported list fields.
        /// </summary>
        public List<Field> Fields { get; set; }

        #endregion

        #region Factory methods

        /// <summary>
        /// Creates a List definition object from a CAML list definition.
        /// </summary>
        /// <param name="listDefinition">SharePoint list definition in CAML.</param>
        /// <returns>List definition object for the specified list.</returns>
        public static List FromCaml(XmlNode listDefinition)
        {
            //
            // List object.
            //
            List list = new List();

            //
            // Set general list information.
            //
            list.Name = (string)listDefinition.Attributes["Title"].Value;
            string listDescription = (string)listDefinition.Attributes["Description"].Value;
            if (listDescription != "")
                list.Description = null;
            list.Id = new Guid((string)listDefinition.Attributes["ID"].Value);
            list.Version = int.Parse(listDefinition.Attributes["Version"].Value);
            list.Path = (string)listDefinition.Attributes["RootFolder"].Value;

            //
            // Get fields.
            //
            list.Fields = new List<Field>();
            if (listDefinition["Fields"] != null)
            {
                foreach (XmlNode c in listDefinition["Fields"].ChildNodes)
                    list.Fields.Add(Field.FromCaml(c));
            }

            //
            // Return list definition object.
            //
            return list;
        }

        /// <summary>
        /// Creates a List definition object from a SPML list definition.
        /// </summary>
        /// <param name="spml">SharePoint list definition in SPML.</param>
        /// <returns>List definition object for the specified list.</returns>
        public static List FromSpml(XmlNode spml)
        {
            //
            // List object.
            //
            List list = new List();

            //
            // Set general list information.
            //
            list.Name = spml.Attributes["Name"].Value;
            string listDescription = (string)spml.Attributes["Description"].Value;
            if (listDescription != "")
                list.Description = null;
            list.Id = new Guid((string)spml.Attributes["Id"].Value);
            list.Version = int.Parse(spml.Attributes["Version"].Value);
            list.Path = (string)spml.Attributes["Path"].Value;

            //
            // Get fields.
            //
            list.Fields = new List<Field>();
            if (spml["Fields"] != null)
                foreach (XmlNode c in spml["Fields"].ChildNodes)
                    list.Fields.Add(Field.FromSpml(c));

            //
            // Return list definition object.
            //
            return list;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all of the fields supported by LINQ to SharePoint.
        /// </summary>
        /// <returns>Fields supported by LINQ to SharePoint.</returns>
        public IEnumerable<Field> GetKnownFields()
        {
            //
            // Export only fields that aren't hidden or the primary key field,
            // and for which the underlying type is recognized and supported by the mapper.
            //
            foreach (Field field in this.Fields)
                if (!field.IsHidden || field.IsPrimaryKey)
                    if (field.FieldType != FieldType.None)
                        yield return field;
        }

        /// <summary>
        /// Generates the SPML representation for the List element.
        /// </summary>
        /// <returns>SPML XML element.</returns>
        public XmlNode ToSpml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement list = doc.CreateElement("List");
            list.Attributes.Append(doc.CreateAttribute("Name")).Value = this.Name;
            list.Attributes.Append(doc.CreateAttribute("Description")).Value = this.Description;
            list.Attributes.Append(doc.CreateAttribute("Id")).Value = this.Id.ToString("D");
            list.Attributes.Append(doc.CreateAttribute("Version")).Value = this.Version.ToString();
            list.Attributes.Append(doc.CreateAttribute("Path")).Value = this.Path;

            XmlElement fields = doc.CreateElement("Fields");
            foreach (Field field in GetKnownFields())
                fields.AppendChild(doc.ImportNode(field.ToSpml(), true));
            list.AppendChild(fields);

            return list;
        }

        #endregion
    }
}
