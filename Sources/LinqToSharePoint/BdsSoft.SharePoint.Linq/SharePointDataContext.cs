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
using System.Net;
using System.IO;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Data context used for LINQ to SharePoint querying.
    /// </summary>
    public class SharePointDataContext : IDisposable
    {
        #region Private members

        #region Connection information

        /// <summary>
        /// Web services URI to connect to SharePoint.
        /// Will be null when using the SharePoint object model instead of web services.
        /// </summary>
        private Uri _wsUri;

        /// <summary>
        /// Web services proxy object to connect to SharePoint.
        /// Will be null when using the SharePoint object model instead of web services.
        /// </summary>
        internal Lists _wsProxy;

        /// <summary>
        /// SharePoint site object to connect to SharePoint.
        /// Will be null when using web services instead of the SharePoint object model.
        /// </summary>
        internal SPSite _site;

        #endregion

        /// <summary>
        /// Dictionary of SharePointListSource objects for entities. The key represents the entity type, the value the SharePointListSource object.
        /// </summary>
        private Dictionary<Type, object> _lists = new Dictionary<Type, object>();

        /// <summary>
        /// Logger object to report information about the query when executing.
        /// </summary>
        private TextWriter _log;

        /// <summary>
        /// Indicates whether the version of the list on the SharePoint server should be matched against the list version as indicated by the metadata on the list items entity type. (Default: true)
        /// </summary>
        private bool _checkVersion = true;

        /// <summary>
        /// Used for Lookup fields. Indicates whether or not a check has to be performed to make sure that a child entity's field referenced by a Lookup field is unique.
        /// </summary>
        private bool _enforceLookupFieldUniqueness = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a data context object using the SharePoint web services to connect to SharePoint.
        /// </summary>
        /// <param name="wsUri">URI to the SharePoint site.</param>
        public SharePointDataContext(Uri wsUri)
        {
            _wsUri = wsUri;
            _wsProxy = new Lists();
            _wsProxy.Url = wsUri.AbsoluteUri.TrimEnd('/') + "/_vti_bin/lists.asmx";
            _wsProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
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

        #endregion

        #region Properties

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
        /// Indicates whether or not a check has to be performed to make sure that a child entity's field referenced by a Lookup field is unique (on by default).
        /// </summary>
        /// <remarks>WARNING! Misuse of this property can cause invalid query results to be produced.</remarks>
        public bool EnforceLookupFieldUniqueness
        {
            get { return _enforceLookupFieldUniqueness; }
            set { _enforceLookupFieldUniqueness = value; }
        }

        /// <summary>
        /// Gets or sets a logger object to report information about the query when executing.
        /// </summary>
        public TextWriter Log
        {
            get { return _log; }
            set { _log = value; }
        }

        /// <summary>
        /// Gets or sets whether the version of the list on the SharePoint server should be matched against the list version as indicated by the metadata on the list items entity type. (Default: true)
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "CheckList")]
        public bool CheckListVersion
        {
            get { return _checkVersion; }
            set { _checkVersion = value; }
        }

        /// <summary>
        /// Gets or sets the network credentials to connect to the SharePoint web services.
        /// </summary>
        public ICredentials Credentials
        {
            get
            {
                if (_wsProxy == null)
                    throw new InvalidOperationException("Cannot use network credentials when using the SharePoint object model as the data source.");
                else
                    return _wsProxy.Credentials;
            }
            set
            {
                if (_wsProxy == null)
                    throw new InvalidOperationException("Cannot use network credentials when using the SharePoint object model as the data source.");
                else
                    _wsProxy.Credentials = value;
            }
        }

        #endregion

        /// <summary>
        /// Retrieves a list source object for the entity as specified by <typeparamref name="T">T</typeparamref>.
        /// </summary>
        /// <typeparam name="T">Entity type to get a list source object for.</typeparam>
        /// <returns>List source object for the specified entity type.</returns>
        /// <remarks>Implements a singleton pattern on a per-entity type basis.</remarks>
        public SharePointList<T> GetList<T>() where T : SharePointListEntity
        {
            Type t = typeof(T);

            if (!_lists.ContainsKey(t))
                _lists.Add(t, new SharePointList<T>(this));

            return (SharePointList<T>)_lists[t];
        }

        /// <summary>
        /// Executes the query specified by the expression parameter.
        /// </summary>
        /// <typeparam name="T">Type for the result objects.</typeparam>
        /// <param name="expression">Expression representing the query.</param>
        /// <returns>Query results.</returns>
        public IEnumerator<T> ExecuteQuery<T>(Expression expression)
        {
            CamlQuery query = CamlQuery.Parse(expression, false);

            return query.Execute<T>();
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
