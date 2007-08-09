using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;
using BdsSoft.SharePoint.Linq;
using Microsoft.SharePoint;
using System.Data.Linq.SqlClient;
using System.Reflection;
using BdsSoft.SharePoint.Linq.Tools.DebuggerVisualizer;

namespace Junkyard
{
    class Program_old
    {
        static void Main(string[] args)
        {
            //Parser warnings

            NorthwindSharePointDataContext ctx = new NorthwindSharePointDataContext();
            ctx.CheckListVersion = false;

            //var res = (from p in ctx.Products select p).Take(10).Where(p => p.UnitPrice > 100).Select(p => p.ProductName);
            //foreach (var p in res)
            //    ;

            var res = (from l in ctx.LookupMultiTest select l).AsEnumerable().SingleOrDefault();
            //EntitySet<Categories> cats = res.Categories;
            bool b = res.Categories.IsLoaded;
            if (!b)
            {
                Category c = res.Categories[0];
            }
            Console.WriteLine(res.Categories.IsLoaded);
            string s = "";

            //var res1 = from p in ctx.Products group p by p.Category; //entity property; no traversals (lookup -> warning)
            //var res2 = from p in ctx.Products group p by p.Category into g select g;
            //var res3 = (from p in ctx.Suppliers group p by p.Country into g select g.Key);//.Take(1);
            //var res = from p in ctx.Suppliers select new { Country = p.Country, Name = p.CompanyName } into s group s by s.Country into g select g.Key;
            //var res3 = (from p in ctx.Suppliers group p by p.Country into g select g.Key);//.Take(1);
            //var res = ctx.Products.Where((Product p, int i) => i == 0).Select((Product p, int i) => p);//.First(p => p.Discontinued.Value);
            //var res3 = (from p in ctx.Suppliers 
            //           select new { Name = p.CompanyName, p.Country, p.City, Contact = p.ContactName } into s select s)
            //           .Where(s => s.Name == "Bart").OrderBy(s => s.City);


            /*
               var res3 = from s in (
                                  from p in ctx.Suppliers
                                  select new { 
                                      Supplier = p, 
                                      Name = p.CompanyName, 
                                      p.Country, 
                                      Foo = p.ID * 2,
                                      Bar = new { Supplier = p, p.Fax, p.Phone } }
                                 )
                       where s.Name == "Bart"
                       orderby s.Country descending
                       select s;

            var res3 =
                from t in
                    (
                    from s in
                        (
                            from p in ctx.Products
                            select new
                            {
                                Product = p, //E (can lift)
                                Price = p.UnitPrice, //P
                                Category = p.Category, //P (can lift)
                                Supplier = p.Supplier.CompanyName, //L
                                Stock = new
                                {
                                    InStock = p.UnitsInStock, //P
                                    Ordered = p.UnitsOnOrder //P
                                }
                            })
                    select new
                    {
                        Ent = s.Product, //E
                        Prop1 = s.Price, //P
                        Prop2 = s.Product.UnitPrice, //P (lifted)
                        Lookups = new
                        {
                            Lookup1 = s.Category.CategoryName, //L (lifted)
                            Lookup2 = s.Product.Supplier.City, //L (lifted²)
                            Lookup3 = s.Supplier, //L
                            Lookup4 = s.Supplier
                        }, //L
                        Stock1 = s.Stock, //Deep copy
                        Stock2 = s.Stock, //Deep copy
                        Child = s //Merge
                    }
                        )
                select new
                {
                    //t.Child.Stock,
                    //t.Child.Category, //P
                    //t.Child.Product, //reference to another anonymous type
                    t.Prop1//,
                    //t.Lookups
                };

            //.Where(s => s.Name == "Bart").OrderBy(s => s.City);

            SharePointListQueryVisualizer.TestShowVisualizer(res3);

            foreach (var p in res3)
            {
                Console.WriteLine(p.Prop1);
                //Console.WriteLine(p.Supplier.CompanyName);
            }

            var res = from p in ctx.Products orderby p.UnitPrice.Value descending select p;

            var res1 = from s in ctx.Suppliers group s by s.Country into g select g;
            foreach (var p in res1)
            {
                Console.WriteLine(p.Key ?? "(null)");
                foreach (var s in p)
                    Console.WriteLine("- " + s.CompanyName);
                Console.WriteLine();
            }

            var res2 = from p in ctx.Products group p by p.Category;
            foreach (var p in res2)
            {
                Console.WriteLine(p.Key.CategoryName);
                foreach (var s in p)
                    Console.WriteLine("- " + s.ProductName);
                Console.WriteLine();
            }

            //var product = ctx.Products.Where(p => p.UnitsInStock > 0).First(p => p.Discontinued.Value);
            //SharePointDataContext ctx = new SharePointDataContext(new Uri("http://wss3demo"));

            //var lst = new SharePointList<UrlTest>(ctx);
            //UrlValue url = new UrlValue("http://www.bartdesmet.net", "Bart's homepage");
            //var res = from u in lst where u.Homepage.Url != url.Url select u; //OK
            //var res = from u in lst where u.Homepage == url select u; //OK (just compare by Url value)
            //var res = from u in lst where u.Homepage == new UrlValue(null, null) select u; //OK
            //var res = from u in lst where u.Homepage == null select u; //OK
            //var res = from u in lst where u.Homepage.Url.Equals(url.Url) select u; //OK
            //var res = from u in lst where u.Homepage.Url.Contains(url.Url) select u; //OK
            //var res = from u in lst where u.Homepage.Url.StartsWith(url.Url) select u; //OK

            //var res = from u in lst where u.Homepage.Description.Equals(url.Url) select u;
            //var res = from u in lst where u.Homepage.Description == "Test" select u;
            //SharePointListQueryVisualizer.TestShowVisualizer(res);
            */
            /*
            int fkey = 1;

            Type t = typeof(EntityRef<>).MakeGenericType(typeof(Junk));
            ConstructorInfo c = t.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(SharePointDataContext), typeof(int) }, null);
            object o = c.Invoke(new object[] { ctx, fkey });
            foreach (ConstructorInfo ci in t.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
            {
            }
            Activator.CreateInstance(t, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { ctx, fkey }, null);
            */

            /*
            var src = new SharePointDataContext(new Uri("http://wss3demo"));
            src.CheckListVersion = false;

            bool b = true;
            int i = 0;
            var res = from t in src.GetList<Test>() where t.IsMember && b || t.LastName == "De Smet" || i / 2 == 0 select t;
            SharePointListQueryVisualizer.TestShowVisualizer(res);
             */

            //var res = from t in src.GetList<Test>() where t.Age >= 24 orderby t.FirstName select new { t.Age, t.FirstName };
            //var res = (from t in src.GetList<Test>() where t.Age >= 24 orderby t.FirstName select t).First();
            //var res = (from t in src.GetList<Test>() where t.Age >= 24 orderby t.FirstName select t).Select(t => t).Select(t => t.Age).Select(t => 2 * t);

            //var p1 = (from t in src.GetList<Test>() where t.FirstName == "Jo" select t).AsEnumerable().Single();
            //var p2 = (from t in src.GetList<Test>() where t.FirstName == "Jo" select t).AsEnumerable().Single();
            //bool r = object.ReferenceEquals(p1, p2);

            //foreach (var t in res)
            //{
            //}

            //var res = from t in src.GetList<Test>() orderby t.FirstName.ToString() select t;
            //var res = from t in src.GetList<Test>() where t.FirstName.ToString() == "Bart".ToString() select t;
            //var res = from t in src.GetList<Test>() where t.FirstName.ToString() == t.LastName.ToString() select t;
            //var res = from t in src.GetList<Test>() where t.FirstName.ToString().Contains("B".ToString()) select t;
            //var res = from t in src.GetList<Test>() where "Bart".ToString().Contains("B".ToString()) select t;
            //SharePointListQueryVisualizer.TestShowVisualizer(res);

            //var cat = (from c in src.GetList<Categories>() where c.CategoryName == "Beverages" select c).First();

            //var res = from t in src.GetList<Products>() where t.Category.CategoryName == "Beverages" select t;
            //var res = from t in src.GetList<Products>() where t.Category == cat select t;

            /*
            var res = from p in src.GetList<Products>()
                      where p.Category.CategoryName.StartsWith("Con")
                            && (p.Supplier.Country == "USA" && p.Supplier.Region == "LA"
                                || p.Supplier.Country == "Canada" && p.Supplier.Region == "Québec")
                      select p;

            foreach (var t in res)
                ;
             */

            //var res = from t in src.GetList<Test>() group t by t.Age;// select new { Age = g.Key };
            //var res = from t in src.GetList<Test>() group t by t.Age into g select g;
            //var res = from t in src.GetList<Test>() group t by t.Age into g select new { Age = g.Key, Items = g.GetEnumerator() };
            //var res = from t in src.GetList<Test>() group t by t.Age into g where g.Key >= 25 select new { Age = g.Key, Items = g.GetEnumerator() };
            //var res1 = (from t in src.GetList<Test>() select t).First();
            //var res2 = (from t in src.GetList<Test>() select t).First();
            //bool b = object.ReferenceEquals(res1, res2);
            //SharePointListQueryVisualizer.TestShowVisualizer(res);

            /*
            foreach (var t in src.GetList<Test>())
            {
            }

            string s = "";
             */

            /*
            var res2 = from t in src.GetList<Test>() select t;
            foreach (var t in res2)
                ;

            var res3 = (from t in src.GetList<Test>() select t).First();
             

            var res4 = (from t in src.GetList<Test>() where t.Age >= 24 orderby t.FirstName select new { t.Age, t.FirstName }).First();
             */

            //SPSite site = new SPSite("http://wss3demo");

            //var lst = new SharePointList<Test>(new SharePointDataContext(new Uri("http://wss3demo")));
            //var temp = (from t in lst select t).Skip(5);
            //var temp = from t in lst where !t.FirstName.Contains("a") select t;
            //var temp = (from t in lst select new { Name = t.FirstName, Age = t.Age }).Select(t => t.Age);
            //var temp = from t in lst orderby 1 select t;
            //var temp = (from t in lst select new { Name = t.FirstName }).Where(t => t.Name.StartsWith("B"));
            //var temp = from t in lst where !(t.FirstName == "Bart" && t.Age >= 24) || t.LastName.StartsWith("De Smet") select t;
            //SharePointListQueryVisualizer.TestShowVisualizer(temp);
            //var res = (from t in lst where !CamlMethods.DateRangesOverlap(DateTime.Now, t.Modified) || (t.Age >= 24 && !t.LastName.StartsWith("Smet")) orderby t.FirstName descending select new { Name = t.FirstName + " " + t.LastName }).Skip(5);
            //var res = (from t in lst where CamlMethods.DateRangesOverlap(DateTime.Now) select t);
            //var res = (from t in lst where CamlMethods.DateRangesOverlap(t.Modified.Value) select t);
            //Uri u = new Uri("http://www.test.be");
            //var res = (from t in lst where u.IsAbsoluteUri select t);
            //var res = (from t in lst where u.IsBaseOf(new Uri("http://www.test2.be")) select t);
            //Junk j = new Junk();
            //var res = (from t in lst where j.User.IsMember select t);
            //var res = from t in lst where t.FirstName.EndsWith("Test") select t;
            //var res = from t in lst where t.Created.Value.IsDaylightSavingTime() select t;
            //var res = from t in lst where !(t.FirstName.Contains("Bart") && t.Age >= 24) || t.LastName.EndsWith("De Smet") && CamlMethods.DateRangesOverlap(t.Modified.Value) orderby 1 select t;
            //var res = (from t in lst where t.LastName.Length == 0 select t);
            //var res = from t in lst where CamlMethods.DateRangesOverlap(DateTime.Now, t.Modified, t.Created) orderby 1 select t;
            //var res = from t in lst where t.FirstName.Contains(t.LastName) select t;
            //SharePointListQueryVisualizer.TestShowVisualizer(res);

            //SharePointDataSource<Test> lst = new SharePointDataSource<Test>(site);//new Uri("http://wss3demo"));
            //SharePointDataSource<Users> usr = new SharePointDataSource<Users>(site);//new Uri("http://wss3demo"));
            //lst.Log = Console.Out;
            //usr.Log = Console.Out;
            //SharePointDataSource<Parent> lst = new SharePointDataSource<Parent>(site);
            //lst.Log = Console.Out;
            //lst.EnforceLookupFieldUniqueness = false;

            /*
            string s = "b";
            var res = from p in lst where "a" == s select p;//where 1 == 1 || p.Title == "Test" && p.Bar.Title.StartsWith("Bart") select p;
            foreach (var p in res)
                ;
            */

            /*
            var lst = new SharePointDataSource<Tasks>(site);
            lst.Log = Console.Out;
            //var res = from t in lst where t.Created == DateTime.Today select t;
            //var res = from t in lst where t.Created == DateTime.Today.AddDays(1) select t;
            var res = from t in lst where CamlElements.DateRangesOverlap(DateTime.Now, t.Created, t.StartDate) select t;
            foreach (var t in res)
                ;
             */
            /*
            var lst = new SharePointDataSource<Test>(site);
            lst.Log = Console.Out;
            var res = from t in lst where CamlMethods.DateRangesOverlap(DateTime.Now.AddDays(1), t.User.Modified, t.User.Created) select t;
            foreach (var t in res)
                ;
             */

            /*
            var lst = new SharePointDataSource<Test>(site);
            lst.Log = Console.Out;
            //var res = from t in lst where t.FirstName == "Bart" where t.Age == 24 select t;
            //var res = from t in lst where 1 == 1 where t.Age == 24 select t;
            //var res = from t in lst where t.FirstName == "Bart" where 1 == 1 select t;
            //var res = from t in lst where 1 == 0 where t.Age == 24 select t;
            //var res = from t in lst where t.FirstName == "Bart" where 1 == 0 select t;
            foreach (var t in res)
                ;
             */

            /*
            var res = from p in lst where p.Title == "Test" && p.Bar.Title.StartsWith("Bart") && p.Foo.Title.StartsWith("De Smet") select p;
            foreach (var p in res)
                Console.WriteLine(p);
            */
            /*

            //SELECTMANY
            //
            //var r = from t in lst
            //        from u in usr
            //        where t.UsersMulti.Contains(u)
            //        select t;

            var res = from t in lst 
                      //where t.UsersMulti.Contains(null)
                      //where t.UsersMulti.Contains(usr.GetEntityById(1, true))
                      //where t.User.Title.StartsWith("Bart")
                      where t.User.ID == 1 || t.User == usr.GetEntityById(3, true)
                      select t;
            foreach (var t in res)
                Console.WriteLine(t.Title + " " + t.User.ID);
                //if (t.UsersMulti != null)
                //    Console.WriteLine(t.User.Title);
            */
            /*
            SPSite s = new SPSite("http://wss3demo");
            SPList lst = s.RootWeb.GetList("/Lists/Test");
            lst.EnableSyndication = true;
            lst.Update();
            //lst.EnsureRssSettings();
            
            Lists l = new Lists();
            l.Credentials = CredentialCache.DefaultNetworkCredentials;
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var xw = new XmlTextWriter(sw);
            xw.Formatting = Formatting.Indented;
            l.GetListAndView(args[0], null).WriteTo(xw);
            Console.WriteLine(sb.ToString());
             

            //StsAdapter ws = new StsAdapter();
            //ws.Credentials = CredentialCache.DefaultNetworkCredentials;
            //QueryRequest req = new QueryRequest();
            //req.dsQuery = new DSQuery();
            //req.dsQuery.Query = new DspQuery();

            //ws.Query(req);
             
            //var src = new SharePointDataSource<Tasks>(new Uri("http://localhost"));
            //src.Log = Console.Out;
            //var res = from t in src select t;// new { t.Title, t.StartDate, t.DueDate, t.Complete, t.Description };
            //foreach (var t in res)
            //{
            //    Console.WriteLine(t.Title + " = " + t.Description);
            //    Console.WriteLine("Start: {0} - End: {1} - Complete: {2}", t.StartDate, t.DueDate, t.Complete);
            //}
             */
        }
    }

