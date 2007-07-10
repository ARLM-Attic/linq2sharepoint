/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using BdsSoft.SharePoint.Linq;

namespace Demo
{
    /// <summary>
    /// Demo
    /// </summary>
    [List("Demo", Id = "34c90895-fbf3-4da7-a260-4b3ddc67146d", Version = 39, Path = "/Lists/Demo")]
    class Demo2 : SharePointListEntity
    {
        /// <summary>
        /// Title
        /// </summary>
        [Field("Title", FieldType.Text)]
        public string Title
        {
            get { return (string)GetValue("Title"); }
            set { SetValue("Title", value); }
        }

        /// <summary>
        /// Profile
        /// </summary>
        [Field("Profile", FieldType.Lookup, LookupDisplayField = "Title")]
        public Users2 Profile
        {
            get { return (Users2)GetValue("Profile"); }
            set { SetValue("Profile", value); }
        }

        /// <summary>
        /// Profile
        /// </summary>
        [Field("Profiles", FieldType.LookupMulti)]
        public Users2[] Profiles
        {
            get { return (Users2[])GetValue("Profiles"); }
            set { SetValue("Profiles", value); }
        }
    }

    /// <summary>
    /// Users
    /// </summary>
    [List("Users", Id = "e2abfbd2-f198-4fea-8a41-68eb23c8b220", Version = 12, Path = "/Lists/Users")]
    class Users2 : SharePointListEntity
    {
        /// <summary>
        /// Title
        /// </summary>
        [Field("ID", FieldType.Counter, PrimaryKey = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
        }

        /// <summary>
        /// Title
        /// </summary>
        [Field("Title", FieldType.Text)]
        public string Title
        {
            get { return (string)GetValue("Title"); }
            set { SetValue("Title", value); }
        }

        /// <summary>
        /// Age
        /// </summary>
        [Field("Age", FieldType.Number)]
        public double? Age
        {
            get { return (double?)GetValue("Age"); }
            set { SetValue("Age", value); }
        }
    }
}
