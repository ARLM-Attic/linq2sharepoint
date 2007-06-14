﻿/*
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
        private SPList lst;

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
        public void EmptyList()
        {
            //
            // Create list People.
            //
            lst = Test.Create<People>(site.RootWeb);

            //
            // Empty list test.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;
            var res1 = (from p in src select p).AsEnumerable();

            Assert.IsTrue(res1.Count() == 0, "Query on empty list did return results.");
        }

        [TestMethod]
        public void DefaultQuery()
        {
            //
            // Create list People.
            //
            lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBio = "Project founder" };
            Test.Add(lst, p1);

            //
            // Test default query.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;
            var res1 = (from p in src select p).AsEnumerable();

            Assert.IsTrue(res1.Count() == 1, "Query did not return results.");
        }

        [TestMethod]
        public void GetEntityById()
        {
            //
            // Create list People.
            //
            lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { ID = 1, FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBio = "Project founder" };
            Test.Add(lst, p1);

            //
            // Get entity by id.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;
            People _p1 = src.GetEntityById(1, false);
            Assert.IsTrue(p1.Equals(_p1), "Invalid entity returned by GetEntityById method");
        }

        [TestMethod]
        public void StringEquality()
        {
            //
            // Create list People.
            //
            lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { ID = 1, FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBio = "Project founder" };
            People p2 = new People() { ID = 2, FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBio = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // List source.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;

            //
            // Using ==
            //
            AssertWhere(src, p => p.FirstName == "Bart", 1, "Equality check failed (==)");

            //
            // Using .Equals
            //
            AssertWhere(src, p => p.FirstName.Equals("Bart"), 1, "Equality check failed (.Equals)");

            //
            // Using == inverse order
            //
            AssertWhere(src, p => "Bart" == p.FirstName, 1, "Equality check failed (== inverse order)");

            //
            // Using .Equals inverse order (NOT SUPPORTED)
            //
            //AssertWhere(src, p => "Bart".Equals(p.FirstName), 1, "Equality check failed (.Equals inverse order)");

            //
            // Using == with .ToString
            //
            AssertWhere(src, p => p.FirstName.ToString() == "Bart".ToString(), 1, "Equality check failed (== with .ToString)");

            //
            // Using .Equals with .ToString
            //
            AssertWhere(src, p => p.FirstName.ToString().Equals("Bart".ToString()), 1, "Equality check failed (.Equals with .ToString)");

            //
            // Using == with .ToString
            //
            AssertWhere(src, p => p.FirstName.ToString() == "Bart".ToString(), 1, "Equality check failed (== with .ToString inverse order)");

            //
            // Using .Equals with .ToString
            //
            //AssertWhere(src, p => p.FirstName.ToString().Equals("Bart".ToString()), 1, "Equality check failed (.Equals with .ToString inverse order)");
        }

        [TestMethod]
        public void LtLeqGtGeq()
        {
            //
            // Create list People.
            //
            lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { ID = 1, FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBio = "Project founder" };
            People p2 = new People() { ID = 2, FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBio = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // List source.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;

            //
            // Gt
            //
            AssertWhere(src, p => p.Age > 50, 1, "Invalid Gt result");
            AssertWhere(src, p => 50 < p.Age, 1, "Invalid Gt result (inverse)");
            AssertWhere(src, p => !(p.Age <= 50), 1, "Invalid Gt result (negated)");
            AssertWhere(src, p => !(50 >= p.Age), 1, "Invalid Gt result (negated) (inverse)");

            //
            // Geq
            //
            AssertWhere(src, p => p.Age >= 52, 1, "Invalid Geq result");
            AssertWhere(src, p => 52 <= p.Age, 1, "Invalid Geq result (inverse)");
            AssertWhere(src, p => !(p.Age < 52), 1, "Invalid Geq result (negated)");
            AssertWhere(src, p => !(52 > p.Age), 1, "Invalid Geq result (negated) (inverse)");

            //
            // Lt
            //
            AssertWhere(src, p => p.Age < 25, 1, "Invalid Lt result");
            AssertWhere(src, p => 25 > p.Age, 1, "Invalid Lt result (inverse)");
            AssertWhere(src, p => !(p.Age >= 25), 1, "Invalid Lt result (negated)");
            AssertWhere(src, p => !(25 <= p.Age), 1, "Invalid Lt result (negated) (inverse)");

            //
            // Leq
            //
            AssertWhere(src, p => p.Age <= 24, 1, "Invalid Leq result");
            AssertWhere(src, p => 24 >= p.Age, 1, "Invalid Leq result (inverse)");
            AssertWhere(src, p => !(p.Age > 24), 1, "Invalid Leq result (negated)");
            AssertWhere(src, p => !(24 < p.Age), 1, "Invalid Leq result (negated) (inverse)");
        }

        private static void AssertWhere<T>(SharePointDataSource<T> src, Func<T, bool> predicate, int expectedCount, string message)
        {
            IEnumerable<T> res = src.Where<T>(predicate).Select(e => e).AsEnumerable();
            Assert.IsTrue(res.Count() == expectedCount && res.All(predicate), message);
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
