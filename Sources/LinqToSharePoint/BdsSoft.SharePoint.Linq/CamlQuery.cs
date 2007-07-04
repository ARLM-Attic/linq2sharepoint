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
 * 0.2.1 - Introduction of CamlQuery.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// CAML query class, used to represent, parse and execute CAML queries.
    /// </summary>
    internal class CamlQuery
    {
        /// <summary>
        /// Entity type of the source list items.
        /// </summary>
        private Type _entityType;

        /// <summary>
        /// Parses a CAML query based on an expression tree.
        /// </summary>
        /// <param name="expression">Expression tree to generate the CAML query object for.</param>
        /// <returns>CAML query object for the specified expression tree.</returns>
        public static CamlQuery Parse(Expression expression)
        {
            MethodCallExpression mce = expression as MethodCallExpression;
            ConstantExpression ce = expression as ConstantExpression;

            //
            // Method call expression represents a query operator.
            //
            if (mce != null)
            {
                CamlQuery q = Parse(mce.Arguments[0]);
                Console.WriteLine(mce.Method.Name);
                return q;
            }
            //
            // Constant expression represents the source of the query.
            //
            else if (ce != null)
            {
                Type t = ce.Value.GetType();
                if (t.GetGenericTypeDefinition() == typeof(SharePointListSource<>))
                {
                    //
                    // Create a query object and store the entity type.
                    //
                    CamlQuery q = new CamlQuery();
                    q._entityType = t.GetGenericArguments()[0];
                    return q;
                }
            }

            throw new Exception("Bang!");
        }
    }
}