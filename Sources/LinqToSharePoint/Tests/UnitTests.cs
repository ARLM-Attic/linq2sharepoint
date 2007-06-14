/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

/*
 * NOTE: For the moment, the unit testing framework for LINQ to SharePoint is in the planning stage.
 *       Therefore, this class will be of little use to project "readers" at the moment (05/04/07).
 */

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SharePoint;
using BdsSoft.SharePoint.Linq;
using Test = Tests.SharePointListEntityTest;

namespace Tests
{
    /// <summary>
    /// Unit tests for LINQ to SharePoint queries.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        private SPSite site;
        private SPWeb web;

        public UnitTests()
        {
            site = new SPSite("http://wss3demo");
            web = site.RootWeb;
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            //
            // Create list People.
            //
            SPList lst = Test.Create<People>(site.RootWeb);

            //
            // Empty list test.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;
            var res1 = from p in src
                       select p;

            Assert.IsTrue(res1.AsEnumerable().Count() == 0, "Query on empty list did return results.");

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBio = "Project founder" };
            Test.Add(lst, p1);

            //
            // Test default query.
            //
            var res2 = from p in src
                       select p;

            Assert.IsTrue(res2.AsEnumerable().Count() == 1, "Query did not return results.");

            //
            // Get entity by id.
            //
            People _p1 = src.GetEntityById(1, false);
            Assert.IsTrue(p1.Equals(_p1), "Invalid entity returned by GetEntityById method");

            //Assert.IsTrue(true);
        }

        #region Scratch pad

        /*
            SPList list = CreateList(
                new CustomList()
                    {
                        Name = "Test",
                        Description = "Some description",
                        Fields = new List<Field>
                        {
                            new Field() { Name = "First name", Type = SPFieldType.Text, Required = true },
                            new Field() { Name = "Last name", Type = SPFieldType.Text, Required = true },
                            new Field() { Name = "IsMember", Type = SPFieldType.Boolean, Required = true },
                            new Field() { Name = "Age", Type = SPFieldType.Number, Required = true },
                            new Field() { Name = "ShortBio", Type = SPFieldType.Note, Required = false },
                        }
                    }
                );
            
            SPListItem item = list.Items.Add();
            item["First name"] = "Bart";
            item["Last name"] = "De Smet";
            item["IsMember"] = true;
            item["Age"] = 24;
            item.Update();
             */

        //private SPList CreateList(CustomList list)
        //{
        //    SPList lst;
        //    try
        //    {
        //        lst = web.Lists[list.Name];
        //        if (lst != null)
        //            lst.Delete();
        //    }
        //    catch { }

        //    web.Lists.Add(list.Name, list.Description, SPListTemplateType.GenericList);
        //    lst = web.Lists[list.Name];
        //    lst.OnQuickLaunch = true;
        //    lst.Update();

        //    foreach (Field f in list.Fields)
        //    {
        //        lst.Fields.Add(f.Name, f.Type, f.Required);
        //        lst.Views[0].ViewFields.Add(lst.Fields[f.Name]);
        //    }

        //    return lst;
        //}
        #endregion
    }
}
