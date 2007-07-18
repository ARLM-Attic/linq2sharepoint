/*
 * LINQ to SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
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

        /// <summary>
        /// Gets a List definition object from a CAML list definition.
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
            list.Name = Helpers.GetFriendlyName((string)listDefinition.Attributes["Title"].Value);
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
        /// Generates the SPML representation for the List element.
        /// </summary>
        /// <returns>SPML XML element.</returns>
        public string ToSpml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement list = doc.CreateElement("List");
            list.Attributes.Append(doc.CreateAttribute("Id")).Value = this.Id.ToString("D");
            list.Attributes.Append(doc.CreateAttribute("Name")).Value = this.Name;
            list.Attributes.Append(doc.CreateAttribute("Description")).Value = this.Description;
            list.Attributes.Append(doc.CreateAttribute("Version")).Value = this.Version.ToString();
            list.Attributes.Append(doc.CreateAttribute("Path")).Value = this.Path;
            return list.OuterXml;
        }
    }
}
