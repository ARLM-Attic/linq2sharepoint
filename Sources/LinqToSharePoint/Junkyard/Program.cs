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

namespace Junkyard
{
    class Program
    {
        static void Main(string[] args)
        {   
            var src = new SharePointDataContext(new Uri("http://wss3demo"));
            /*
            var res = from t in src.GetList<Test>() where t.Age >= 24 orderby t.FirstName select new { t.Age, t.FirstName };
            foreach (var t in res)
                ;

            var res2 = from t in src.GetList<Test>() select t;
            foreach (var t in res2)
                ;

            var res3 = (from t in src.GetList<Test>() select t).First();
             */

            var res4 = (from t in src.GetList<Test>() where t.Age >= 24 orderby t.FirstName select new { t.Age, t.FirstName }).First();
            
            /*
            SPSite site = new SPSite("http://wss3demo");

            SharePointDataSource<Test> lst = new SharePointDataSource<Test>(site);
            var res = from t in lst where t.Age >= 24 orderby t.FirstName descending select new { Name = t.FirstName + " " + t.LastName };
            SharePointDataSourceVisualizer.TestShowVisualizer(res);
             */

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

    /// <summary>
    /// Use the Tasks list to keep track of work that you or your team needs to complete.
    /// </summary>
    [List("Tasks", Id = "f9a325b5-8929-4ebd-9fdc-2ebdebf459fd", Version = 0, Path = "/Lists/Tasks")]
    class Tasks : SharePointListEntity
    {
        /// <summary>
        /// Content Type
        /// </summary>
        [Field("ContentType", FieldType.Text, Id = "c042a256-787d-4a6f-8a8a-cf6ab767f12d", ReadOnly = true)]
        public string ContentType {
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
class Test : SharePointListEntity
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
    [Field("User", FieldType.Lookup, Id = "0965897f-74c2-4242-b70f-20713f768045", LookupField = "Title")]
    public Users User
    {
        get { return (Users)GetValue("User"); }
        set { SetValue("User", value); }
    }

    /// <summary>
    /// UsersMulti
    /// </summary>
    [Field("UsersMulti", FieldType.LookupMulti, Id = "4b86be8a-f422-4e77-a2ec-0819d397c77a", LookupField = "Title")]
    public IList<Users> UsersMulti
    {
        get { return (IList<Users>)GetValue("UsersMulti"); }
    }

    /// <summary>
    /// Spouse
    /// </summary>
    [Field("Spouse", FieldType.Lookup, Id = "00b7eecc-8a07-49a9-bb9f-8041f315103b", LookupField = "Title")]
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
class Users : SharePointListEntity
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
    [Field("Title", FieldType.Text, Id = "fa564e0f-0c70-4ab9-b863-0177e6ddd247", IsUnique = true)]
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