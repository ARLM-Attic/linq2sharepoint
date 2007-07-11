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

#endregion

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Represents a URL used in a SharePoint list. Extends a Uri with a friendly name that indicates the title of the URL.
    /// </summary>
    [Serializable]
    public class Url : Uri
    {
        #region Constructors

        /// <summary>
        /// Internal constructor for a Url field.
        /// </summary>
        /// <param name="url">Url.</param>
        /// <param name="friendlyName">Friendly name for the Url.</param>
        internal Url(string url, string friendlyName)
            : base(url)
        {
            FriendlyName = friendlyName;
        }

        #endregion

        #region Factory methods

        /// <summary>
        /// Parses a SharePoint Url field.
        /// </summary>
        /// <param name="s">SharePoint Url field value.</param>
        /// <returns>Url object representing the specified field value.</returns>
        public static Url Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            //
            // Split the specified URL string. The first part contains the URI, the second part the friendly name.
            //
            string[] ss = s.Split(',');
            if (ss.Length != 2)
                throw new ArgumentException(Errors.InvalidUrlParseArgument, "s");

            //
            // Create Url object and return.
            //
            Url url = new Url(ss[0], ss[1]);
            return url;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the friendly name for the URL.
        /// </summary>
        public string FriendlyName
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the hash code for the URL.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ((Uri)this).GetHashCode() ^ FriendlyName.GetHashCode();
        }

        /// <summary>
        /// Checks for equality with a given a Url instance.
        /// </summary>
        /// <param name="obj">Object to check for equality.</param>
        /// <returns>True if both objects represent the same URL; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            //
            // Instances compared to null aren't equal.
            //
            if (obj == null)
                return false;

            //
            // Same reference?
            //
            if (object.ReferenceEquals(this, obj))
                return true;

            //
            // Can only compare to a Url instance.
            //
            Url u = obj as Url;
            if (u == null)
                return false;

            //
            // Same friendly name and same Uri required.
            //
            return (FriendlyName == u.FriendlyName) && ((Uri)this == (Uri)obj);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Checks for equality between a Url and a string-representation of a URL. Used for LINQ queries that compare a URL field with a string containing the URL's address.
        /// </summary>
        /// <param name="url">Url to check.</param>
        /// <param name="address">URL address string representation to check.</param>
        /// <returns>True if the Url and the string refer to the same URL; false otherwise.</returns>
        public static bool operator ==(Url url, string address)
        {
            if (url == null && address == null)
                return true;
            else if (url == null || address == null)
                return false;
            else
                return url.AbsoluteUri == address;
        }

        /// <summary>
        /// Checks for non-equality between a Url and a string-representation of a URL. Used for LINQ queries that compare a URL field with a string containing the URL's address.
        /// </summary>
        /// <param name="url">Url to check.</param>
        /// <param name="address">URL address string representation to check.</param>
        /// <returns>True if the Url and the string don't refer to the same URL; false otherwise.</returns>
        public static bool operator !=(Url url, string address)
        {
            return !(url == address);
        }

        #endregion

        #region Serialization support

        /// <summary>
        /// Initializes a new instance of the Url class from the specified instances of the SerializationInfo and StreamingContext classes.
        /// </summary>
        /// <param name="info">An instance of the SerializationInfo class containing the information required to serialize the new Url instance.</param>
        /// <param name="context">An instance of the StreamingContext class containing the source of the serialized stream associated with the new Uri instance.</param>
        protected Url(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            FriendlyName = (string)info.GetValue("FriendlyName", typeof(string));
        }

        /// <summary>
        /// Adds the friendly name to the SerializationInfo object.
        /// </summary>
        /// <param name="info">The SerializationInfo to populate with data.</param>
        /// <param name="context">The destination for this serialization.</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FriendlyName", FriendlyName);
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
