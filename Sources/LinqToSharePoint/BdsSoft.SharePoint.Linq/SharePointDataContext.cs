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
 * 0.2.1 - Introduction of SharePointDataContext.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Linq.Expressions;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Data context used for LINQ to SharePoint querying.
    /// </summary>
    public class SharePointDataContext : IDisposable
    {
        /// <summary>
        /// Web services URI to connect to SharePoint.
        /// Will be null when using the SharePoint object model instead of web services.
        /// </summary>
        private Uri _wsUri;

        /// <summary>
        /// Web services proxy object to connect to SharePoint.
        /// Will be null when using the SharePoint object model instead of web services.
        /// </summary>
        private Lists _wsProxy;

        /// <summary>
        /// SharePoint site object to connect to SharePoint.
        /// Will be null when using web services instead of the SharePoint object model.
        /// </summary>
        private SPSite _site;

        /// <summary>
        /// Dictionary of SharePointListSource objects for entities. The key represents the entity type, the value the SharePointListSource object.
        /// </summary>
        private Dictionary<Type, object> _lists = new Dictionary<Type, object>();

        /// <summary>
        /// Create a data context object using the SharePoint web services to connect to SharePoint.
        /// </summary>
        /// <param name="wsUri">URI to the SharePoint site.</param>
        public SharePointDataContext(Uri wsUri)
        {
            _wsUri = wsUri;
            _wsProxy = new Lists();
            _wsProxy.Url = wsUri.AbsoluteUri.TrimEnd('/') + "/_vti_bin/lists.asmx";
        }

        /// <summary>
        /// Create a data context object using the SharePoint object model to connect to SharePoint.
        /// </summary>
        /// <param name="site">SharePoint site object.</param>
        [CLSCompliant(false)]
        public SharePointDataContext(SPSite site)
        {
            _site = site;
        }

        /// <summary>
        /// Gets the web services URI used to connect to SharePoint.
        /// Will be null when using the SharePoint object model instead of web services.
        /// </summary>
        public Uri Url
        {
            get
            {
                return _wsUri;
            }
        }

        /// <summary>
        /// Gets the SharePoint site object used to connect to SharePoint.
        /// Will be null when using web services instead of the SharePoint object model.
        /// </summary>
        [CLSCompliant(false)]
        public SPSite Site
        {
            get
            {
                return _site;
            }
        }

        /// <summary>
        /// Retrieves a list source object for the entity as specified by <typeparamref name="T">T</typeparamref>.
        /// </summary>
        /// <typeparam name="T">Entity type to get a list source object for.</typeparam>
        /// <returns>List source object for the specified entity type.</returns>
        /// <remarks>Implements a singleton pattern on a per-entity type basis.</remarks>
        public SharePointListSource<T> GetList<T>() where T : SharePointListEntity
        {
            Type t = typeof(T);

            if (!_lists.ContainsKey(t))
                _lists.Add(t, new SharePointListSource<T>(this));

            return (SharePointListSource<T>)_lists[t];
        }

        /// <summary>
        /// Executes the query specified by the expression parameter.
        /// </summary>
        /// <typeparam name="T">Type for the result objects.</typeparam>
        /// <param name="expression">Expression representing the query.</param>
        /// <returns>Query results.</returns>
        public IEnumerable<T> ExecuteQuery<T>(Expression expression)
        {
            CamlQuery query = CamlQuery.Parse(expression);

            yield break;
        }

        #region Dispose pattern implementation

        /// <summary>
        /// Track whether the Dispose method has been called.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// Releases all resources used by the data source object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Helper method to support resource disposal.
        /// </summary>
        /// <param name="disposing">Indicates the source of the dispose call.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_wsProxy != null)
                        _wsProxy.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}
