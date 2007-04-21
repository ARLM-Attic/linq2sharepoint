using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Represents a URL used in a SharePoint list. Extends a Uri with a friendly name that indicates the title of the URL.
    /// </summary>
    [Serializable]
    public class Url : Uri
    {
        internal Url(string s)
            : base(s.Split(',')[0])
        {
            string[] ss = s.Split(',');
            name = ss[1];
        }

        private string name;

        /// <summary>
        /// Gets or sets the friendly name for the URL.
        /// </summary>
        public string FriendlyName
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Returns the hash code for the URL.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ((Uri)this).GetHashCode() ^ name.GetHashCode();
        }

        /// <summary>
        /// Checks for equality with a given a Url instance.
        /// </summary>
        /// <param name="obj">Object to check for equality.</param>
        /// <returns>True if both objects represent the same URL; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (object.ReferenceEquals(this, obj))
                return true;

            Url u = obj as Url;
            if (u == null)
                return false;

            return (name == u.name) && ((Uri)this == (Uri)obj);
        }

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

        /// <summary>
        /// Initializes a new instance of the Url class from the specified instances of the SerializationInfo and StreamingContext classes.
        /// </summary>
        /// <param name="info">An instance of the SerializationInfo class containing the information required to serialize the new Url instance.</param>
        /// <param name="context">An instance of the StreamingContext class containing the source of the serialized stream associated with the new Uri instance.</param>
        protected Url(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            name = (string)info.GetValue("FriendlyName", typeof(string));
        }

        /// <summary>
        /// Adds the friendly name to the SerializationInfo object.
        /// </summary>
        /// <param name="info">The SerializationInfo to populate with data.</param>
        /// <param name="context">The destination for this serialization.</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public new virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("FriendlyName", name);
            base.GetObjectData(info, context);
        }
    }
}
