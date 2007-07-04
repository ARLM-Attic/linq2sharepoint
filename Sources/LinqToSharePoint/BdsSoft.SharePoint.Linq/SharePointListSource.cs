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
 * 0.2.1 - Introduction of SharePointListSource<T>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;
using System.Reflection;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Data source object for querying of a SharePoint list as specified by <typeparamref name="T">T</typeparamref>.
    /// </summary>
    /// <typeparam name="T">Entity type for the underlying SharePoint list.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class SharePointListSource<T> : IOrderedQueryable<T> where T : SharePointListEntity
    {
        /// <summary>
        /// Data context object used to connect to SharePoint.
        /// </summary>
        private SharePointDataContext _context;

        /// <summary>
        /// Create a list source object for querying of a SharePoint list.
        /// </summary>
        /// <param name="context">Data context object used to connect to SharePoint.</param>
        public SharePointListSource(SharePointDataContext context)
        {
            _context = context;
        }

        #region Properties

        /// <summary>
        /// Gets the SharePoint data context object used to connect to this list.
        /// </summary>
        public SharePointDataContext Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// Gets the type of the elements held in the list data source object.
        /// </summary>
        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Gets the expression tree representation of the list data source object.
        /// </summary>
        public Expression Expression
        {
            get
            {
                return Expression.Constant(this);
            }
        }

        #endregion

        /// <summary>
        /// Creates a query for the list source.
        /// </summary>
        /// <typeparam name="TElement">Type of the query result objects.</typeparam>
        /// <param name="expression">Expression representing the query.</param>
        /// <returns>Query object representing the list query.</returns>
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new SharePointListQuery<TElement>(_context, expression);
        }

        /// <summary>
        /// Gets all entity objects from the SharePoint list.
        /// </summary>
        /// <returns>All entity objects from the SharePoint list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _context.ExecuteQuery<T>(this.Expression);
        }

        /// <summary>
        /// Gets all entity objects from the SharePoint list.
        /// </summary>
        /// <returns>All entity objects from the SharePoint list.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _context.ExecuteQuery<T>(this.Expression);
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
            throw new NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Support for entity retrieval by key value

        /// <summary>
        /// Cache of entities retrieved using a *ById method.
        /// </summary>
        private Dictionary<int, T> cache = new Dictionary<int, T>();

        /// <summary>
        /// Retrieves an entity by the given ID (primary key field).
        /// </summary>
        /// <param name="id">ID of the entity to retrieve.</param>
        /// <param name="fromCache">Used to indicate that entities should be looked up in the entity cache first before launching a query against SharePoint.</param>
        /// <returns>Entity object with the given ID; null if not found.</returns>
        public T GetEntityById(int id, bool fromCache)
        {
            //
            // Look in cache first.
            //
            if (fromCache && cache.ContainsKey(id))
                return cache[id];

            FieldAttribute pkField = null;
            PropertyInfo pkProp = null;

            //
            // Find field attribute and corresponding property for the field with PrimaryKey attribute value set to true.
            //
            foreach (PropertyInfo property in typeof(T).GetProperties())
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
            if (pkField == null || pkProp == null)
                throw new InvalidOperationException("No primary key field found on entity type.");

            //
            // Build a manual query representing this.Where(e => e.ID = id) with ID property being variable, based on pkProp.
            //
            ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
            MemberExpression pk = Expression.Property(parameter, pkProp);
            BinaryExpression byID = Expression.Equal(pk, Expression.Constant(id, typeof(int)));
            Expression<Func<T, bool>> filter = Expression.Lambda<Func<T, bool>>(byID, parameter);

            //
            // Return the result if found, null otherwise.
            // Remark: AsEnumerable() is required because calling SingleOrDefualt on the IQueryable directly triggers the Execute method (not implemented).
            //
            T result = Queryable.Where<T>(this, filter).AsEnumerable().SingleOrDefault();

            //
            // Cache the result and return it.
            //
            cache[id] = result;
            return result;
        }

        /// <summary>
        /// Retrieves a list of entities by the given set of IDs (primary key field).
        /// </summary>
        /// <param name="ids">IDs of the entities to retrieve.</param>
        /// <param name="fromCache">Used to indicate that entities should be looked up in the entity cache first before launching a query against SharePoint.</param>
        /// <returns>List of entity objects with the given IDs; null if not found.</returns>
        public IList<T> GetEntitiesById(int[] ids, bool fromCache)
        {
            //
            // TODO
            //
            // Replace naive implementation with <Or>-based query on identifier field, excluding items already in cache.
            // This implementation will launch a lot of small queries for each individual referenced entity.
            //
            List<T> lst = new List<T>();
            foreach (int id in ids)
                lst.Add(GetEntityById(id, fromCache));
            return lst;
        }

        #endregion
    }
}
