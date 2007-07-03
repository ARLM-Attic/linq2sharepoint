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
 * 0.2.0 - Support for entity types deriving from SharePointEntityType
 *         Support for Lookup fields and lazy loading.
 * 
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Base class for entities used in LINQ to SharePoint.
    /// </summary>
    public class SharePointListEntity
    {
        /// <summary>
        /// Dictionary of fields with associated values for an entity instance.
        /// </summary>
        private Dictionary<string, object> fields;

        /// <summary>
        /// Default constructor for entities.
        /// </summary>
        public SharePointListEntity()
        {
            fields = new Dictionary<string, object>();
        }

        /// <summary>
        /// Retrieves the value from a specified field.
        /// </summary>
        /// <param name="field">Field name to get the value for.</param>
        /// <returns>Entity field value; can be null if not specified.</returns>
        /// <remarks>Calling GetValue can trigger lazy loading for lookup fields.</remarks>
        public object GetValue(string field)
        {
            //
            // Does the field exist in the fields dictionary?
            //
            if (fields.ContainsKey(field))
            {
                object o = fields[field];

                //
                // Lazy loading thunks can be present for lookup fields.
                // In such a case, load the entity through the thunk and replace the thunk by the retrieved value.
                //
                ILazyLoadingThunk ll = o as ILazyLoadingThunk;
                if (ll != null)
                {
                    object entity = ll.LoadEntity();
                    fields[field] = entity;
                    return entity;
                }
                else
                    return o;
            }
            //
            // Not found; return null (CHECK).
            //
            else
                return null;
        }

        /// <summary>
        /// Sets the value for the specified field.
        /// </summary>
        /// <param name="field">Field to be set.</param>
        /// <param name="value">Value to be assigned to the field.</param>
        public void SetValue(string field, object value)
        {
            fields[field] = value;
        }
    }

    /// <summary>
    /// Helper interface for lazy loading.
    /// </summary>
    internal interface ILazyLoadingThunk
    {
        /// <summary>
        /// Loads the entity from the thunk.
        /// </summary>
        /// <returns></returns>
        object LoadEntity();
    }

    /// <summary>
    /// Lazy loading thunk. Helps to load lookup fields lazily and acts as a marker for fields not yet loaded.
    /// </summary>
    /// <typeparam name="T">Original source representing the entity that contains the lookup field.</typeparam>
    /// <typeparam name="R">Type of the lookup field to be loaded.</typeparam>
    internal class LazyLoadingThunk<T, R> : ILazyLoadingThunk
    {
        /// <summary>
        /// Source for the containing list of the lookup field. Will be used to get the child entity from.
        /// This allows for caching of loaded child entities on the level of the containing entity.
        /// </summary>
        private SharePointDataSource<T> source;

        /// <summary>
        /// Unique id of the entity to be loaded from the child (lookup) list. Used for Lookup fields.
        /// </summary>
        private int? id;

        /// <summary>
        /// List of id values of the entities to be loaded from the child (multi-lookup) list. Used for LookupMulti fields.
        /// </summary>
        private int[] ids;

        /// <summary>
        /// Creates a new lazy loading thunk referring to the containing list source and the id of the child entity as represented by <typeparamref name="R">R</typeparamref>. Used for Lookup fields.
        /// </summary>
        /// <param name="source">Source for the containing list of the lookup field. Will be used to get the child entity from. This allows for caching of loaded child entities on the level of the containing entity.</param>
        /// <param name="id">Unique id of the entity to be loaded from the child (lookup) list.</param>
        public LazyLoadingThunk(SharePointDataSource<T> source, int id)
        {
            this.source = source;
            this.id = id;
        }

        /// <summary>
        /// Creates a new lazy loading thunk referring to the containing list source and the id of the child entities as represented by <typeparamref name="R">R</typeparamref>. Used for LookupMulti fields.
        /// </summary>
        /// <param name="source">Source for the containing list of the lookup field. Will be used to get the child entity from. This allows for caching of loaded child entities on the level of the containing entity.</param>
        /// <param name="ids">List of unique ids of the entities to be loaded from the child (lookup) list.</param>
        public LazyLoadingThunk(SharePointDataSource<T> source, int[] ids)
        {
            this.source = source;
            this.ids = ids;
        }

        /// <summary>
        /// Loads the entity from the thunk represented by the thunk's entity id.
        /// </summary>
        /// <returns></returns>
        public object LoadEntity()
        {
            if (id != null)
                return source.GetEntityById<R>(id.Value);
            else
                return source.GetEntitiesById<R>(ids);
        }
    }
}
