using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Junkyard
{
    /// <summary>
    /// Categories
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("Categories", Id = "2f5a24e6-43dd-4af0-b28f-dfb75c96c380", Version = 3, Path = "/Lists/Categories")]
    public partial class Categories : global::BdsSoft.SharePoint.Linq.SharePointListEntity
    {

        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public string Title
        {
            get
            {
                return ((string)(base.GetValue("Title")));
            }
            set
            {
                base.SetValue("Title", value);
            }
        }

        /// <summary>
        /// CategoryName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("CategoryName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "6a41acdd-f021-417e-9a39-2c687381de56")]
        public string CategoryName
        {
            get
            {
                return ((string)(base.GetValue("CategoryName")));
            }
            set
            {
                base.SetValue("CategoryName", value);
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Description", global::BdsSoft.SharePoint.Linq.FieldType.Note, Id = "b78064ae-3482-4e31-aa4a-bb26db5e506d")]
        public string Description
        {
            get
            {
                return ((string)(base.GetValue("Description")));
            }
            set
            {
                base.SetValue("Description", value);
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get
            {
                return ((int)(base.GetValue("ID")));
            }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContentType", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType
        {
            get
            {
                return ((string)(base.GetValue("ContentType")));
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Modified", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true)]
        public global::System.Nullable<System.DateTime> Modified
        {
            get
            {
                return ((global::System.Nullable<System.DateTime>)(base.GetValue("Modified")));
            }
        }

        /// <summary>
        /// Created
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Created", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true)]
        public global::System.Nullable<System.DateTime> Created
        {
            get
            {
                return ((global::System.Nullable<System.DateTime>)(base.GetValue("Created")));
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("_UIVersionString", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true)]
        public string Version
        {
            get
            {
                return ((string)(base.GetValue("Version")));
            }
        }
    }

    /// <summary>
    /// Suppliers
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("Suppliers", Id = "287cc088-9e8f-4c34-b3a0-e8a9d89f3f39", Version = 11, Path = "/Lists/Suppliers")]
    public partial class Suppliers : global::BdsSoft.SharePoint.Linq.SharePointListEntity
    {

        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public string Title
        {
            get
            {
                return ((string)(base.GetValue("Title")));
            }
            set
            {
                base.SetValue("Title", value);
            }
        }

        /// <summary>
        /// CompanyName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("CompanyName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "cd52757f-2870-497f-ba88-1285fab4c7e1")]
        public string CompanyName
        {
            get
            {
                return ((string)(base.GetValue("CompanyName")));
            }
            set
            {
                base.SetValue("CompanyName", value);
            }
        }

        /// <summary>
        /// ContactName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContactName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "8d821f90-796f-49f2-9278-0784d6f95ee6")]
        public string ContactName
        {
            get
            {
                return ((string)(base.GetValue("ContactName")));
            }
            set
            {
                base.SetValue("ContactName", value);
            }
        }

        /// <summary>
        /// ContactTitle
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContactTitle", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "196b85f9-10da-4f87-9332-dfb52a9603c1")]
        public string ContactTitle
        {
            get
            {
                return ((string)(base.GetValue("ContactTitle")));
            }
            set
            {
                base.SetValue("ContactTitle", value);
            }
        }

        /// <summary>
        /// Address
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Address", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "cc2a010d-e22e-498a-a85a-8d32af374d58")]
        public string Address
        {
            get
            {
                return ((string)(base.GetValue("Address")));
            }
            set
            {
                base.SetValue("Address", value);
            }
        }

        /// <summary>
        /// City
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("City", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "cc559100-d7e6-4817-8f31-ff89333e5860")]
        public string City
        {
            get
            {
                return ((string)(base.GetValue("City")));
            }
            set
            {
                base.SetValue("City", value);
            }
        }

        /// <summary>
        /// Region
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Region", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "6d0af8b5-2735-4e19-8806-dfd3eaf6aeca")]
        public string Region
        {
            get
            {
                return ((string)(base.GetValue("Region")));
            }
            set
            {
                base.SetValue("Region", value);
            }
        }

        /// <summary>
        /// PostalCode
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("PostalCode", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "3e84ea38-ae73-4387-8c91-09972eb3b7d2")]
        public string PostalCode
        {
            get
            {
                return ((string)(base.GetValue("PostalCode")));
            }
            set
            {
                base.SetValue("PostalCode", value);
            }
        }

        /// <summary>
        /// Country
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Country", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "04f6dc29-0884-42ac-99eb-aac3bb1326bd")]
        public string Country
        {
            get
            {
                return ((string)(base.GetValue("Country")));
            }
            set
            {
                base.SetValue("Country", value);
            }
        }

        /// <summary>
        /// Phone
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Phone", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "82015708-da95-45b5-b1b2-77426e4e33d9")]
        public string Phone
        {
            get
            {
                return ((string)(base.GetValue("Phone")));
            }
            set
            {
                base.SetValue("Phone", value);
            }
        }

        /// <summary>
        /// Fax
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Fax", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "61f037f4-f780-4b21-a8e3-b60cfed1c4d2")]
        public string Fax
        {
            get
            {
                return ((string)(base.GetValue("Fax")));
            }
            set
            {
                base.SetValue("Fax", value);
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get
            {
                return ((int)(base.GetValue("ID")));
            }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContentType", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType
        {
            get
            {
                return ((string)(base.GetValue("ContentType")));
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Modified", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true)]
        public global::System.Nullable<System.DateTime> Modified
        {
            get
            {
                return ((global::System.Nullable<System.DateTime>)(base.GetValue("Modified")));
            }
        }

        /// <summary>
        /// Created
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Created", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true)]
        public global::System.Nullable<System.DateTime> Created
        {
            get
            {
                return ((global::System.Nullable<System.DateTime>)(base.GetValue("Created")));
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("_UIVersionString", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true)]
        public string Version
        {
            get
            {
                return ((string)(base.GetValue("Version")));
            }
        }
    }

    /// <summary>
    /// Products
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("Products", Id = "d6be2daa-f0c3-48fe-ae0b-3d3bd6b860ef", Version = 15, Path = "/Lists/Products")]
    public partial class Products : global::BdsSoft.SharePoint.Linq.SharePointListEntity
    {

        /// <summary>
        /// Title
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Title", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public string Title
        {
            get
            {
                return ((string)(base.GetValue("Title")));
            }
            set
            {
                base.SetValue("Title", value);
            }
        }

        /// <summary>
        /// ProductName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ProductName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "38d0b85f-fa6d-4888-83c3-1a2a1a05d944")]
        public string ProductName
        {
            get
            {
                return ((string)(base.GetValue("ProductName")));
            }
            set
            {
                base.SetValue("ProductName", value);
            }
        }

        /// <summary>
        /// QuantityPerUnit
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("QuantityPerUnit", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "b418ce10-53e8-4bc5-823c-1fe199b3df70")]
        public string QuantityPerUnit
        {
            get
            {
                return ((string)(base.GetValue("QuantityPerUnit")));
            }
            set
            {
                base.SetValue("QuantityPerUnit", value);
            }
        }

        /// <summary>
        /// UnitPrice
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("UnitPrice", global::BdsSoft.SharePoint.Linq.FieldType.Currency, Id = "8daebd3b-9fbf-4858-9f05-1e88f62a0417")]
        public global::System.Nullable<double> UnitPrice
        {
            get
            {
                return ((global::System.Nullable<double>)(base.GetValue("UnitPrice")));
            }
            set
            {
                base.SetValue("UnitPrice", value);
            }
        }

        /// <summary>
        /// UnitsInStock
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("UnitsInStock", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id = "82117b89-5115-42dc-82e5-ec494cb93258")]
        public global::System.Nullable<double> UnitsInStock
        {
            get
            {
                return ((global::System.Nullable<double>)(base.GetValue("UnitsInStock")));
            }
            set
            {
                base.SetValue("UnitsInStock", value);
            }
        }

        /// <summary>
        /// UnitsOnOrder
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("UnitsOnOrder", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id = "68a03a4e-9b42-4437-a38b-33f6ed69ad4d")]
        public global::System.Nullable<double> UnitsOnOrder
        {
            get
            {
                return ((global::System.Nullable<double>)(base.GetValue("UnitsOnOrder")));
            }
            set
            {
                base.SetValue("UnitsOnOrder", value);
            }
        }

        /// <summary>
        /// ReorderLevel
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ReorderLevel", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id = "c63ef5b7-d6b6-4246-967d-a60c41018ed9")]
        public global::System.Nullable<double> ReorderLevel
        {
            get
            {
                return ((global::System.Nullable<double>)(base.GetValue("ReorderLevel")));
            }
            set
            {
                base.SetValue("ReorderLevel", value);
            }
        }

        /// <summary>
        /// Discontinued
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Discontinued", global::BdsSoft.SharePoint.Linq.FieldType.Boolean, Id = "4df879f5-7a2f-4db0-baac-7593c0fdc112")]
        public global::System.Nullable<bool> Discontinued
        {
            get
            {
                return ((global::System.Nullable<bool>)(base.GetValue("Discontinued")));
            }
            set
            {
                base.SetValue("Discontinued", value);
            }
        }

        /// <summary>
        /// Category
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Category", global::BdsSoft.SharePoint.Linq.FieldType.Lookup, Id = "081784b3-2bba-4456-a25c-2fc1f6c4d492", LookupField = "CategoryName")]
        public Categories Category
        {
            get
            {
                return ((Categories)(base.GetValue("Category")));
            }
            set
            {
                base.SetValue("Category", value);
            }
        }

        /// <summary>
        /// Supplier
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Supplier", global::BdsSoft.SharePoint.Linq.FieldType.Lookup, Id = "e6fd6cbc-e50e-486c-89c5-8fb65e5099a1", LookupField = "CompanyName")]
        public Suppliers Supplier
        {
            get
            {
                return ((Suppliers)(base.GetValue("Supplier")));
            }
            set
            {
                base.SetValue("Supplier", value);
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ID", global::BdsSoft.SharePoint.Linq.FieldType.Counter, Id = "1d22ea11-1e32-424e-89ab-9fedbadb6ce1", PrimaryKey = true, ReadOnly = true)]
        public int ID
        {
            get
            {
                return ((int)(base.GetValue("ID")));
            }
        }

        /// <summary>
        /// Content Type
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContentType", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType
        {
            get
            {
                return ((string)(base.GetValue("ContentType")));
            }
        }

        /// <summary>
        /// Modified
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Modified", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true)]
        public global::System.Nullable<System.DateTime> Modified
        {
            get
            {
                return ((global::System.Nullable<System.DateTime>)(base.GetValue("Modified")));
            }
        }

        /// <summary>
        /// Created
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Created", global::BdsSoft.SharePoint.Linq.FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true)]
        public global::System.Nullable<System.DateTime> Created
        {
            get
            {
                return ((global::System.Nullable<System.DateTime>)(base.GetValue("Created")));
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("_UIVersionString", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true)]
        public string Version
        {
            get
            {
                return ((string)(base.GetValue("Version")));
            }
        }
    }

    public partial class DemoSharePointDataContext : global::BdsSoft.SharePoint.Linq.SharePointDataContext
    {

        /// <summary>
        /// Connect to SharePoint using the SharePoint web services.
        /// </summary>
        /// <param name="wsUri">URI to the SharePoint site.</param>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public DemoSharePointDataContext(System.Uri wsUri)
            :
                base(wsUri)
        {
        }

        /// <summary>
        /// Connect to SharePoint using the SharePoint object model.
        /// </summary>
        /// <param name="site">SharePoint site object.</param>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public DemoSharePointDataContext(Microsoft.SharePoint.SPSite site)
            :
                base(site)
        {
        }

        /// <summary>
        // Products list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Products> Products
        {
            get
            {
                return this.GetList<Products>();
            }
        }

        /// <summary>
        // Categories list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Categories> Categories
        {
            get
            {
                return this.GetList<Categories>();
            }
        }

        /// <summary>
        // Suppliers list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Suppliers> Suppliers
        {
            get
            {
                return this.GetList<Suppliers>();
            }
        }
    }
}
