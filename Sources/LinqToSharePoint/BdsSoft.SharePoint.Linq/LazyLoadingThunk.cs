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
 * 0.2.1 - New LazyLoadingThunk implementation.
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Helper interface for lazy loading.
    /// </summary>
    internal interface ILazyLoadingThunk
    {
        /// <summary>
        /// Loads the entity or set of entitites from the thunk.
        /// </summary>
        /// <returns></returns>
        object Load();
    }

    /// <summary>
    /// Lazy loading thunk. Helps to load lookup fields lazily and acts as a marker for fields not yet loaded.
    /// </summary>
    /// <typeparam name="R">Type of the lookup field to be loaded.</typeparam>
    internal class LazyLoadingThunk<R> : ILazyLoadingThunk
        where R : SharePointListEntity
    {
        #region Private members

        /// <summary>
        /// Context source to gain access to the list for list item lookup(s).
        /// </summary>
        private SharePointDataContext context;

        /// <summary>
        /// Unique id of the entity to be loaded from the child (lookup) list. Used for Lookup fields.
        /// </summary>
        private int? id;

        /// <summary>
        /// List of id values of the entities to be loaded from the child (multi-lookup) list. Used for LookupMulti fields.
        /// </summary>
        private int[] ids;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new lazy loading thunk referring to the context source and the id of the child entity as represented by <typeparamref name="R">R</typeparamref>. Used for Lookup fields.
        /// </summary>
        /// <param name="context">Context source. Will be used to get the child entity from the list represented by <typeparamref name="R">R</typeparamref>.</param>
        /// <param name="id">Unique id of the entity to be loaded from the child (lookup) list.</param>
        public LazyLoadingThunk(SharePointDataContext context, int id)
        {
            Debug.Assert(context != null);

            this.context = context;
            this.id = id;
        }

        /// <summary>
        /// Creates a new lazy loading thunk referring to the containing list source and the id of the child entities as represented by <typeparamref name="R">R</typeparamref>. Used for LookupMulti fields.
        /// </summary>
        /// <param name="context">Context source. Will be used to get the child entities from the list represented by <typeparamref name="R">R</typeparamref>.</param>
        /// <param name="ids">List of unique ids of the entities to be loaded from the child (lookup) list.</param>
        public LazyLoadingThunk(SharePointDataContext context, int[] ids)
        {
            Debug.Assert(context != null && ids != null);

            this.context = context;
            this.ids = ids;
        }

        #endregion

        #region ILazyLoadingThunk implementation

        /// <summary>
        /// Loads the entity or set of entities from the thunk represented by the thunk's entity id(s).
        /// </summary>
        /// <returns></returns>
        public object Load()
        {
            if (id != null)
                return context.GetList<R>().GetEntityById(id.Value);
            else
                return context.GetList<R>().GetEntitiesById(ids);
        }

        #endregion
    }
}
