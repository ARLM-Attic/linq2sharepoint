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

namespace BdsSoft.SharePoint.Linq.Tools.EntityGenerator
{
    public class List
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }
        public string Path { get; set; }
        public List<Field> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listDefinition">SharePoint list definition.</param>
        /// <returns></returns>
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
            foreach (XmlNode c in listDefinition["Fields"].ChildNodes)
                list.Fields.Add(Field.FromCaml(c));

            //
            // Return list definition object.
            //
            return list;
        }
    }
}