    /*
    /// <summary>
    /// UrlTest
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("UrlTest", Id = "a1ff3460-b7d2-47cd-a986-e51b15f90dfd", Version = 1, Path = "/Lists/UrlTest")]
    public partial class UrlTest : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private global::BdsSoft.SharePoint.Linq.Url _Homepage;

        private int _ID = default(int);

        private string _ContentType = default(string);

        private global::System.Nullable<System.DateTime> _Modified = default(global::System.Nullable<System.DateTime>);

        private global::System.Nullable<System.DateTime> _Created = default(global::System.Nullable<System.DateTime>);

        private string _Version = default(string);

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
        /// Homepage
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Homepage", global::BdsSoft.SharePoint.Linq.FieldType.URL, Id = "f188c0ac-fc72-4a79-b10c-82f35f23caa5", Storage = "_Homepage")]
        public global::BdsSoft.SharePoint.Linq.Url Homepage
        {
            get
            {
                return this._Homepage;
            }
            set
            {
                if ((this._Homepage != value))
                {
                    this.OnPropertyChanging("Homepage");
                    this._Homepage = value;
                    this.OnPropertyChanged("Homepage");
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
    /// Use the Tasks list to keep track of work that you or your team needs to complete.
    /// </summary>
    [List("Tasks", Id = "f9a325b5-8929-4ebd-9fdc-2ebdebf459fd", Version = 0, Path = "/Lists/Tasks")]
    class Tasks// : SharePointListEntity
    {
        /// <summary>
        /// Content Type
        /// </summary>
        [Field("ContentType", FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType
        {
            get { return (string)GetValue("ContentType"); }
            set { SetValue("ContentType", value); }
        }

        /// <summary>
        /// Title
        /// </summary>
        [Field("Title", FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public string Title { get; set; }

        /// <summary>
        /// Modified
        /// </summary>
        [Field("Modified", FieldType.DateTime, Id = "28cf69c5-fa48-462a-b5cd-27b6f9d2bd5f", ReadOnly = true)]
        public System.DateTime? Modified { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        [Field("Created", FieldType.DateTime, Id = "8c06beca-0777-48f7-91c7-6da68bc07b69", ReadOnly = true)]
        public System.DateTime? Created { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [Field("_UIVersionString", FieldType.Text, Id = "dce8262a-3ae9-45aa-aab4-83bd75fb738a", ReadOnly = true)]
        public string Version { get; set; }

        /// <summary>
        /// Priority
        /// </summary>
        [Field("Priority", FieldType.Choice, Id = "a8eb573e-9e11-481a-a8c9-1104a54b2fbd")]
        public Priority? Priority { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Field("Status", FieldType.Choice, Id = "c15b34c3-ce7d-490a-b133-3f4de8801b76")]
        public Status? Status { get; set; }

        /// <summary>
        /// % Complete
        /// </summary>
        [Field("PercentComplete", FieldType.Number, Id = "d2311440-1ed6-46ea-b46d-daa643dc3886")]
        public double? Complete { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Field("Body", FieldType.Note, Id = "7662cd2c-f069-4dba-9e35-082cf976e170")]
        public string Description { get; set; }

        /// <summary>
        /// Start Date
        /// </summary>
        [Field("StartDate", FieldType.DateTime, Id = "64cd368d-2f95-4bfc-a1f9-8d4324ecb007")]
        public System.DateTime? StartDate { get; set; }

        /// <summary>
        /// Due Date
        /// </summary>
        [Field("DueDate", FieldType.DateTime, Id = "cd21b4c2-6841-4f9e-a23a-738a65f99889")]
        public System.DateTime? DueDate { get; set; }
    }

    enum Priority : uint { [Choice("(1) High")] _1High, [Choice("(2) Normal")] _2Normal, [Choice("(3) Low")] _3Low }

    enum Status : uint { [Choice("Not Started")] NotStarted, [Choice("In Progress")] InProgress, Completed, Deferred, [Choice("Waiting on someone else")] WaitingOnSomeoneElse }

    /// <summary>
    /// Some description
    /// </summary>
    [List("Test", Id = "f49dd431-7a05-4532-8ef5-af507badc427", Version = 14, Path = "/Lists/Test")]
    class Test// : SharePointListEntity
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
        /// First name
        /// </summary>
        [Field("First name", FieldType.Text, Id = "5e871834-d5ee-412b-87ac-bee1e6562800")]
        public string FirstName
        {
            get { return (string)GetValue("FirstName"); }
            set { SetValue("FirstName", value); }
        }

        /// <summary>
        /// Last name
        /// </summary>
        [Field("Last name", FieldType.Text, Id = "1c110c26-7cf2-461b-97a7-ed8c0b709d17")]
        public string LastName
        {
            get { return (string)GetValue("LastName"); }
            set { SetValue("LastName", value); }
        }

        /// <summary>
        /// IsMember
        /// </summary>
        [Field("IsMember", FieldType.Boolean, Id = "cac1f3a4-716d-449e-a9f0-ab409ef30c85")]
        public bool IsMember
        {
            get { return (bool)GetValue("IsMember"); }
            set { SetValue("IsMember", value); }
        }

        /// <summary>
        /// Age
        /// </summary>
        [Field("Age", FieldType.Number, Id = "fded6112-64ea-4c9d-88de-f049803a28c9")]
        public double Age
        {
            get { return (double)GetValue("Age"); }
            set { SetValue("Age", value); }
        }

        /// <summary>
        /// ShortBio
        /// </summary>
        [Field("ShortBio", FieldType.Note, Id = "d50d894d-64df-4bab-a94e-6ec8046e3969")]
        public string ShortBio
        {
            get { return (string)GetValue("ShortBio"); }
            set { SetValue("ShortBio", value); }
        }

        /// <summary>
        /// User
        /// </summary>
        [Field("User", FieldType.Lookup, Id = "0965897f-74c2-4242-b70f-20713f768045", LookupDisplayField = "Title")]
        public Users User
        {
            get { return (Users)GetValue("User"); }
            set { SetValue("User", value); }
        }

        /// <summary>
        /// UsersMulti
        /// </summary>
        [Field("UsersMulti", FieldType.LookupMulti, Id = "4b86be8a-f422-4e77-a2ec-0819d397c77a", LookupDisplayField = "Title")]
        public IList<Users> UsersMulti
        {
            get { return (IList<Users>)GetValue("UsersMulti"); }
        }

        /// <summary>
        /// Spouse
        /// </summary>
        [Field("Spouse", FieldType.Lookup, Id = "00b7eecc-8a07-49a9-bb9f-8041f315103b", LookupDisplayField = "Title")]
        public Test Spouse
        {
            get { return (Test)GetValue("Spouse"); }
            set { SetValue("Spouse", value); }
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
    /// Users
    /// </summary>
    [List("Users", Id = "2b719ff2-8390-4386-9089-39570ddfa7ae", Version = 0, Path = "/Lists/Users")]
    class Users// : SharePointListEntity
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

        [Field("IsMember", FieldType.Boolean, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247")]
        public bool IsMember
        { get; set; }

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
     */

