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
 * 0.2.3 - Grouping support.
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Key-based grouping of SharePoint list items.
    /// </summary>
    /// <typeparam name="K">Key type.</typeparam>
    /// <typeparam name="T">Element type.</typeparam>
    public class Grouping<K,T> : IGrouping<K,T>
    {
        #region Private members

        /// <summary>
        /// Key of the group.
        /// </summary>
        private K _key;

        /// <summary>
        /// Values in the group.
        /// </summary>
        private List<T> _values;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new group with the given key.
        /// </summary>
        /// <param name="key">Key of the group.</param>
        internal Grouping(K key)
        {
            _key = key;
            _values = new List<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Key of the group.
        /// </summary>
        public K Key
        {
            get { return _key; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a value to the group.
        /// </summary>
        /// <param name="item"></param>
        internal void Add(object item)
        {
            _values.Add((T)item);
        }

        /// <summary>
        /// Gets the items in the group.
        /// </summary>
        /// <returns>Items in the group.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <summary>
        /// Gets the items in the group.
        /// </summary>
        /// <returns>Items in the group.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
