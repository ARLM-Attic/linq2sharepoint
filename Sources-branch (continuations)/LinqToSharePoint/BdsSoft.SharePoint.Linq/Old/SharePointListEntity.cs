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
 * 0.2.1 - Event model.
 *         New LazyLoadingThunk implementation (see LazyLoadingThunk.cs).
 * 0.2.2 - Class deprecated.
 */

#region Namespace imports

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Base class for entities used in LINQ to SharePoint.
    /// </summary>
    [Obsolete("As of 0.2.2 this class is obsolete. The new entity model doesn't use a base class anymore.", true)]
    public class SharePointListEntity : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Private members

        /// <summary>
        /// Dictionary of fields with associated values for an entity instance.
        /// </summary>
        private Dictionary<string, object> fields;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for entities.
        /// </summary>
        public SharePointListEntity()
        {
            fields = new Dictionary<string, object>();
        }

        #endregion

        #region Accessor methods

        /// <summary>
        /// Retrieves the value from a specified field.
        /// </summary>
        /// <param name="field">Field name to get the value for.</param>
        /// <returns>Entity field value; can be null if not specified.</returns>
        /// <remarks>Calling GetValue can trigger lazy loading for lookup fields.</remarks>
        public object GetValue(string field)
        {
            if (field == null)
                throw new ArgumentNullException("field");

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
                    object entity = ll.Load();
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
            if (field == null)
                throw new ArgumentNullException("field");

            OnPropertyChanging(field);

            fields[field] = value;

            OnPropertyChanged(field);
        }

        #endregion

        #region Event model

        /// <summary>
        /// Raised before a property value is changed.
        /// </summary>
        /// <param name="propertyName">Name of the changing property.</param>
        protected void OnPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Raised after a property value has been changed.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies clients that a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Notifies clients that a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
