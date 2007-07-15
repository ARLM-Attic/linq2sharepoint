/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Diagnostics;

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Represents a URL used in a SharePoint list. Extends a Uri with a friendly name that indicates the title of the URL.
    /// </summary>
    [Serializable]
    public sealed class UrlValue : ISerializable
    {
        #region Private members

        /// <summary>
        /// Internal storage of the URL's field value.
        /// </summary>
        private SPFieldUrlValue _urlValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Url instance based on a given SPFieldUrlValue.
        /// </summary>
        /// <param name="urlValue">SPFieldUrlValue instance representing the URL and description.</param>
        internal UrlValue(SPFieldUrlValue urlValue)
        {
            Debug.Assert(urlValue != null);

            _urlValue = urlValue;
        }

        /// <summary>
        /// Creates a new Url instance based on a given URI and a description.
        /// </summary>
        /// <param name="url">URL to refer to.</param>
        /// <param name="description">Description for the URL.</param>
        public UrlValue(string url, string description)
        {
            _urlValue = new SPFieldUrlValue();

            if (url != null)
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    throw new Exception(); //EXTODO

                _urlValue.Url = url;
                _urlValue.Description = description;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Url's description.
        /// </summary>
        public string Description
        {
            get { return _urlValue.Description; }
        }

        /// <summary>
        /// Gets the Url being referred to.
        /// </summary>
        public string Url
        {
            get { return _urlValue.Url; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a hash code for the Url instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _urlValue.GetHashCode();
        }

        /// <summary>
        /// Checks whether the given object is equal to the Url instance.
        /// </summary>
        /// <param name="obj">Object to be compared to the current instance.</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            UrlValue u = obj as UrlValue;

            if (u == null)
                return false;
            else if (object.ReferenceEquals(this, u))
                return true;
            else
                return u.Description == _urlValue.Description && u.Url == _urlValue.Url;
        }

        /// <summary>
        /// Gets a friendly string representation of the URL including the friendly name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _urlValue.ToString();
        }

        #endregion

        #region Serialization support

        /// <summary>
        /// Initializes a new instance of the Url class from the specified instances of the SerializationInfo and StreamingContext classes.
        /// </summary>
        /// <param name="info">An instance of the SerializationInfo class containing the information required to serialize the new Url instance.</param>
        /// <param name="context">An instance of the StreamingContext class containing the source of the serialized stream associated with the new Uri instance.</param>
        private UrlValue(SerializationInfo info, StreamingContext context)
        {
            _urlValue = (SPFieldUrlValue)info.GetValue("UrlValue", typeof(SPFieldUrlValue));
        }

        /// <summary>
        /// Adds the friendly name to the SerializationInfo object.
        /// </summary>
        /// <param name="info">The SerializationInfo to populate with data.</param>
        /// <param name="context">The destination for this serialization.</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UrlValue", _urlValue);
        }

        #endregion
    }
}