    /// <summary>
    /// Products
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("Products", Id = "d6be2daa-f0c3-48fe-ae0b-3d3bd6b860ef", Version = 17, Path = "/Lists/Products")]
    public partial class Product : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

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

        private string _ContentType = default(string);

        private global::System.Nullable<System.DateTime> _Modified = default(global::System.Nullable<System.DateTime>);

        private global::System.Nullable<System.DateTime> _Created = default(global::System.Nullable<System.DateTime>);

        private string _Version = default(string);

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
        /// ProductName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ProductName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "38d0b85f-fa6d-4888-83c3-1a2a1a05d944", Storage = "_ProductName")]
        public string ProductName
        {
            get
            {
                return this._ProductName;
            }
            set
            {
                if ((this._ProductName != value))
                {
                    this.OnPropertyChanging("ProductName");
                    this._ProductName = value;
                    this.OnPropertyChanged("ProductName");
                }
            }
        }

        /// <summary>
        /// QuantityPerUnit
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("QuantityPerUnit", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "b418ce10-53e8-4bc5-823c-1fe199b3df70", Storage = "_QuantityPerUnit")]
        public string QuantityPerUnit
        {
            get
            {
                return this._QuantityPerUnit;
            }
            set
            {
                if ((this._QuantityPerUnit != value))
                {
                    this.OnPropertyChanging("QuantityPerUnit");
                    this._QuantityPerUnit = value;
                    this.OnPropertyChanged("QuantityPerUnit");
                }
            }
        }

        /// <summary>
        /// UnitPrice
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("UnitPrice", global::BdsSoft.SharePoint.Linq.FieldType.Currency, Id = "8daebd3b-9fbf-4858-9f05-1e88f62a0417", Storage = "_UnitPrice")]
        public global::System.Nullable<double> UnitPrice
        {
            get
            {
                return this._UnitPrice;
            }
            set
            {
                if ((this._UnitPrice != value))
                {
                    this.OnPropertyChanging("UnitPrice");
                    this._UnitPrice = value;
                    this.OnPropertyChanged("UnitPrice");
                }
            }
        }

        /// <summary>
        /// UnitsInStock
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("UnitsInStock", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id = "82117b89-5115-42dc-82e5-ec494cb93258", Storage = "_UnitsInStock")]
        public global::System.Nullable<double> UnitsInStock
        {
            get
            {
                return this._UnitsInStock;
            }
            set
            {
                if ((this._UnitsInStock != value))
                {
                    this.OnPropertyChanging("UnitsInStock");
                    this._UnitsInStock = value;
                    this.OnPropertyChanged("UnitsInStock");
                }
            }
        }

        /// <summary>
        /// UnitsOnOrder
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("UnitsOnOrder", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id = "68a03a4e-9b42-4437-a38b-33f6ed69ad4d", Storage = "_UnitsOnOrder")]
        public global::System.Nullable<double> UnitsOnOrder
        {
            get
            {
                return this._UnitsOnOrder;
            }
            set
            {
                if ((this._UnitsOnOrder != value))
                {
                    this.OnPropertyChanging("UnitsOnOrder");
                    this._UnitsOnOrder = value;
                    this.OnPropertyChanged("UnitsOnOrder");
                }
            }
        }

        /// <summary>
        /// ReorderLevel
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ReorderLevel", global::BdsSoft.SharePoint.Linq.FieldType.Number, Id = "c63ef5b7-d6b6-4246-967d-a60c41018ed9", Storage = "_ReorderLevel")]
        public global::System.Nullable<double> ReorderLevel
        {
            get
            {
                return this._ReorderLevel;
            }
            set
            {
                if ((this._ReorderLevel != value))
                {
                    this.OnPropertyChanging("ReorderLevel");
                    this._ReorderLevel = value;
                    this.OnPropertyChanged("ReorderLevel");
                }
            }
        }

        /// <summary>
        /// Discontinued
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Discontinued", global::BdsSoft.SharePoint.Linq.FieldType.Boolean, Id = "4df879f5-7a2f-4db0-baac-7593c0fdc112", Storage = "_Discontinued")]
        public global::System.Nullable<bool> Discontinued
        {
            get
            {
                return this._Discontinued;
            }
            set
            {
                if ((this._Discontinued != value))
                {
                    this.OnPropertyChanging("Discontinued");
                    this._Discontinued = value;
                    this.OnPropertyChanged("Discontinued");
                }
            }
        }

        /// <summary>
        /// Category
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Category", global::BdsSoft.SharePoint.Linq.FieldType.Lookup, Id = "081784b3-2bba-4456-a25c-2fc1f6c4d492", LookupDisplayField = "CategoryName", Storage = "_Category")]
        public Category Category
        {
            get
            {
                return this._Category.Entity;
            }
            set
            {
                if ((this._Category.Entity != value))
                {
                    this.OnPropertyChanging("Category");
                    this._Category.Entity = value;
                    this.OnPropertyChanged("Category");
                }
            }
        }

        /// <summary>
        /// Supplier
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Supplier", global::BdsSoft.SharePoint.Linq.FieldType.Lookup, Id = "e6fd6cbc-e50e-486c-89c5-8fb65e5099a1", LookupDisplayField = "CompanyName", Storage = "_Supplier")]
        public Supplier Supplier
        {
            get
            {
                return this._Supplier.Entity;
            }
            set
            {
                if ((this._Supplier.Entity != value))
                {
                    this.OnPropertyChanging("Supplier");
                    this._Supplier.Entity = value;
                    this.OnPropertyChanged("Supplier");
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
    /// Categories
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("Categories", Id = "2f5a24e6-43dd-4af0-b28f-dfb75c96c380", Version = 4, Path = "/Lists/Categories")]
    public partial class Category : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private string _CategoryName;

        private string _Description;

        private int _ID = default(int);

        private string _ContentType = default(string);

        private global::System.Nullable<System.DateTime> _Modified = default(global::System.Nullable<System.DateTime>);

        private global::System.Nullable<System.DateTime> _Created = default(global::System.Nullable<System.DateTime>);

        private string _Version = default(string);

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
        /// CategoryName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("CategoryName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "6a41acdd-f021-417e-9a39-2c687381de56", Storage = "_CategoryName")]
        public string CategoryName
        {
            get
            {
                return this._CategoryName;
            }
            set
            {
                if ((this._CategoryName != value))
                {
                    this.OnPropertyChanging("CategoryName");
                    this._CategoryName = value;
                    this.OnPropertyChanged("CategoryName");
                }
            }
        }

        /// <summary>
        /// Description
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Description", global::BdsSoft.SharePoint.Linq.FieldType.Note, Id = "b78064ae-3482-4e31-aa4a-bb26db5e506d", Storage = "_Description")]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                if ((this._Description != value))
                {
                    this.OnPropertyChanging("Description");
                    this._Description = value;
                    this.OnPropertyChanged("Description");
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
    /// Suppliers
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("Suppliers", Id = "287cc088-9e8f-4c34-b3a0-e8a9d89f3f39", Version = 12, Path = "/Lists/Suppliers")]
    public partial class Supplier : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

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

        private string _ContentType = default(string);

        private global::System.Nullable<System.DateTime> _Modified = default(global::System.Nullable<System.DateTime>);

        private global::System.Nullable<System.DateTime> _Created = default(global::System.Nullable<System.DateTime>);

        private string _Version = default(string);

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
        /// CompanyName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("CompanyName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "cd52757f-2870-497f-ba88-1285fab4c7e1", Storage = "_CompanyName")]
        public string CompanyName
        {
            get
            {
                return this._CompanyName;
            }
            set
            {
                if ((this._CompanyName != value))
                {
                    this.OnPropertyChanging("CompanyName");
                    this._CompanyName = value;
                    this.OnPropertyChanged("CompanyName");
                }
            }
        }

        /// <summary>
        /// ContactName
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContactName", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "8d821f90-796f-49f2-9278-0784d6f95ee6", Storage = "_ContactName")]
        public string ContactName
        {
            get
            {
                return this._ContactName;
            }
            set
            {
                if ((this._ContactName != value))
                {
                    this.OnPropertyChanging("ContactName");
                    this._ContactName = value;
                    this.OnPropertyChanged("ContactName");
                }
            }
        }

        /// <summary>
        /// ContactTitle
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("ContactTitle", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "196b85f9-10da-4f87-9332-dfb52a9603c1", Storage = "_ContactTitle")]
        public string ContactTitle
        {
            get
            {
                return this._ContactTitle;
            }
            set
            {
                if ((this._ContactTitle != value))
                {
                    this.OnPropertyChanging("ContactTitle");
                    this._ContactTitle = value;
                    this.OnPropertyChanged("ContactTitle");
                }
            }
        }

        /// <summary>
        /// Address
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Address", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "cc2a010d-e22e-498a-a85a-8d32af374d58", Storage = "_Address")]
        public string Address
        {
            get
            {
                return this._Address;
            }
            set
            {
                if ((this._Address != value))
                {
                    this.OnPropertyChanging("Address");
                    this._Address = value;
                    this.OnPropertyChanged("Address");
                }
            }
        }

        /// <summary>
        /// City
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("City", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "cc559100-d7e6-4817-8f31-ff89333e5860", Storage = "_City")]
        public string City
        {
            get
            {
                return this._City;
            }
            set
            {
                if ((this._City != value))
                {
                    this.OnPropertyChanging("City");
                    this._City = value;
                    this.OnPropertyChanged("City");
                }
            }
        }

        /// <summary>
        /// Region
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Region", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "6d0af8b5-2735-4e19-8806-dfd3eaf6aeca", Storage = "_Region")]
        public string Region
        {
            get
            {
                return this._Region;
            }
            set
            {
                if ((this._Region != value))
                {
                    this.OnPropertyChanging("Region");
                    this._Region = value;
                    this.OnPropertyChanged("Region");
                }
            }
        }

        /// <summary>
        /// PostalCode
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("PostalCode", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "3e84ea38-ae73-4387-8c91-09972eb3b7d2", Storage = "_PostalCode")]
        public string PostalCode
        {
            get
            {
                return this._PostalCode;
            }
            set
            {
                if ((this._PostalCode != value))
                {
                    this.OnPropertyChanging("PostalCode");
                    this._PostalCode = value;
                    this.OnPropertyChanged("PostalCode");
                }
            }
        }

        /// <summary>
        /// Country
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Country", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "04f6dc29-0884-42ac-99eb-aac3bb1326bd", Storage = "_Country")]
        public string Country
        {
            get
            {
                return this._Country;
            }
            set
            {
                if ((this._Country != value))
                {
                    this.OnPropertyChanging("Country");
                    this._Country = value;
                    this.OnPropertyChanged("Country");
                }
            }
        }

        /// <summary>
        /// Phone
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Phone", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "82015708-da95-45b5-b1b2-77426e4e33d9", Storage = "_Phone")]
        public string Phone
        {
            get
            {
                return this._Phone;
            }
            set
            {
                if ((this._Phone != value))
                {
                    this.OnPropertyChanging("Phone");
                    this._Phone = value;
                    this.OnPropertyChanged("Phone");
                }
            }
        }

        /// <summary>
        /// Fax
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Fax", global::BdsSoft.SharePoint.Linq.FieldType.Text, Id = "61f037f4-f780-4b21-a8e3-b60cfed1c4d2", Storage = "_Fax")]
        public string Fax
        {
            get
            {
                return this._Fax;
            }
            set
            {
                if ((this._Fax != value))
                {
                    this.OnPropertyChanging("Fax");
                    this._Fax = value;
                    this.OnPropertyChanged("Fax");
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
    /// LookupMultiTest
    /// </summary>
    [global::BdsSoft.SharePoint.Linq.ListAttribute("LookupMultiTest", Id = "b15d38c9-fa09-4736-a95f-9bd217df9da8", Version = 1, Path = "/Lists/LookupMultiTest")]
    public partial class LookupMultiTest : global::System.ComponentModel.INotifyPropertyChanged, global::System.ComponentModel.INotifyPropertyChanging
    {

        private string _Title;

        private global::BdsSoft.SharePoint.Linq.EntitySet<Category> _Categories = default(global::BdsSoft.SharePoint.Linq.EntitySet<Category>);

        private int _ID = default(int);

        private string _ContentType = default(string);

        private global::System.Nullable<System.DateTime> _Modified = default(global::System.Nullable<System.DateTime>);

        private global::System.Nullable<System.DateTime> _Created = default(global::System.Nullable<System.DateTime>);

        private string _Version = default(string);

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
        /// Categories
        /// </summary>
        [global::BdsSoft.SharePoint.Linq.FieldAttribute("Categories", global::BdsSoft.SharePoint.Linq.FieldType.LookupMulti, Id = "72d4ca7f-637a-4161-8a3e-a9a84782fa0d", LookupDisplayField = "CategoryName", Storage = "_Categories")]
        public global::BdsSoft.SharePoint.Linq.EntitySet<Category> Categories
        {
            get
            {
                return this._Categories;
            }
            set
            {
                this._Categories.Assign(value);
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

    public partial class NorthwindSharePointDataContext : global::BdsSoft.SharePoint.Linq.SharePointDataContext
    {

        /// <summary>
        /// Connect to SharePoint using the SharePoint web services.
        /// </summary>
        /// <param name="wsUri">URI to the SharePoint site.</param>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public NorthwindSharePointDataContext(System.Uri wsUri)
            :
                base(wsUri)
        {
        }

        /// <summary>
        /// Connect to SharePoint using the SharePoint object model.
        /// </summary>
        /// <param name="site">SharePoint site object.</param>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public NorthwindSharePointDataContext(Microsoft.SharePoint.SPSite site)
            :
                base(site)
        {
        }

        /// <summary>
        /// Connect to the http://wss3demo SharePoint site using the SharePoint web services.
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public NorthwindSharePointDataContext()
            :
                base(new global::System.Uri("http://wss3demo"))
        {
        }

        /// <summary>
        /// Products list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Product> Products
        {
            get
            {
                return this.GetList<Product>();
            }
        }

        /// <summary>
        /// Categories list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Category> Categories
        {
            get
            {
                return this.GetList<Category>();
            }
        }

        /// <summary>
        /// Suppliers list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<Supplier> Suppliers
        {
            get
            {
                return this.GetList<Supplier>();
            }
        }

        /// <summary>
        /// LookupMultiTest list.
        /// </summary>
        public global::BdsSoft.SharePoint.Linq.SharePointList<LookupMultiTest> LookupMultiTest
        {
            get
            {
                return this.GetList<LookupMultiTest>();
            }
        }
    }
}