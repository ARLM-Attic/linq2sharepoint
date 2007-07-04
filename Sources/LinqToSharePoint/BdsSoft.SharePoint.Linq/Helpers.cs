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
 * 0.2.1 - Introduction of Helpers.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

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
    }
}
