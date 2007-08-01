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
 * 0.2.1 - Introduction of SharePointListQuery<T>
 */

#region Namespace imports

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Xml;

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Represents a SharePoint list query.
    /// </summary>
    /// <typeparam name="T">Type of the query result objects.</typeparam>
    [DebuggerVisualizer(typeof(SharePointListQueryVisualizer))]
    [Serializable]
    class SharePointListQuery<T> : IOrderedQueryable<T>, ISerializable
    {
        #region Private members

        /// <summary>
        /// Data context object used to connect to SharePoint.
        /// </summary>
        private SharePointDataContext _context;

        /// <summary>
        /// Expression representing the query.
        /// </summary>
        private Expression _expression;

        #endregion

        #region Constructors

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

        #endregion

        #region Properties

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
        /// Gets the query provider for LINQ support.
        /// </summary>
        public IQueryProvider Provider
        {
            get
            {
                return SharePointListQueryProvider.GetInstance(_context);
            }
        }

        #endregion

        #region Methods

        ///// <summary>
        ///// Creates a query on top of the existing query. Allows incremental query definition as done in LINQ.
        ///// </summary>
        ///// <typeparam name="TElement">Type of the query result objects.</typeparam>
        ///// <param name="expression">Expression representing the query.</param>
        ///// <returns>Query object representing the combined list query.</returns>
        //public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        //{
        //    return new SharePointListQuery<TElement>(_context, expression);
        //}

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

        ///// <summary>
        ///// Executes the query and returns a single result of the specified type.
        ///// </summary>
        ///// <typeparam name="TResult">Type of the query result object.</typeparam>
        ///// <param name="expression">Expression representing the query.</param>
        ///// <returns>Singleton query result object.</returns>
        //public TResult Execute<TResult>(Expression expression)
        //{
        //    //
        //    // We expect a method call expression.
        //    //
        //    MethodCallExpression mc = expression as MethodCallExpression;
        //    if (mc != null && mc.Method.DeclaringType == typeof(Queryable))
        //    {
        //        //
        //        // First and FirstOrDefault query operators.
        //        //
        //        if (mc.Method.Name == "First" || mc.Method.Name == "FirstOrDefault")
        //        {
        //            //
        //            // Execute the query. The parser will take care of the retrieval of only one item.
        //            //
        //            IEnumerator<TResult> res = _context.ExecuteQuery<TResult>(expression);

        //            //
        //            // Return the first element of the sequence, if it exists.
        //            //
        //            if (res.MoveNext())
        //                return res.Current;
        //            //
        //            // Empty sequence is valid for FirstOrDefault call. Return the default value of TResult.
        //            //
        //            else if (mc.Method.Name.EndsWith("OrDefault", StringComparison.Ordinal))
        //                return default(TResult);
        //            //
        //            // Empty sequence is invalid for First call.
        //            //
        //            else
        //                throw RuntimeErrors.EmptySequence();
        //        }
        //        else
        //            throw RuntimeErrors.UnsupportedQueryOperator(mc.Method.Name);
        //    }

        //    throw RuntimeErrors.FatalError();
        //}

        #endregion

        //#region Not implemented

        //public IQueryable CreateQuery(Expression expression)
        //{
        //    throw new NotImplementedException("The method or operation is not implemented.");
        //}

        //public object Execute(Expression expression)
        //{
        //    throw new NotImplementedException("The method or operation is not implemented.");
        //}

        //#endregion

        #region Debugger visualizer support

        #region Private members

        /// <summary>
        /// CAML of the query.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string _camlForDebuggerVisualizer;

        /// <summary>
        /// Entity name of source entities.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string _entityForDebuggerVisualizer;

        /// <summary>
        /// LINQ query.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private string _linqForDebuggerVisualizer;

        /// <summary>
        /// Error collection.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        private ParseErrorCollection _errorsForDebuggerVisualizer;

        #endregion

        #region Serialization support

        /// <summary>
        /// Constructor to support debugger visualizers. Not for direct use in end-user code.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <remarks>Query parser refactorings should allow to move this to another 'SharePointQuery' class level at a later stage.</remarks>
        private SharePointListQuery(SerializationInfo info, StreamingContext context)
        {
            _camlForDebuggerVisualizer = (string)info.GetValue("Caml", typeof(string));
            _linqForDebuggerVisualizer = (string)info.GetValue("Linq", typeof(string));
            _entityForDebuggerVisualizer = (string)info.GetValue("Entity", typeof(string));
            _errorsForDebuggerVisualizer = (ParseErrorCollection)info.GetValue("Errors", typeof(ParseErrorCollection));
        }

        /// <summary>
        /// Serialization support for debugger visualizers. Not for direct use in end-user code.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <remarks>Query parser refactorings should allow to move this to another 'SharePointQuery' class level at a later stage.</remarks>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //
            // Any errors in the validation parser mode (second parameter set to true) are considered fatal.
            //
            CamlQuery query = CamlQuery.Parse(_expression, true);

            //
            // Store the CAML query and the entity name.
            //
            info.AddValue("Caml", query.ToString());
            info.AddValue("Entity", query._results.EntityType.Name);

            //
            // Store errors, if any.
            //
            if (query._errors != null)
            {
                info.AddValue("Errors", query._errors);
                info.AddValue("Linq", query._errors.Expression);
            }
        }

        #endregion

        #endregion
    }
}
