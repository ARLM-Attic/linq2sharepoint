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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BdsSoft.SharePoint.Linq;

namespace Tests
{
    [List("People", Path = "/Lists/People", Version = 1)]
    public class People : SharePointListEntityTest
    {
        [Field("ID", FieldType.Counter, PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
            set { base.SetValue("ID", value); }
        }

        [Field("First name", FieldType.Text)]
        public string FirstName
        {
            get { return (string)base.GetValue("FirstName"); }
            set { base.SetValue("FirstName", value); }
        }

        [Field("Last name", FieldType.Text)]
        public string LastName
        {
            get { return (string)base.GetValue("LastName"); }
            set { base.SetValue("LastName", value); }
        }

        [Field("Is member", FieldType.Boolean)]
        public bool IsMember
        {
            get { return (bool)base.GetValue("IsMember"); }
            set { base.SetValue("IsMember", value); }
        }

        [Field("Short biography", FieldType.Note)]
        public string ShortBio
        {
            get { return (string)base.GetValue("ShortBio"); }
            set { base.SetValue("ShortBio", value); }
        }

        [Field("Age", FieldType.Number)]
        public double Age
        {
            get { return (double)base.GetValue("Age"); }
            set { base.SetValue("Age", value); }
        }

        [Field("SecondAge", FieldType.Number)]
        public double? SecondAge
        {
            get { return (double?)base.GetValue("SecondAge"); }
            set { base.SetValue("SecondAge", value); }
        }
    }

    [List("ChoiceTest", Path = "/Lists/ChoiceTest", Version = 1)]
    public class ChoiceTest : SharePointListEntityTest
    {
        [Field("ID", FieldType.Counter, PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
            set { base.SetValue("ID", value); }
        }

        [Field("Title", FieldType.Text)]
        public string Title
        {
            get { return (string)base.GetValue("Title"); }
            set { base.SetValue("Title", value); }
        }

        [Field("Options", FieldType.Choice)]
        public Options Options
        {
            get { return (Options)base.GetValue("Options"); }
            set { base.SetValue("Options", value); }
        }
    }

    public enum Options : uint { A, B, [Choice("C & D")]CD }

    [List("ChoiceTest2", Path = "/Lists/ChoiceTest2", Version = 1)]
    public class ChoiceTest2 : SharePointListEntityTest
    {
        [Field("ID", FieldType.Counter, PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
            set { base.SetValue("ID", value); }
        }

        [Field("Title", FieldType.Text)]
        public string Title
        {
            get { return (string)base.GetValue("Title"); }
            set { base.SetValue("Title", value); }
        }

        [Field("Options", FieldType.Choice)]
        public Options2 Options
        {
            get { return (Options2)base.GetValue("Options"); }
            set { base.SetValue("Options", value); }
        }
    }

    [Flags]
    public enum Options2 : uint { A = 1, B = 2, [Choice("C & D")] CD = 4 }

    [List("LookupParent", Path = "/Lists/LookupParent", Version = 1)]
    public class LookupParent : SharePointListEntityTest
    {
        [Field("ID", FieldType.Counter, PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
            set { base.SetValue("ID", value); }
        }

        [Field("Title", FieldType.Text)]
        public string Title
        {
            get { return (string)base.GetValue("Title"); }
            set { base.SetValue("Title", value); }
        }

        [Field("Child", FieldType.Lookup, LookupDisplayField = "Title")]
        public LookupChild Child
        {
            get { return (LookupChild)base.GetValue("Child"); }
            set { base.SetValue("Child", value); }
        }
    }

    [List("LookupChild", Path = "/Lists/LookupChild", Version = 1)]
    public class LookupChild : SharePointListEntityTest
    {
        [Field("ID", FieldType.Counter, PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
            set { base.SetValue("ID", value); }
        }

        [Field("Title", FieldType.Text)]
        public string Title
        {
            get { return (string)base.GetValue("Title"); }
            set { base.SetValue("Title", value); }
        }

        [Field("Number", FieldType.Number)]
        public double Number
        {
            get { return (double)base.GetValue("Number"); }
            set { base.SetValue("Number", value); }
        }
    }

    [List("DateTimeTest", Path = "/Lists/DateTimeTest", Version = 1)]
    public class DateTimeTest : SharePointListEntityTest
    {
        [Field("ID", FieldType.Counter, PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
            set { base.SetValue("ID", value); }
        }

        [Field("Name", FieldType.Text)]
        public string Name
        {
            get { return (string)base.GetValue("Name"); }
            set { base.SetValue("Name", value); }
        }

        [Field("DateTime", FieldType.DateTime)]
        public DateTime DateTime
        {
            get { return (DateTime)base.GetValue("DateTime"); }
            set { base.SetValue("DateTime", value); }
        }
    }
}
