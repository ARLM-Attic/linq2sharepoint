﻿/*
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
 * 0.2.2 - Introduction of EntityRef<T>; replaces lazy loading thunk functionality.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Entity reference. Used to enable deferred loading.
    /// </summary>
    /// <typeparam name="T">Type of the referenced entity.</typeparam>
    public struct EntityRef<T>
        where T : class
    {
        #region Private members

        /// <summary>
        /// List to retrieve the entity from.
        /// </summary>
        private SharePointList<T> _list;

        /// <summary>
        /// Id of the entity referenced to. Allows deferred loading.
        /// </summary>
        private int _id;

        /// <summary>
        /// Entity referenced to. Will be null if not loaded yet.
        /// </summary>
        private T _entity;

        /// <summary>
        /// Indicates whether or not the entity has been loaded already.
        /// </summary>
        private bool _loaded;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an entity reference for deferred loading.
        /// </summary>
        /// <param name="context">Context used to load the entity.</param>
        /// <param name="id">Primary key of the entity to be loaded.</param>
        internal EntityRef(SharePointDataContext context, int id)
        {
            _list = context.GetList<T>();
            _id = id;
            _entity = default(T);
            _loaded = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the referenced entity. If the entity isn't loaded already, deferred loading will be done.
        /// </summary>
        public T Entity
        {
            get
            {
                //
                // Deferred loading.
                //
                if (!_loaded)
                    this.Load();

                //
                // Return result.
                //
                return _entity;
            }

            set
            {
                _entity = value;
                _loaded = true;
            }
        }

        /// <summary>
        /// Indicates whether or not the referenced entity has been loaded already.
        /// </summary>
        public bool HasLoadedOrAssignedValue
        {
            get
            {
                return _loaded;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the entity being referred to.
        /// </summary>
        internal void Load()
        {
            _entity = _list.GetEntityById(_id);
            _loaded = true;
        }

        #endregion
    }
}
