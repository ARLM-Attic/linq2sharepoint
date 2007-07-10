﻿/*
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
 * 0.2.1 - Introduction of Helpers.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Class with helper methods used by the query parser.
    /// </summary>
    static internal class Helpers
    {
        /// <summary>
        /// Retrieves the FieldAttribute applied on the specified entity property, if one is set.
        /// </summary>
        /// <param name="member">Entity property to examine for a FieldAttribute.</param>
        /// <returns>The FieldAttribute applied on the specified entity property; null if not set.</returns>
        internal static FieldAttribute GetFieldAttribute(PropertyInfo member)
        {
            //
            // Get custom attributes of type FieldAttribute that are applied on the member.
            //
            FieldAttribute[] fa = member.GetCustomAttributes(typeof(FieldAttribute), false) as FieldAttribute[];
            if (fa != null && fa.Length != 0 && fa[0] != null)
                return fa[0];
            else
                return null;
        }

        /// <summary>
        /// Helper method to get the ListAttribute applied for the given entity type. An InvalidOperationException will be thrown if no ListAttribute is found.
        /// </summary>
        /// <param name="type">Type to get the ListAttribute for.</param>
        /// <returns>ListAttribute applied on the entity object.</returns>
        internal static ListAttribute GetListAttribute(Type type)
        {
            ListAttribute[] la = type.GetCustomAttributes(typeof(ListAttribute), false) as ListAttribute[];
            if (la != null && la.Length != 0)
                return la[0];
            else
                throw new InvalidOperationException("Missing ListAttribute on the entity type.");
        }

        /// <summary>
        /// Helper method to log query information before fetching results.
        /// </summary>
        /// <param name="output">Output to write log information to.</param>
        /// <param name="where">Query predicate.</param>
        /// <param name="order">Ordering clause.</param>
        /// <param name="projection">Projection clause.</param>
        internal static void LogTo(TextWriter output, XmlElement where, XmlElement order, XmlElement projection)
        {
            //
            // We'll output XML representing various CAML elements. Output should be indented for natural reading.
            //
            XmlTextWriter xw = new XmlTextWriter(output);
            xw.Formatting = Formatting.Indented;

            //
            // Write the query, including the predicate and the ordering element.
            //
            xw.WriteStartElement("Query");
            if (where != null && where.ChildNodes.Count != 0)
                where.WriteTo(xw);
            if (order != null)
                order.WriteTo(xw);
            xw.WriteEndElement();

            //
            // If a projection clause is present, we'll print that too.
            //
            if (projection != null)
            {
                XmlDocument projectDoc = new XmlDocument();
                projectDoc.LoadXml(projection.OuterXml);
                projectDoc.Save(xw);
            }

            //
            // Flush output.
            //
            output.Flush();
        }

        internal static void FindPrimaryKey(Type t, out FieldAttribute pkField, out PropertyInfo pkProp, bool required)
        {
            pkField = null;
            pkProp = null;

            //
            // Find field attribute and corresponding property for the field with PrimaryKey attribute value set to true.
            //
            foreach (PropertyInfo property in t.GetProperties())
            {
                FieldAttribute field = Helpers.GetFieldAttribute(property);
                if (field != null && field.PrimaryKey && field.FieldType == FieldType.Counter)
                {
                    if (pkField != null)
                        throw new InvalidOperationException("More than one primary key field found on entity type. There should only be one field marked as primary key on each entity type.");

                    pkField = field;
                    pkProp = property;
                    break;
                }
            }

            //
            // Primary key field should be present in order to make the query.
            //
            if (required && (pkField == null || pkProp == null))
                throw new InvalidOperationException("No primary key field found on entity type.");
        }
    }
}
