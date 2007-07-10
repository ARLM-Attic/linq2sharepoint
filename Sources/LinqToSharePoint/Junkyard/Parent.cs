using System;
using BdsSoft.SharePoint.Linq;

namespace Junkyard
{
    /// <summary>
    /// Parent
    /// </summary>
    [List("Parent", Id = "04eb5131-db8c-4735-b177-2ab542f2e607", Version = 2, Path = "/Lists/Parent")]
    class Parent : SharePointListEntity
    {
        /// <summary>
        /// Title
        /// </summary>
        [Field("Title", FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public string Title
        {
            get { return (string)GetValue("Title"); }
            set { SetValue("Title", value); }
        }

        /// <summary>
        /// Bar
        /// </summary>
        [Field("Bar", FieldType.Lookup, Id = "14963ed3-9d3c-48e3-903d-e3faf6a3d291", LookupDisplayField = "Title")]
        public Bar Bar
        {
            get { return (Bar)GetValue("Bar"); }
            set { SetValue("Bar", value); }
        }

        /// <summary>
        /// Foo
        /// </summary>
        [Field("Foo", FieldType.Lookup, Id = "242a42ca-8066-4e74-b5b6-9f79c611bd47", LookupDisplayField = "Title")]
        public Foo Foo
        {
            get { return (Foo)GetValue("Foo"); }
            set { SetValue("Foo", value); }
        }

        /// <summary>
        /// ID
        /// </summary>
        [Field("ID", FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [Field("ContentType", FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType
        {
            get { return (string)GetValue("ContentType"); }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [Field("Modified", FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true)]
        public System.DateTime? Modified
        {
            get { return (System.DateTime?)GetValue("Modified"); }
        }

        /// <summary>
        /// Created
        /// </summary>
        [Field("Created", FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true)]
        public System.DateTime? Created
        {
            get { return (System.DateTime?)GetValue("Created"); }
        }

        /// <summary>
        /// Version
        /// </summary>
        [Field("_UIVersionString", FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true)]
        public string Version
        {
            get { return (string)GetValue("Version"); }
        }
    }

    /// <summary>
    /// Bar
    /// </summary>
    [List("Bar", Id = "7d8fdcb9-ca35-4875-b0b6-700442546e91", Version = 0, Path = "/Lists/Bar")]
    class Bar : SharePointListEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Field("ID", FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [Field("ContentType", FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType
        {
            get { return (string)GetValue("ContentType"); }
        }

        /// <summary>
        /// Title
        /// </summary>
        [Field("Title", FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public string Title
        {
            get { return (string)GetValue("Title"); }
            set { SetValue("Title", value); }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [Field("Modified", FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true)]
        public System.DateTime? Modified
        {
            get { return (System.DateTime?)GetValue("Modified"); }
        }

        /// <summary>
        /// Created
        /// </summary>
        [Field("Created", FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true)]
        public System.DateTime? Created
        {
            get { return (System.DateTime?)GetValue("Created"); }
        }

        /// <summary>
        /// Version
        /// </summary>
        [Field("_UIVersionString", FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true)]
        public string Version
        {
            get { return (string)GetValue("Version"); }
        }
    }

    /// <summary>
    /// Foo
    /// </summary>
    [List("Foo", Id = "05a4d23b-7ea3-4fce-a3f8-7f41164dfe41", Version = 0, Path = "/Lists/Foo")]
    class Foo : SharePointListEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Field("ID", FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get { return (int)GetValue("ID"); }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [Field("ContentType", FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType
        {
            get { return (string)GetValue("ContentType"); }
        }

        /// <summary>
        /// Title
        /// </summary>
        [Field("Title", FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public string Title
        {
            get { return (string)GetValue("Title"); }
            set { SetValue("Title", value); }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [Field("Modified", FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true)]
        public System.DateTime? Modified
        {
            get { return (System.DateTime?)GetValue("Modified"); }
        }

        /// <summary>
        /// Created
        /// </summary>
        [Field("Created", FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true)]
        public System.DateTime? Created
        {
            get { return (System.DateTime?)GetValue("Created"); }
        }

        /// <summary>
        /// Version
        /// </summary>
        [Field("_UIVersionString", FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true)]
        public string Version
        {
            get { return (string)GetValue("Version"); }
        }
    }
}