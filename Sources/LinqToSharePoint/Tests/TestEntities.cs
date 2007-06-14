using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BdsSoft.SharePoint.Linq;

namespace Tests
{
    [List("People", Path = "/Lists/People", Version = 1)]
    class People : SharePointListEntityTest
    {
        [Field("ID", FieldType.Counter, PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
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
    }
}
