using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SharePoint;
using System.Data.Linq;
using System.Reflection;
using System.Collections.Specialized;

namespace SharePointNorthwindSample
{
    class Program
    {
        static string dsn = "Data Source=.;Initial Catalog=Northwind;Integrated Security=True";
        static string url = "http://wss3demo";

        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            using (SPSite site = new SPSite(url))
            {
                using (NorthwindDataContext ctx = new NorthwindDataContext(dsn))
                {
                    SPList lstCategories = CreateCategoryList(site.RootWeb);
                    SPList lstSuppliers = CreateSupplierList(site.RootWeb);
                    SPList lstProducts = CreateProductList(site.RootWeb, lstCategories, lstSuppliers);

                    var categories = from c in ctx.Categories select c;
                    var suppliers = from s in ctx.Suppliers select s;
                    var products = from p in ctx.Products select p;

                    //string cat = "new Category() { ID = {0}, CategoryName = {1}, Description = {2} },\n";
                    //StringBuilder cats = new StringBuilder();
                    foreach (var c in categories)
                    {
                        SPListItem cat = lstCategories.Items.Add();
                        cat["CategoryName"] = c.CategoryName;
                        cat["Description"] = c.Description;
                        cat.Update();

                        //cats.AppendFormat(
                        //    cat,
                        //    c.CategoryID, 
                        //    c.CategoryName == null ? "null" : "\"" + c.CategoryName + "\"",
                        //    c.Description == null ? "null" : "\"" + c.Description + "\""
                        //);
                    }

                    //string sup = "new Supplier() { ID = {0}, CompanyName = {1}, ContactTitle = {2}, ContactName = {3}, Address = {4}, City = {5}, Country = {6} },\n";
                    //StringBuilder sups = new StringBuilder();
                    foreach (var s in suppliers)
                    {
                        SPListItem sup = lstSuppliers.Items.Add();
                        sup["CompanyName"] = s.CompanyName;
                        sup["ContactTitle"] = s.ContactTitle;
                        sup["ContactName"] = s.ContactName;
                        sup["Address"] = s.Address;
                        sup["City"] = s.City;
                        sup["Country"] = s.Country;
                        sup["Fax"] = s.Fax;
                        sup["Phone"] = s.Phone;
                        sup["PostalCode"] = s.PostalCode;
                        sup["Region"] = s.Region;
                        sup.Update();
                    }

                    foreach (var p in products)
                    {
                        SPListItem prod = lstProducts.Items.Add();
                        prod["ProductName"] = p.ProductName;
                        prod["Discontinued"] = p.Discontinued;
                        prod["QuantityPerUnit"] = p.QuantityPerUnit;
                        prod["ReorderLevel"] = p.ReorderLevel;
                        prod["UnitPrice"] = p.UnitPrice;
                        prod["UnitsInStock"] = p.UnitsInStock;
                        prod["UnitsOnOrder"] = p.UnitsOnOrder;
                        prod["Supplier"] = p.SupplierID;
                        prod["Category"] = p.CategoryID;
                        prod.Update();
                    }
                }
            }
        }

        static SPList CreateCategoryList(SPWeb web)
        {
            SPList list = CreateList(web, "Categories");

            list.Fields.Add("CategoryName", SPFieldType.Text, true);
            list.Fields.Add("Description", SPFieldType.Note, false);

            list.Update();

            StringCollection fields = new StringCollection();
            fields.Add("CategoryName");
            fields.Add("Description");

            SPView view = list.Views.Add("My view", fields, list.DefaultView.Query, list.DefaultView.RowLimit, list.DefaultView.Paged, true);
            view.Update();

            return list;
        }

        static SPList CreateSupplierList(SPWeb web)
        {
            SPList list = CreateList(web, "Suppliers");

            list.Fields.Add("CompanyName", SPFieldType.Text, true);
            list.Fields.Add("ContactName", SPFieldType.Text, false);
            list.Fields.Add("ContactTitle", SPFieldType.Text, false);
            list.Fields.Add("Address", SPFieldType.Text, false);
            list.Fields.Add("City", SPFieldType.Text, false);
            list.Fields.Add("Region", SPFieldType.Text, false);
            list.Fields.Add("PostalCode", SPFieldType.Text, false);
            list.Fields.Add("Country", SPFieldType.Text, false);
            list.Fields.Add("Phone", SPFieldType.Text, false);
            list.Fields.Add("Fax", SPFieldType.Text, false);

            list.Update();

            StringCollection fields = new StringCollection();
            fields.Add("CompanyName");
            fields.Add("ContactName");
            fields.Add("ContactTitle");
            fields.Add("Address");
            fields.Add("City");
            fields.Add("Region");
            fields.Add("PostalCode");
            fields.Add("Country");
            fields.Add("Phone");
            fields.Add("Fax");

            SPView view = list.Views.Add("My view", fields, list.DefaultView.Query, list.DefaultView.RowLimit, list.DefaultView.Paged, true);
            view.Update();

            return list;
        }

        static SPList CreateProductList(SPWeb web, SPList lstCategories, SPList lstSuppliers)
        {
            SPList list = CreateList(web, "Products");

            list.Fields.Add("ProductName", SPFieldType.Text, true);
            list.Fields.Add("QuantityPerUnit", SPFieldType.Text, false);
            list.Fields.Add("UnitPrice", SPFieldType.Currency, false);
            list.Fields.Add("UnitsInStock", SPFieldType.Number, false);
            list.Fields.Add("UnitsOnOrder", SPFieldType.Number, false);
            list.Fields.Add("ReorderLevel", SPFieldType.Number, false);
            list.Fields.Add("Discontinued", SPFieldType.Boolean, false);
            list.Fields.AddLookup("Category", lstCategories.ID, false);
            list.Fields.AddLookup("Supplier", lstSuppliers.ID, false);

            list.Update();

            SPFieldLookup category = new SPFieldLookup(list.Fields, "Category");
            category.LookupField = "CategoryName";
            category.Update();

            SPFieldLookup supplier = new SPFieldLookup(list.Fields, "Supplier");
            supplier.LookupField = "CompanyName";
            supplier.Update();

            list.Update();

            StringCollection fields = new StringCollection();
            fields.Add("ProductName");
            fields.Add("QuantityPerUnit");
            fields.Add("UnitPrice");
            fields.Add("UnitsInStock");
            fields.Add("UnitsOnOrder");
            fields.Add("ReorderLevel");
            fields.Add("Discontinued");
            fields.Add("Category");
            fields.Add("Supplier");

            SPView view = list.Views.Add("My view", fields, list.DefaultView.Query, list.DefaultView.RowLimit, list.DefaultView.Paged, true);
            view.Update();

            return list;
        }

        static SPList CreateList(SPWeb web, string listName)
        {
            SPList list;
            try
            {
                list = web.Lists[listName];
                if (list != null)
                    list.Delete();
            }
            catch { }

            Guid g = web.Lists.Add(listName, "", SPListTemplateType.GenericList);
            list = web.Lists[g];

            return list;
        }

        /*
        static SPList CreateList<T>(SPWeb web)
        {
            Type t = typeof(T);
            TableAttribute ta = (TableAttribute)t.GetCustomAttributes(typeof(TableAttribute), false)[0];

            string listName = ta.Name;
            
            SPList list;
            try
            {
                list = web.Lists[listName];
                if (list != null)
                    list.Delete();
            }
            catch {}
            
            Guid g = web.Lists.Add(listName, "", SPListTemplateType.GenericList);
            list = web.Lists[g];

            foreach (PropertyInfo prop in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
            }

            return list;
        }
         */
    }
}
