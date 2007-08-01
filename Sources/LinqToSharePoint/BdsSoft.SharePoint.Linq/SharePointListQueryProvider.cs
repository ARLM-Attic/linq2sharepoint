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
 * 0.2.3 - Orcas Beta 2 changes
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// 
    /// </summary>
    class SharePointListQueryProvider : IQueryProvider
    {
        #region Private members

        /// <summary>
        /// Data context object used to connect to SharePoint.
        /// </summary>
        private SharePointDataContext _context;

        #endregion

        internal SharePointListQueryProvider(SharePointDataContext context)
        {
            _context = context;
        }

        public static SharePointListQueryProvider GetInstance(SharePointDataContext context)
        {
            return new SharePointListQueryProvider(context);
        }

        #region IQueryProvider implementation

        /// <summary>
        /// Creates a query for the list source.
        /// </summary>
        /// <typeparam name="TElement">Type of the query result objects.</typeparam>
        /// <param name="expression">Expression representing the query.</param>
        /// <returns>Query object representing the list query.</returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return new SharePointListQuery<TElement>(_context, expression);
        }

        /// <summary>
        /// Executes the query and returns a single result of the specified type.
        /// </summary>
        /// <typeparam name="TResult">Type of the query result object.</typeparam>
        /// <param name="expression">Expression representing the query.</param>
        /// <returns>Singleton query result object.</returns>
        public TResult Execute<TResult>(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            //
            // We expect a method call expression.
            //
            MethodCallExpression mc = expression as MethodCallExpression;
            if (mc != null && mc.Method.DeclaringType == typeof(Queryable))
            {
                //
                // First and FirstOrDefault query operators.
                //
                if (mc.Method.Name == "First" || mc.Method.Name == "FirstOrDefault")
                {
                    //
                    // Execute the query. The parser will take care of the retrieval of only one item.
                    //
                    IEnumerator<TResult> res = _context.ExecuteQuery<TResult>(expression);

                    //
                    // Return the first element of the sequence, if it exists.
                    //
                    if (res.MoveNext())
                        return res.Current;
                    //
                    // Empty sequence is valid for FirstOrDefault call. Return the default value of TResult.
                    //
                    else if (mc.Method.Name.EndsWith("OrDefault", StringComparison.Ordinal))
                        return default(TResult);
                    //
                    // Empty sequence is invalid for First call.
                    //
                    else
                        throw RuntimeErrors.EmptySequence();
                }
                else
                    throw RuntimeErrors.UnsupportedQueryOperator(mc.Method.Name);
            }

            throw RuntimeErrors.FatalError();
        }

        #endregion

        #region Not implemented

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
