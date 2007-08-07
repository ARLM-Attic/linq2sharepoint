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
 * 0.2.3 - Introduction of Context class + SPML conversions
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
    /// Represents a SharePointDataContext with all the information required by LINQ to SharePoint.
    /// </summary>
    public class Context
    {
        #region Properties

        /// <summary>
        /// Friendly name for the data context.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url to the SharePoint site.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Connection parameters.
        /// </summary>
        public Connection Connection { get; set; }

        /// <summary>
        /// Lists contained by the data context.
        /// </summary>
        public List<List> Lists { get; set; }

        #endregion

        #region Factory methods

        /// <summary>
        /// Creates a Context definition object from a SPML data context definition.
        /// </summary>
        /// <param name="spml">SharePoint data context definition in SPML.</param>
        /// <returns>Context definition object for the specified data context.</returns>
        public static Context FromSpml(XmlNode spml)
        {
            //
            // Context object.
            //
            Context context = new Context();

            //
            // Set general context information.
            //
            context.Name = spml.Attributes["Name"].Value;
            context.Url = spml.Attributes["Url"].Value;
            context.Connection = Connection.FromSpml(spml["Connection"]);

            //
            // Get lists.
            //
            context.Lists = new List<List>();
            if (spml["Lists"] != null)
                foreach (XmlNode c in spml["Lists"].ChildNodes)
                    context.Lists.Add(List.FromSpml(c));

            //
            // Return context definition object.
            //
            return context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates the SPML representation for the Context element.
        /// </summary>
        /// <returns>SPML XML element.</returns>
        public XmlNode ToSpml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement ctx = doc.CreateElement("SharePointDataContext");
            doc.AppendChild(ctx);

            XmlAttribute name = doc.CreateAttribute("Name");
            name.Value = Name ?? "";
            ctx.Attributes.Append(name);

            XmlAttribute url = doc.CreateAttribute("Url");
            url.Value = Url;
            ctx.Attributes.Append(url);

            XmlAttribute ns = doc.CreateAttribute("xmlns");
            ns.Value = "http://www.codeplex.com/LINQtoSharePoint/SPML.xsd";
            ctx.Attributes.Append(ns);

            XmlElement lists = doc.CreateElement("Lists");
            ctx.AppendChild(lists);
            foreach (List list in Lists)
                lists.AppendChild(doc.ImportNode(list.ToSpml(), true));

            ctx.AppendChild(doc.ImportNode(Connection.ToSpml(), true));

            return ctx;
        }

        #endregion
    }
}
