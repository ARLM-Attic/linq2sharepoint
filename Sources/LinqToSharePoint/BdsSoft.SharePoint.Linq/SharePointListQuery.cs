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
 * 0.2.1 - Introduction of SharePointListQuery<T>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Represents a SharePoint list query.
    /// </summary>
    /// <typeparam name="T">Type of the query result objects.</typeparam>
    class SharePointListQuery<T> : IOrderedQueryable<T>
    {
        /// <summary>
        /// Data context object used to connect to SharePoint.
        /// </summary>
        private SharePointDataContext _context;

        /// <summary>
        /// Expression representing the query.
        /// </summary>
        private Expression _expression;

        /// <summary>
        /// Creates a query object for querying a SharePoint list.
        /// </summary>
        /// <param name="context">Data context object used to connect to SharePoint.</param>
        /// <param name="expression">Expression representing the query.</param>
        public SharePointListQuery(SharePointDataContext context, Expression expression)
        {
            _context = context;
            _expression = expression;
        }

        /// <summary>
        /// Creates a query on top of the existing query. Allows incremental query definition as done in LINQ.
        /// </summary>
        /// <typeparam name="TElement">Type of the query result objects.</typeparam>
        /// <param name="expression">Expression representing the query.</param>
        /// <returns>Query object representing the combined list query.</returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new SharePointListQuery<TElement>(_context, expression);
        }

        /// <summary>
        /// Triggers the query and fetches results.
        /// </summary>
        /// <returns>Query results.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _context.ExecuteQuery<T>(_expression);
        }

        /// <summary>
        /// Triggers the query and fetches results.
        /// </summary>
        /// <returns>Query results.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _context.ExecuteQuery<T>(_expression);
        }

        /// <summary>
        /// Gets the type of the elements returned by the query.
        /// </summary>
        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Gets the expression representing the query.
        /// </summary>
        public Expression Expression
        {
            get
            {
                return _expression;
            }
        }

        /// <summary>
        /// Executes the query and returns a single result of the specified type.
        /// </summary>
        /// <typeparam name="TResult">Type of the query result object.</typeparam>
        /// <param name="expression">Expression representing the query.</param>
        /// <returns>Singleton query result object.</returns>
        public TResult Execute<TResult>(Expression expression)
        {
            IEnumerator<TResult> res = _context.ExecuteQuery<TResult>(expression);
            if (res.MoveNext())
                return res.Current;
            else
                throw new InvalidOperationException("Query did not return any results.");
        }

        #region Not implemented

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        #endregion
    }
}
