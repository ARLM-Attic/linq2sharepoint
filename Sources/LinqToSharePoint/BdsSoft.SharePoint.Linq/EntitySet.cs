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
 * 0.2.2 - Introduction of EntitySet<T>; replaces lazy loading thunk functionality.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Set of entity references. Used to enable lazy loading.
    /// </summary>
    /// <typeparam name="T">Type of the referenced entities.</typeparam>
    public struct EntitySet<T> : IList<T>, ICollection<T>
        where T : class
    {
        private SharePointList<T> _list;
        private int[] _ids;
        private IList<T> _entities;
        private bool _loaded;
        private Action<T> _onAdd;
        private Action<T> _onRemove;

        /// <summary>
        /// Creates a set of entity references for lazy loading.
        /// </summary>
        /// <param name="context">Context used to load the entities.</param>
        /// <param name="ids">Primary keys of the entities to be loaded.</param>
        /// <param name="onAdd">Action to take when an entity is added to the set.</param>
        /// <param name="onRemove">Action to take when an entity is removed from the set.</param>
        internal EntitySet(SharePointDataContext context, int[] ids, Action<T> onAdd, Action<T> onRemove)
        {
            _list = context.GetList<T>();
            _ids = ids;
            _entities = new List<T>();
            _loaded = false;
            _onAdd = onAdd;
            _onRemove = onRemove;
        }

        /// <summary>
        /// Indicates whether or not the referenced entities have been loaded already.
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return _loaded;
            }
        }

        /// <summary>
        /// Loads the entity being referred to.
        /// </summary>
        internal void Load()
        {
            _entities = _list.GetEntitiesById(_ids);
            _loaded = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public void Assign(IEnumerable<T> source)
        {
            //
            // TODO
            //
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            //
            // Lazy loading.
            //
            if (!_loaded)
                this.Load();

            return _entities.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            //
            // Lazy loading.
            //
            if (!_loaded)
                this.Load();

            if (_onAdd != null)
                _onAdd(item);

            _entities.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            //
            // Lazy loading.
            //
            if (!_loaded)
                this.Load();

            if (_onRemove != null)
                _onRemove(_entities[index]);

            _entities.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                //
                // Lazy loading.
                //
                if (!_loaded)
                    this.Load();

                return _entities[index];
            }
            set
            {
                if (_onRemove != null)
                    _onRemove(_entities[index]);

                if (_onAdd != null)
                    _onAdd(value);

                _entities[index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            //
            // Lazy loading.
            //
            if (!_loaded)
                this.Load();

            if (_onAdd != null)
                _onAdd(item);

            _entities.Add(item);
        }

        public void Clear()
        {
            //
            // Lazy loading.
            //
            if (!_loaded)
                this.Load();

            if (_onRemove != null)
            {
                foreach (T item in _entities)
                    _onRemove(item);
            }

            _entities.Clear();
        }

        public bool Contains(T item)
        {
            //
            // Lazy loading.
            //
            if (!_loaded)
                this.Load();

            return _entities.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            //
            // Lazy loading.
            //
            if (!_loaded)
                this.Load();

            _entities.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                //
                // Lazy loading.
                //
                if (!_loaded)
                    this.Load();

                return _entities.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                //
                // Lazy loading.
                //
                if (!_loaded)
                    this.Load();

                return false;
            }
        }

        public bool Remove(T item)
        {
            //
            // Lazy loading.
            //
            if (!_loaded)
                this.Load();

            if (_onRemove != null)
                _onRemove(item);

            return _entities.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            //
            // Lazy loading.
            //
            if (!_loaded)
                this.Load();

            return _entities.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
