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
    /*
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
     */

    /// <summary>
    /// People
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("People", Id = "6be9949c-108b-4507-b48e-8e89cd71135c", Version = 7, Path = "/Lists/People")]
    public partial class People : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private string _FirstName;

        private string _LastName;

        private bool _IsMember;

        private string _ShortBiography;

        private double _Age;

        private global::System.Nullable<double> _SecondAge;

        private int _ID;

        private string _ContentType;

        private global::System.Nullable<System.DateTime> _Modified;

        private global::System.Nullable<System.DateTime> _Created;

        private string _Version;

        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage = "_Title")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// First name
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("First name", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "f4011cef-5f4c-48b3-b7ac-cd0160468cf2", Storage = "_FirstName")]
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                if ((this._FirstName != value))
                {
                    this.OnPropertyChanging("FirstName");
                    this._FirstName = value;
                    this.OnPropertyChanged("FirstName");
                }
            }
        }

        /// <summary>
        /// Last name
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Last name", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "ed186bd7-3c0f-4cf2-be97-ec9bbfba416c", Storage = "_LastName")]
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                if ((this._LastName != value))
                {
                    this.OnPropertyChanging("LastName");
                    this._LastName = value;
                    this.OnPropertyChanged("LastName");
                }
            }
        }

        /// <summary>
        /// Is member
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Is member", global::BdsSoft.SharePoint.Linq.FieldType.Boolean, Id = "9aea0cb7-cd44-4362-9d32-e11594706f87", Storage = "_IsMember")]
        public bool IsMember
        {
            get
            {
                return this._IsMember;
            }
            set
            {
                if ((this._IsMember != value))
                {
                    this.OnPropertyChanging("IsMember");
                    this._IsMember = value;
                    this.OnPropertyChanged("IsMember");
                }
            }
        }

        /// <summary>
        /// Short biography
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Short biography", global::BdsSoft.SharePoint.Linq.FieldType.Note, Id = "2b73bd3f-6ead-4c90-92c0-d1bac21e7c35", Storage = "_ShortBiography")]
        public string ShortBiography
        {
            get
            {
                return this._ShortBiography;
            }
            set
            {
                if ((this._ShortBiography != value))
                {
                    this.OnPropertyChanging("ShortBiography");
                    this._ShortBiography = value;
                    this.OnPropertyChanged("ShortBiography");
                }
            }
        }

        /// <summary>
        /// Age
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Age", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id = "b53cd8de-8e0e-4a9d-9437-f2da83d9f214", Storage = "_Age")]
        public double Age
        {
            get
            {
                return this._Age;
            }
            set
            {
                if ((this._Age != value))
                {
                    this.OnPropertyChanging("Age");
                    this._Age = value;
                    this.OnPropertyChanged("Age");
                }
            }
        }

        /// <summary>
        /// SecondAge
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("SecondAge", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id = "dcbe23da-fe80-43cd-bbab-ad624dd22343", Storage = "_SecondAge")]
        public global::System.Nullable<double> SecondAge
        {
            get
            {
                return this._SecondAge;
            }
            set
            {
                if ((this._SecondAge != value))
                {
                    this.OnPropertyChanging("SecondAge");
                    this._SecondAge = value;
                    this.OnPropertyChanged("SecondAge");
                }
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true, Storage = "_ID")]
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContentType", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true, Storage = "_ContentType")]
        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Modified", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true, Storage = "_Modified")]
        public global::System.Nullable<System.DateTime> Modified
        {
            get
            {
                return this._Modified;
            }
        }

        /// <summary>
        /// Created
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Created", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true, Storage = "_Created")]
        public global::System.Nullable<System.DateTime> Created
        {
            get
            {
                return this._Created;
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("_UIVersionString", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true, Storage = "_Version")]
        public string Version
        {
            get
            {
                return this._Version;
            }
        }

        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public enum Options : uint
    {

        A,

        B,

        [global::BdsSoft.SharePoint.Linq.ChoiceAttribute("C & D")]
        CD,
    }

    /// <summary>
    /// ChoiceTest
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("ChoiceTest", Id = "8bc207dc-0813-4a57-8656-7a4031f1157a", Version = 6, Path = "/Lists/ChoiceTest")]
    public partial class ChoiceTest : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private Options _Options;

        private int _ID;

        private string _ContentType;

        private global::System.Nullable<System.DateTime> _Modified;

        private global::System.Nullable<System.DateTime> _Created;

        private string _Version;

        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage = "_Title")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Options
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Options", global::BdsSoft.SharePoint.Linq.FieldType.Choice, Id = "116fe1eb-a0ff-4e87-9a39-b522912de9f7", Storage = "_Options")]
        public Options Options
        {
            get
            {
                return this._Options;
            }
            set
            {
                if ((this._Options != value))
                {
                    this.OnPropertyChanging("Options");
                    this._Options = value;
                    this.OnPropertyChanged("Options");
                }
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true, Storage = "_ID")]
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContentType", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true, Storage = "_ContentType")]
        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Modified", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true, Storage = "_Modified")]
        public global::System.Nullable<System.DateTime> Modified
        {
            get
            {
                return this._Modified;
            }
        }

        /// <summary>
        /// Created
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Created", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true, Storage = "_Created")]
        public global::System.Nullable<System.DateTime> Created
        {
            get
            {
                return this._Created;
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("_UIVersionString", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true, Storage = "_Version")]
        public string Version
        {
            get
            {
                return this._Version;
            }
        }

        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [Flags()]
    public enum Options2 : uint
    {

        A = 1,

        B = 2,

        [global::BdsSoft.SharePoint.Linq.ChoiceAttribute("C & D")]
        CD = 4,
    }

    /// <summary>
    /// ChoiceTest2
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("ChoiceTest2", Id = "c780c9dd-f1bb-42ab-bc52-55743d55d0d4", Version = 6, Path = "/Lists/ChoiceTest2")]
    public partial class ChoiceTest2 : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private Options2 _Options;

        private int _ID;

        private string _ContentType;

        private global::System.Nullable<System.DateTime> _Modified;

        private global::System.Nullable<System.DateTime> _Created;

        private string _Version;

        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage = "_Title")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Options
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Options", global::BdsSoft.SharePoint.Linq.FieldType.MultiChoice, Id = "d98fa087-850b-420e-9b4d-9259a59578e6", Storage = "_Options")]
        public Options2 Options
        {
            get
            {
                return this._Options;
            }
            set
            {
                if ((this._Options != value))
                {
                    this.OnPropertyChanging("Options");
                    this._Options = value;
                    this.OnPropertyChanged("Options");
                }
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true, Storage = "_ID")]
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContentType", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true, Storage = "_ContentType")]
        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Modified", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true, Storage = "_Modified")]
        public global::System.Nullable<System.DateTime> Modified
        {
            get
            {
                return this._Modified;
            }
        }

        /// <summary>
        /// Created
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Created", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true, Storage = "_Created")]
        public global::System.Nullable<System.DateTime> Created
        {
            get
            {
                return this._Created;
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("_UIVersionString", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true, Storage = "_Version")]
        public string Version
        {
            get
            {
                return this._Version;
            }
        }

        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// LookupChild
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("LookupChild", Id = "7c874d71-aabf-44a3-a751-ae4c981265f5", Version = 5, Path = "/Lists/LookupChild")]
    public partial class LookupChild : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private double _Number;

        private int _ID;

        private string _ContentType;

        private global::System.Nullable<System.DateTime> _Modified;

        private global::System.Nullable<System.DateTime> _Created;

        private string _Version;

        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage = "_Title")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Number
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Number", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id = "10e4b57e-5813-45f4-8ab6-e6b463c8d8b1", Storage = "_Number")]
        public double Number
        {
            get
            {
                return this._Number;
            }
            set
            {
                if ((this._Number != value))
                {
                    this.OnPropertyChanging("Number");
                    this._Number = value;
                    this.OnPropertyChanged("Number");
                }
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true, Storage = "_ID")]
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContentType", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true, Storage = "_ContentType")]
        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Modified", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true, Storage = "_Modified")]
        public global::System.Nullable<System.DateTime> Modified
        {
            get
            {
                return this._Modified;
            }
        }

        /// <summary>
        /// Created
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Created", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true, Storage = "_Created")]
        public global::System.Nullable<System.DateTime> Created
        {
            get
            {
                return this._Created;
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("_UIVersionString", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true, Storage = "_Version")]
        public string Version
        {
            get
            {
                return this._Version;
            }
        }

        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// LookupParent
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("LookupParent", Id = "f5f9f21c-8d44-45db-8280-d59b2ec3928c", Version = 6, Path = "/Lists/LookupParent")]
    public partial class LookupParent : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private global::BdsSoft.SharePoint.Linq.EntityRef<LookupChild> _Child;

        private int _ID;

        private string _ContentType;

        private global::System.Nullable<System.DateTime> _Modified;

        private global::System.Nullable<System.DateTime> _Created;

        private string _Version;

        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage = "_Title")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Child
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Child", global::BdsSoft.SharePoint.Linq.FieldType.Lookup, Id = "9cf5b500-8c5b-4e13-ae17-e0b2e057ddc2", LookupDisplayField = "Title", Storage = "_Child")]
        public LookupChild Child
        {
            get
            {
                return this._Child.Entity;
            }
            set
            {
                if ((this._Child.Entity != value))
                {
                    this.OnPropertyChanging("Child");
                    this._Child.Entity = value;
                    this.OnPropertyChanged("Child");
                }
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true, Storage = "_ID")]
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContentType", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true, Storage = "_ContentType")]
        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Modified", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true, Storage = "_Modified")]
        public global::System.Nullable<System.DateTime> Modified
        {
            get
            {
                return this._Modified;
            }
        }

        /// <summary>
        /// Created
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Created", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true, Storage = "_Created")]
        public global::System.Nullable<System.DateTime> Created
        {
            get
            {
                return this._Created;
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("_UIVersionString", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true, Storage = "_Version")]
        public string Version
        {
            get
            {
                return this._Version;
            }
        }

        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// DateTimeTest
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("DateTimeTest", Id = "1b8fc768-fd70-4322-a184-6f3a95725011", Version = 3, Path = "/Lists/DateTimeTest")]
    public partial class DateTimeTest : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private string _Name;

        private global::System.DateTime _DateTime;

        private int _ID;

        private string _ContentType;

        private global::System.Nullable<System.DateTime> _Modified;

        private global::System.Nullable<System.DateTime> _Created;

        private string _Version;

        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage = "_Title")]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Name
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Name", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "e8f853ee-2d1a-4b90-ad7f-fe3404c666d5", Storage = "_Name")]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnPropertyChanging("Name");
                    this._Name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// DateTime
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("DateTime", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "cd2825a1-1822-4ae0-9dda-21f9578299b8", Storage = "_DateTime")]
        public global::System.DateTime DateTime
        {
            get
            {
                return this._DateTime;
            }
            set
            {
                if ((this._DateTime != value))
                {
                    this.OnPropertyChanging("DateTime");
                    this._DateTime = value;
                    this.OnPropertyChanged("DateTime");
                }
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true, Storage = "_ID")]
        public int ID
        {
            get
            {
                return this._ID;
            }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContentType", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true, Storage = "_ContentType")]
        public string ContentType
        {
            get
            {
                return this._ContentType;
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Modified", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true, Storage = "_Modified")]
        public global::System.Nullable<System.DateTime> Modified
        {
            get
            {
                return this._Modified;
            }
        }

        /// <summary>
        /// Created
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Created", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true, Storage = "_Created")]
        public global::System.Nullable<System.DateTime> Created
        {
            get
            {
                return this._Created;
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("_UIVersionString", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true, Storage = "_Version")]
        public string Version
        {
            get
            {
                return this._Version;
            }
        }

        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
