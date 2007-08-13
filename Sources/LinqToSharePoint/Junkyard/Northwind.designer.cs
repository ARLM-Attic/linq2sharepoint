//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1378
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Junkyard {
    using System;
    using System.Collections.Generic;
    using BdsSoft.SharePoint.Linq;
    
    
    /// <summary>
    /// Categories
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("Categories", Id="7934f5ae-ffeb-4fef-8502-1403a44625c3", Version=4, Path="/Lists/Categories")]
    public partial class Category : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging {
        
        private string _Title;
        
        private string _CategoryName;
        
        private string _Description;
        
        private int _ID = default(int);
        
        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage="_Title")]
        public string Title {
            get {
                return this._Title;
            }
            set {
                if ((this._Title != value)) {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }
        
        /// <summary>
        /// CategoryName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("CategoryName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="5f8d2c2c-a5b1-470d-9713-0f120d39fa87", Storage="_CategoryName")]
        public string CategoryName {
            get {
                return this._CategoryName;
            }
            set {
                if ((this._CategoryName != value)) {
                    this.OnPropertyChanging("CategoryName");
                    this._CategoryName = value;
                    this.OnPropertyChanged("CategoryName");
                }
            }
        }
        
        /// <summary>
        /// Description
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Description", global::BdsSoft.SharePoint.Linq.FieldType.Note, Id="ba3945f9-a12b-4b4e-b07e-305260b53bf3", Storage="_Description")]
        public string Description {
            get {
                return this._Description;
            }
            set {
                if ((this._Description != value)) {
                    this.OnPropertyChanging("Description");
                    this._Description = value;
                    this.OnPropertyChanged("Description");
                }
            }
        }
        
        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id="1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey=true, ReadOnly=true, Storage="_ID")]
        public int ID {
            get {
                return this._ID;
            }
        }
        
        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
        
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName) {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <summary>
    /// NestedSubqueries
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("NestedSubqueries", Id="2d19fcdf-8e96-4765-9974-73b7f5d41cde", Version=1, Path="/Lists/NestedSubqueries")]
    public partial class Order : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging {
        
        private string _Title;
        
        private global::BdsSoft.SharePoint.Linq.EntityRef<Product> _Product;
        
        private int _ID = default(int);
        
        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage="_Title")]
        public string Title {
            get {
                return this._Title;
            }
            set {
                if ((this._Title != value)) {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }
        
        /// <summary>
        /// Product
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Product", global::BdsSoft.SharePoint.Linq.FieldType.Lookup, Id="143d14c7-94c1-4977-a9f6-f18e9d06e013", LookupDisplayField="ProductName", Storage="_Product")]
        public Product Product {
            get {
                return this._Product.Entity;
            }
            set {
                if ((this._Product.Entity != value)) {
                    this.OnPropertyChanging("Product");
                    this._Product.Entity = value;
                    this.OnPropertyChanged("Product");
                }
            }
        }
        
        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id="1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey=true, ReadOnly=true, Storage="_ID")]
        public int ID {
            get {
                return this._ID;
            }
        }
        
        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
        
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName) {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <summary>
    /// Products
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("Products", Id="a03f5c0c-4abc-43a5-9512-8fa38579f69d", Version=16, Path="/Lists/Products")]
    public partial class Product : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging {
        
        private string _Title;
        
        private string _ProductName;
        
        private string _QuantityPerUnit;
        
        private global::System.Nullable<double> _UnitPrice;
        
        private global::System.Nullable<double> _UnitsInStock;
        
        private global::System.Nullable<double> _UnitsOnOrder;
        
        private global::System.Nullable<double> _ReorderLevel;
        
        private global::System.Nullable<bool> _Discontinued;
        
        private global::BdsSoft.SharePoint.Linq.EntityRef<Category> _Category;
        
        private global::BdsSoft.SharePoint.Linq.EntityRef<Supplier> _Supplier;
        
        private int _ID = default(int);
        
        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage="_Title")]
        public string Title {
            get {
                return this._Title;
            }
            set {
                if ((this._Title != value)) {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }
        
        /// <summary>
        /// ProductName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ProductName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="d2c0dffe-affd-45a5-ae45-7db0b3962404", Storage="_ProductName")]
        public string ProductName {
            get {
                return this._ProductName;
            }
            set {
                if ((this._ProductName != value)) {
                    this.OnPropertyChanging("ProductName");
                    this._ProductName = value;
                    this.OnPropertyChanged("ProductName");
                }
            }
        }
        
        /// <summary>
        /// QuantityPerUnit
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("QuantityPerUnit", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="66115209-169b-4bc8-b58c-079989c6587e", Storage="_QuantityPerUnit")]
        public string QuantityPerUnit {
            get {
                return this._QuantityPerUnit;
            }
            set {
                if ((this._QuantityPerUnit != value)) {
                    this.OnPropertyChanging("QuantityPerUnit");
                    this._QuantityPerUnit = value;
                    this.OnPropertyChanged("QuantityPerUnit");
                }
            }
        }
        
        /// <summary>
        /// UnitPrice
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("UnitPrice", global::BdsSoft.SharePoint.Linq.FieldType.Currency, Id="6bcff53d-73a3-45f0-bff6-906f79ee885a", Storage="_UnitPrice")]
        public global::System.Nullable<double> UnitPrice {
            get {
                return this._UnitPrice;
            }
            set {
                if ((this._UnitPrice != value)) {
                    this.OnPropertyChanging("UnitPrice");
                    this._UnitPrice = value;
                    this.OnPropertyChanged("UnitPrice");
                }
            }
        }
        
        /// <summary>
        /// UnitsInStock
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("UnitsInStock", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id="e9a1f148-f657-44b4-9220-caf31fcec947", Storage="_UnitsInStock")]
        public global::System.Nullable<double> UnitsInStock {
            get {
                return this._UnitsInStock;
            }
            set {
                if ((this._UnitsInStock != value)) {
                    this.OnPropertyChanging("UnitsInStock");
                    this._UnitsInStock = value;
                    this.OnPropertyChanged("UnitsInStock");
                }
            }
        }
        
        /// <summary>
        /// UnitsOnOrder
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("UnitsOnOrder", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id="eeab7f07-c645-415d-9fb6-776ac93a9176", Storage="_UnitsOnOrder")]
        public global::System.Nullable<double> UnitsOnOrder {
            get {
                return this._UnitsOnOrder;
            }
            set {
                if ((this._UnitsOnOrder != value)) {
                    this.OnPropertyChanging("UnitsOnOrder");
                    this._UnitsOnOrder = value;
                    this.OnPropertyChanged("UnitsOnOrder");
                }
            }
        }
        
        /// <summary>
        /// ReorderLevel
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ReorderLevel", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id="5ce9016c-9657-418c-959e-de38c5bf4b86", Storage="_ReorderLevel")]
        public global::System.Nullable<double> ReorderLevel {
            get {
                return this._ReorderLevel;
            }
            set {
                if ((this._ReorderLevel != value)) {
                    this.OnPropertyChanging("ReorderLevel");
                    this._ReorderLevel = value;
                    this.OnPropertyChanged("ReorderLevel");
                }
            }
        }
        
        /// <summary>
        /// Discontinued
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Discontinued", global::BdsSoft.SharePoint.Linq.FieldType.Boolean, Id="a379d115-ff85-43b1-a9cf-4f4bd173e94b", Storage="_Discontinued")]
        public global::System.Nullable<bool> Discontinued {
            get {
                return this._Discontinued;
            }
            set {
                if ((this._Discontinued != value)) {
                    this.OnPropertyChanging("Discontinued");
                    this._Discontinued = value;
                    this.OnPropertyChanged("Discontinued");
                }
            }
        }
        
        /// <summary>
        /// Category
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Category", global::BdsSoft.SharePoint.Linq.FieldType.Lookup, Id="1e7592e7-17a8-4a25-a041-fda2676faa0d", LookupDisplayField="CategoryName", Storage="_Category")]
        public Category Category {
            get {
                return this._Category.Entity;
            }
            set {
                if ((this._Category.Entity != value)) {
                    this.OnPropertyChanging("Category");
                    this._Category.Entity = value;
                    this.OnPropertyChanged("Category");
                }
            }
        }
        
        /// <summary>
        /// Supplier
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Supplier", global::BdsSoft.SharePoint.Linq.FieldType.Lookup, Id="01eacf59-28e5-46cc-b4b1-6cbef7cde4ea", LookupDisplayField="CompanyName", Storage="_Supplier")]
        public Supplier Supplier {
            get {
                return this._Supplier.Entity;
            }
            set {
                if ((this._Supplier.Entity != value)) {
                    this.OnPropertyChanging("Supplier");
                    this._Supplier.Entity = value;
                    this.OnPropertyChanged("Supplier");
                }
            }
        }
        
        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id="1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey=true, ReadOnly=true, Storage="_ID")]
        public int ID {
            get {
                return this._ID;
            }
        }
        
        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
        
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName) {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <summary>
    /// Suppliers
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("Suppliers", Id="8f5e11d8-dff6-476f-81e4-dd8e56c98f4c", Version=12, Path="/Lists/Suppliers")]
    public partial class Supplier : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging {
        
        private string _Title;
        
        private string _CompanyName;
        
        private string _ContactName;
        
        private string _ContactTitle;
        
        private string _Address;
        
        private string _City;
        
        private string _Region;
        
        private string _PostalCode;
        
        private string _Country;
        
        private string _Phone;
        
        private string _Fax;
        
        private int _ID = default(int);
        
        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="fa564e0f-0c70-4ab9-b863-0177e6ddd247", Storage="_Title")]
        public string Title {
            get {
                return this._Title;
            }
            set {
                if ((this._Title != value)) {
                    this.OnPropertyChanging("Title");
                    this._Title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }
        
        /// <summary>
        /// CompanyName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("CompanyName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="fbb79871-b500-4993-b937-792c80147f75", Storage="_CompanyName")]
        public string CompanyName {
            get {
                return this._CompanyName;
            }
            set {
                if ((this._CompanyName != value)) {
                    this.OnPropertyChanging("CompanyName");
                    this._CompanyName = value;
                    this.OnPropertyChanged("CompanyName");
                }
            }
        }
        
        /// <summary>
        /// ContactName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContactName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="7e25c420-e1e5-404d-bab8-b8dda39e0df2", Storage="_ContactName")]
        public string ContactName {
            get {
                return this._ContactName;
            }
            set {
                if ((this._ContactName != value)) {
                    this.OnPropertyChanging("ContactName");
                    this._ContactName = value;
                    this.OnPropertyChanged("ContactName");
                }
            }
        }
        
        /// <summary>
        /// ContactTitle
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContactTitle", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="b0b54b36-dec0-41ef-96d9-347bf2150610", Storage="_ContactTitle")]
        public string ContactTitle {
            get {
                return this._ContactTitle;
            }
            set {
                if ((this._ContactTitle != value)) {
                    this.OnPropertyChanging("ContactTitle");
                    this._ContactTitle = value;
                    this.OnPropertyChanged("ContactTitle");
                }
            }
        }
        
        /// <summary>
        /// Address
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Address", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="b784e5fb-bcd2-4ab6-9055-6f919e73d6d6", Storage="_Address")]
        public string Address {
            get {
                return this._Address;
            }
            set {
                if ((this._Address != value)) {
                    this.OnPropertyChanging("Address");
                    this._Address = value;
                    this.OnPropertyChanged("Address");
                }
            }
        }
        
        /// <summary>
        /// City
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("City", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="1190fec3-445c-47e4-b16e-e4d1583fd50a", Storage="_City")]
        public string City {
            get {
                return this._City;
            }
            set {
                if ((this._City != value)) {
                    this.OnPropertyChanging("City");
                    this._City = value;
                    this.OnPropertyChanged("City");
                }
            }
        }
        
        /// <summary>
        /// Region
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Region", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="fe8f684d-37f8-4582-970d-3997aea8e830", Storage="_Region")]
        public string Region {
            get {
                return this._Region;
            }
            set {
                if ((this._Region != value)) {
                    this.OnPropertyChanging("Region");
                    this._Region = value;
                    this.OnPropertyChanged("Region");
                }
            }
        }
        
        /// <summary>
        /// PostalCode
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("PostalCode", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="06417c71-2e72-4c04-bf81-7442f0c97cc3", Storage="_PostalCode")]
        public string PostalCode {
            get {
                return this._PostalCode;
            }
            set {
                if ((this._PostalCode != value)) {
                    this.OnPropertyChanging("PostalCode");
                    this._PostalCode = value;
                    this.OnPropertyChanged("PostalCode");
                }
            }
        }
        
        /// <summary>
        /// Country
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Country", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="3fdaf061-54fd-4937-8268-321ac680beaa", Storage="_Country")]
        public string Country {
            get {
                return this._Country;
            }
            set {
                if ((this._Country != value)) {
                    this.OnPropertyChanging("Country");
                    this._Country = value;
                    this.OnPropertyChanged("Country");
                }
            }
        }
        
        /// <summary>
        /// Phone
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Phone", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="2bd3dcaa-6aa2-492d-8aaf-01fa9812b316", Storage="_Phone")]
        public string Phone {
            get {
                return this._Phone;
            }
            set {
                if ((this._Phone != value)) {
                    this.OnPropertyChanging("Phone");
                    this._Phone = value;
                    this.OnPropertyChanged("Phone");
                }
            }
        }
        
        /// <summary>
        /// Fax
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Fax", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id="aa36b9ae-31fa-4344-821e-796bcf73fddb", Storage="_Fax")]
        public string Fax {
            get {
                return this._Fax;
            }
            set {
                if ((this._Fax != value)) {
                    this.OnPropertyChanging("Fax");
                    this._Fax = value;
                    this.OnPropertyChanged("Fax");
                }
            }
        }
        
        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id="1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey=true, ReadOnly=true, Storage="_ID")]
        public int ID {
            get {
                return this._ID;
            }
        }
        
        public event global::System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
        
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanging(string propertyName) {
            if ((this.PropertyChanging != null)) {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void OnPropertyChanged(string propertyName) {
            if ((this.PropertyChanged != null)) {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// NestedSubqueriesMulti
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("NestedSubqueriesMulti", Id = "1c6a498f-bc0c-478c-8dcd-8d9f91dc2f06", Version = 1, Path = "/Lists/NestedSubqueriesMulti")]
    public partial class NestedSubqueriesMulti : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private global::BdsSoft.SharePoint.Linq.EntitySet<Product> _Products = default(global::BdsSoft.SharePoint.Linq.EntitySet<Product>);

        private int _ID = default(int);

        private string _ContentType = default(string);

        private global::System.Nullable<System.DateTime> _Modified = default(global::System.Nullable<System.DateTime>);

        private global::System.Nullable<System.DateTime> _Created = default(global::System.Nullable<System.DateTime>);

        private string _Version = default(string);

        partial void OnTitleChanging(string value);

        partial void OnTitleChanged();


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
                    this.OnTitleChanging(value);
                    this.SendPropertyChanging("Title");
                    this._Title = value;
                    this.SendPropertyChanged("Title");
                    this.OnTitleChanged();
                }
            }
        }

        /// <summary>
        /// Products
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Products", global::BdsSoft.SharePoint.Linq.FieldType.LookupMulti, Id = "c17c03b0-c922-45e2-826e-6a737edd2554", LookupDisplayField = "Title", Storage = "_Products")]
        public global::BdsSoft.SharePoint.Linq.EntitySet<Product> Products
        {
            get
            {
                return this._Products;
            }
            set
            {
                this._Products.Assign(value);
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
        protected void SendPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new global::System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected void SendPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    public partial class NorthwindSharePointDataContext : global::BdsSoft.SharePoint.Linq.SharePointDataContext {
        
        /// <summary>
        /// Connect to SharePoint using the SharePoint web services.
        /// </summary>
        /// <param name="wsUri">URI to the SharePoint site.</param>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public NorthwindSharePointDataContext(System.Uri wsUri) : 
                base(wsUri) {
        }
        
        /// <summary>
        /// Connect to SharePoint using the SharePoint object model.
        /// </summary>
        /// <param name="site">SharePoint site object.</param>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public NorthwindSharePointDataContext(Microsoft.SharePoint.SPSite site) : 
                base(site) {
        }
        
        /// <summary>
        /// Connect to the http://wss3demo/ SharePoint site using the SharePoint web services.
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public NorthwindSharePointDataContext() : 
                base(new global::System.Uri("http://wss3demo/")) {
        }
        
        /// <summary>
        /// Categories list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Category> Categories {
            get {
                return this.GetList<Category>();
            }
        }
        
        /// <summary>
        /// NestedSubqueries list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Order> Orders {
            get {
                return this.GetList<Order>();
            }
        }
        
        /// <summary>
        /// Products list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Product> Products {
            get {
                return this.GetList<Product>();
            }
        }
        
        /// <summary>
        /// Suppliers list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Supplier> Suppliers {
            get {
                return this.GetList<Supplier>();
            }
        }

        /// <summary>
        /// NestedSubqueriesMulti list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<NestedSubqueriesMulti> NestedSubqueriesMulti
        {
            get
            {
                return this.GetList<NestedSubqueriesMulti>();
            }
        }
    }
}
