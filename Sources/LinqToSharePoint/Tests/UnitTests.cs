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
using System.Diagnostics;
using System.Linq.Expressions;

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
            AssertWhere(src, p => p.FirstName != "Bart", 1, "Inequality check failed (!=)");

            //
            // Using .Equals
            //
            AssertWhere(src, p => p.FirstName.Equals("Bart"), 1, "Equality check failed (.Equals)");
            AssertWhere(src, p => !p.FirstName.Equals("Bart"), 1, "Inequality check failed (!.Equals)");

            //
            // Using == inverse order
            //
            AssertWhere(src, p => "Bart" == p.FirstName, 1, "Equality check failed (== inverse order)");
            AssertWhere(src, p => "Bart" != p.FirstName, 1, "Inquality check failed (!= inverse order)");

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

        [TestMethod]
        public void BoolPredicate()
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
            // Test queries.
            //
            AssertWhere(src, p => p.IsMember, 1, "Invalid result for Boolean check (true)");
            AssertWhere(src, p => !p.IsMember, 1, "Invalid result for Boolean check (false)");
        }

        [TestMethod]
        public void Nullable()
        {
            //
            // Create list People.
            //
            lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { ID = 1, FirstName = "Bart", LastName = "De Smet", Age = 24, SecondAge = 24, IsMember = true, ShortBio = "Project founder" };
            People p2 = new People() { ID = 2, FirstName = "Bill", LastName = "Gates", Age = 52, SecondAge = null, IsMember = false, ShortBio = "Microsoft Corporation founder" };
            People p3 = new People() { ID = 3, FirstName = "William", LastName = "Gates", Age = 52, SecondAge = null, IsMember = false, ShortBio = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);
            Test.Add(lst, p3);

            //
            // List source.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;

            //
            // Test queries.
            //
            AssertWhere(src, p => p.SecondAge.HasValue, 1, "Invalid result for nullable query (1)");
            AssertWhere(src, p => !p.SecondAge.HasValue, 2, "Invalid result for nullable query (2)");
            AssertWhere(src, p => p.SecondAge != null, 1, "Invalid result for nullable query (3)");
            AssertWhere(src, p => p.SecondAge == null, 2, "Invalid result for nullable query (4)");
            AssertWhere(src, p => p.SecondAge == 24, 1, "Invalid result for nullable query (5)");
            AssertWhere(src, p => p.SecondAge.Value == 24, 1, "Invalid result for nullable query (6)");
            AssertWhere(src, p => p.SecondAge >= 25, 0, "Invalid result for nullable query (7)");
        }

        [TestMethod]
        public void StartsWith()
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
            // StartsWith empty string.
            //
            AssertWhere(src, p => p.LastName.StartsWith(""), 2, "StartsWith with empty string failed");

            //
            // StartsWith with valid parameter.
            //
            AssertWhere(src, p => p.LastName.StartsWith("De"), 1, "StartsWith with valid parameter failed");

            //
            // StartsWith without results.
            //
            AssertWhere(src, p => p.FirstName.StartsWith("C"), 0, "Empty StartsWith failed");
        }

        [TestMethod]
        public void Contains()
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
            // Contains empty string.
            //
            AssertWhere(src, p => p.ShortBio.Contains(""), 2, "Contains with empty string failed");

            //
            // Contains with valid parameter.
            //
            AssertWhere(src, p => p.ShortBio.Contains("Corp"), 1, "Contains with valid parameter failed");

            //
            // Contains without results.
            //
            AssertWhere(src, p => p.ShortBio.Contains("azerty"), 0, "Empty Contains failed");
        }

        [TestMethod]
        public void NullChecks()
        {
            //
            // Create list People.
            //
            lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { ID = 1, FirstName = null, LastName = "De Smet", Age = 24, IsMember = true, ShortBio = "Project founder" };
            People p2 = new People() { ID = 2, FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBio = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // List source.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;

            //
            // Test queries.
            //
            AssertWhere(src, p => p.FirstName == null, 1, "Invalid IsNull result");
            //AssertWhere(src, p => p.FirstName.Equals(null), 1, "Invalid IsNull result (.Equals)");
            AssertWhere(src, p => null == p.FirstName, 1, "Invalid IsNull result (inverse)");

            AssertWhere(src, p => p.FirstName != null, 1, "Invalid IsNotNull result");
            //AssertWhere(src, p => !p.FirstName.Equals(null), 1, "Invalid IsNotNull result (.Equals)");
            AssertWhere(src, p => null != p.FirstName, 1, "Invalid IsNotNull result (inverse)");

            AssertWhere(src, p => !(p.FirstName == null), 1, "Invalid IsNotNull result 2");
            AssertWhere(src, p => !(null == p.FirstName), 1, "Invalid IsNotNull result 2 (inverse)");

            //
            // Use of variable.
            //
            string s = null;
            AssertWhere(src, p => p.FirstName == s, 1, "Invalid IsNull result (variable)");
            AssertWhere(src, p => s == p.FirstName, 1, "Invalid IsNull result (variable) (inverse)");

            AssertWhere(src, p => p.FirstName != s, 1, "Invalid IsNotNull result (variable)");
            AssertWhere(src, p => s != p.FirstName, 1, "Invalid IsNotNull result (variable) (inverse)");
        }

        [TestMethod]
        public void ProjectSingleton()
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
            // Projections.
            //
            AssertProject(src, p => p.Age, new double[] { 24, 52 }, "Integer projection failure");
            AssertProject(src, p => p.FirstName + " " + p.LastName, new string[] { "Bart De Smet", "Bill Gates" }, "String projection failure");
        }

        [TestMethod]
        public void ComplexProjection()
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
            // Complex projection. Should trigger recursive parsing of the projection expression to find view fields.
            //
            var res = (from p in src
                       where p.Age == 24
                       select new
                              {
                                  p.FirstName,
                                  Ages = new double[] { p.Age },
                                  Ages2 = new List<double>() { p.Age },
                                  DoubleAge = p.Age * 2,
                                  Member = p.IsMember ? p.IsMember : p.IsMember,
                                  NoMember = !p.IsMember,
                                  NameLower = (p.FirstName + " " + p.LastName).ToLower(),
                                  LastName = new String(p.LastName.ToCharArray()),
                                  CharactersInBio = Helper(p.ShortBio)
                              }).AsEnumerable().First();
            Assert.IsTrue(res.FirstName == "Bart" && res.Ages[0] == 24 && res.Ages2[0] == 24 && res.DoubleAge == 48 && res.Member == true && res.NoMember == false && res.NameLower == "bart de smet" && res.LastName == "De Smet" && res.CharactersInBio == 15);
        }

        private int Helper(string s)
        {
            return s.Length;
        }

        [TestMethod]
        public void Take()
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
            // Take operation.
            //
            var res1 = (from p in src select p).Take(1).AsEnumerable();
            Assert.IsTrue(res1.Count() == 1, "Take failed (1).");
            var res2 = (from p in src select p).Take(0).AsEnumerable();
            Assert.IsTrue(res2.Count() == 0, "Take failed (2).");
            var res3 = (from p in src select p).Take(3).AsEnumerable();
            Assert.IsTrue(res3.Count() == 2, "Take failed (3).");
        }

        [TestMethod]
        public void Ordering()
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
            People p3 = new People() { ID = 3, FirstName = "Bart", LastName = "Simpson", Age = 15, IsMember = false, ShortBio = "Funny guy" };
            People p4 = new People() { ID = 4, FirstName = "Ray", LastName = "Ozzie", Age = 52, IsMember = false, ShortBio = "Chief Software Architect at Microsoft Corporation" };
            People p5 = new People() { ID = 5, FirstName = "Anders", LastName = "Hejlsberg", Age = 47, IsMember = true, ShortBio = "C# language architect" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);
            Test.Add(lst, p3);
            Test.Add(lst, p4);
            Test.Add(lst, p5);

            //
            // List source.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;

            //
            // Ascending.
            //
            var res1 = (from p in src orderby p.FirstName select p).AsEnumerable().ToArray();
            Assert.IsTrue(res1.First().FirstName == "Anders" && res1.Last().FirstName == "Ray", "Ascending order failed");

            //
            // Descending.
            //
            var res2 = (from p in src orderby p.LastName descending select p).AsEnumerable().ToArray();
            Assert.IsTrue(res2.First().LastName == "Simpson" && res2.Last().LastName == "De Smet", "Descending order failed");

            //
            // Multiple.
            //
            var res3 = (from p in src orderby p.Age ascending, p.FirstName descending select p).AsEnumerable().ToArray();
            Assert.IsTrue(res3.First().LastName == "Simpson" && res3.Last().LastName == "Gates", "OrderBy/ThenBy failed");
        }

        [TestMethod]
        public void AndOr()
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
            People p3 = new People() { ID = 3, FirstName = "Bart", LastName = "Simpson", Age = 15, IsMember = false, ShortBio = "Funny guy" };
            People p4 = new People() { ID = 4, FirstName = "Ray", LastName = "Ozzie", Age = 52, IsMember = false, ShortBio = "Chief Software Architect at Microsoft Corporation" };
            People p5 = new People() { ID = 5, FirstName = "Anders", LastName = "Hejlsberg", Age = 47, IsMember = true, ShortBio = "C# language architect" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);
            Test.Add(lst, p3);
            Test.Add(lst, p4);
            Test.Add(lst, p5);

            //
            // List source.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;

            //
            // And.
            //
            AssertWhere(src, p => p.FirstName == "Bart" && p.Age >= 24, 1, "And failed (1).");
            AssertWhere(src, p => p.ShortBio.Contains("Microsoft") && p.Age == 52, 2, "And failed (2).");
            AssertWhere(src, p => p.FirstName.StartsWith("B") && p.IsMember && p.ShortBio.Contains("founder"), 1, "And failed (3).");

            //
            // Or.
            //
            AssertWhere(src, p => p.Age == 24 || p.Age == 52, 3, "Or failed (1).");
            AssertWhere(src, p => p.Age == 52 || p.FirstName == "Bart", 4, "Or failed (2).");
            AssertWhere(src, p => p.Age >= 40 || p.Age <= 20 || p.Age == 24, 5, "Or failed (3).");

            //
            // Mixed.
            //
            AssertWhere(src, p => p.FirstName.StartsWith("B") && (p.Age == 24 || p.Age == 52), 2, "And/Or failed (1).");
            AssertWhere(src, p => p.FirstName.StartsWith("B") || (p.FirstName == "Anders" && p.Age >= 40), 4, "And/Or failed (2).");

            //
            // De Morgan.
            //
            AssertWhere(src, p => !(p.FirstName == "Bart" && p.Age <= 24), 3, "De Morgan failed (1).");
            AssertWhere(src, p => !(p.FirstName == "Bill" || p.LastName == "De Smet"), 3, "De Morgan failed (2).");
        }

        [TestMethod]
        public void BooleanOptimizations()
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
            People p3 = new People() { ID = 3, FirstName = "Bart", LastName = "Simpson", Age = 15, IsMember = false, ShortBio = "Funny guy" };
            People p4 = new People() { ID = 4, FirstName = "Ray", LastName = "Ozzie", Age = 52, IsMember = false, ShortBio = "Chief Software Architect at Microsoft Corporation" };
            People p5 = new People() { ID = 5, FirstName = "Anders", LastName = "Hejlsberg", Age = 47, IsMember = true, ShortBio = "C# language architect" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);
            Test.Add(lst, p3);
            Test.Add(lst, p4);
            Test.Add(lst, p5);

            //
            // List source.
            //
            SharePointDataSource<People> src = new SharePointDataSource<People>(site);
            src.CheckListVersion = false;

            //
            // Optimize.
            //
            AssertWhere(src, p => 1 == 1, 5, "Boolean optimization failed (1).");
            AssertWhere(src, p => 1 == 0, 0, "Boolean optimization failed (2).");
            AssertWhere(src, p => p.FirstName == "Bart" && 1 == 1, 2, "Boolean optimization failed (3).");
            AssertWhere(src, p => p.FirstName == "Bart" && 1 == 0, 0, "Boolean optimization failed (4).");
            AssertWhere(src, p => p.FirstName == "Bart" || 1 == 1, 5, "Boolean optimization failed (5).");
            AssertWhere(src, p => p.FirstName == "Bart" || 1 == 0, 2, "Boolean optimization failed (6).");
        }

        [TestMethod]
        public void MultiWhere()
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
            // Multipe where clauses.
            //
            var res1 = (from p in src where p.Age >= 24 where p.FirstName == "Bart" select p).AsEnumerable();
            Assert.IsTrue(res1.Count() == 1 && res1.First().FirstName == "Bart", "Invalid result for multiple where clauses (1).");

            var res2 = (from p in src where p.FirstName == "Bart" where p.Age >= 24 select p).AsEnumerable();
            Assert.IsTrue(res2.Count() == 1 && res2.First().FirstName == "Bart", "Invalid result for multiple where clauses (2).");

            var res3 = (from p in src where 1 == 1 where p.FirstName == "Bart" select p).AsEnumerable();
            Assert.IsTrue(res3.Count() == 1 && res3.First().FirstName == "Bart", "Invalid result for multiple where clauses (3).");

            var res4 = (from p in src where p.FirstName == "Bart" where 1 == 1 select p).AsEnumerable();
            Assert.IsTrue(res4.Count() == 1 && res4.First().FirstName == "Bart", "Invalid result for multiple where clauses (4).");

            var res5 = (from p in src where 1 == 0 where p.FirstName == "Bart" select p).AsEnumerable();
            Assert.IsTrue(res5.Count() == 0, "Invalid result for multiple where clauses (5).");

            var res6 = (from p in src where p.FirstName == "Bart" where 1 == 0 select p).AsEnumerable();
            Assert.IsTrue(res6.Count() == 0, "Invalid result for multiple where clauses (6).");
        }

        [TestMethod]
        public void Choice()
        {
            //
            // Create list.
            //
            SPList lst = Test.CreateList<ChoiceTest>(site.RootWeb);

            //
            // Add fields.
            //
            lst.Fields.Add("Options", SPFieldType.Choice, true);
            lst.Update();
            SPFieldChoice fld = new SPFieldChoice(lst.Fields, "Options");
            SPFieldLookup l;
            fld.Choices.Add("A");
            fld.Choices.Add("B");
            fld.Choices.Add("C & D");
            fld.Update();
            lst.Update();

            //
            // Add items.
            //
            SPListItem item = lst.Items.Add();
            item["Title"] = "1";
            item["Options"] = "A";
            item.Update();
            item = lst.Items.Add();
            item["Title"] = "2";
            item["Options"] = "C & D";
            item.Update();

            //
            // List source.
            //
            SharePointDataSource<ChoiceTest> src = new SharePointDataSource<ChoiceTest>(site);
            src.CheckListVersion = false;

            //
            // Queries.
            //
            var res1 = (from c in src where c.Options == Options.A select c).AsEnumerable();
            Assert.IsTrue(res1.Count() == 1 && res1.First().Title == "1", "Test for Choice fields failed (1).");
            var res2 = (from c in src where c.Options == Options.CD select c).AsEnumerable();
            Assert.IsTrue(res2.Count() == 1 && res2.First().Title == "2", "Test for Choice fields failed (2).");
            var res3 = (from c in src where c.Options == Options.A || c.Options == Options.CD select c).AsEnumerable();
            Assert.IsTrue(res3.Count() == 2, "Test for Choice fields failed (3).");
        }

        [TestMethod]
        public void MultiChoice()
        {
            //
            // Create list.
            //
            SPList lst = Test.CreateList<ChoiceTest2>(site.RootWeb);

            //
            // Add fields.
            //
            lst.Fields.Add("Options", SPFieldType.MultiChoice, true);
            lst.Update();
            SPFieldMultiChoice fld = new SPFieldMultiChoice(lst.Fields, "Options");
            fld.Choices.Add("A");
            fld.Choices.Add("B");
            fld.Choices.Add("C & D");
            fld.Update();
            lst.Update();

            //
            // Add items.
            //
            SPListItem item = lst.Items.Add();
            item["Title"] = "1";
            item["Options"] = "A";
            item.Update();
            item = lst.Items.Add();
            item["Title"] = "2";
            item["Options"] = "C & D";
            item.Update();
            item = lst.Items.Add();
            item["Title"] = "3";
            item["Options"] = "A;#C & D";
            item.Update();
            item = lst.Items.Add();
            item["Title"] = "4";
            item["Options"] = "B";
            item.Update();

            //
            // List source.
            //
            SharePointDataSource<ChoiceTest2> src = new SharePointDataSource<ChoiceTest2>(site);
            src.CheckListVersion = false;

            //
            // Queries.
            //
            var res1 = (from c in src where c.Options == Options2.A select c).AsEnumerable();
            Assert.IsTrue(res1.Count() == 2 && res1.First().Title == "1" && res1.Last().Title == "3", "Test for MultiChoice fields failed (1).");
            var res2 = (from c in src where c.Options == Options2.CD select c).AsEnumerable();
            Assert.IsTrue(res2.Count() == 2 && res2.First().Title == "2" && res2.Last().Title == "3", "Test for MultiChoice fields failed (2).");
            var res3 = (from c in src where c.Options == (Options2.A | Options2.B) select c).AsEnumerable();
            Assert.IsTrue(res3.Count() == 0, "Test for MultiChoice fields failed (3).");
            var res4 = (from c in src where c.Options == Options2.A || c.Options == Options2.B select c).AsEnumerable();
            Assert.IsTrue(res4.Count() == 3, "Test for MultiChoice fields failed (4).");
            var res5 = (from c in src where c.Options != Options2.A select c).AsEnumerable();
            Assert.IsTrue(res5.Count() == 2, "Test for MultiChoice fields failed (5).");
        }

        [TestMethod]
        public void LookupSubquery()
        {
            //
            // Get lists with sample data.
            //
            SPList child, parent;
            GetLookupLists(out child, out parent);

            //
            // Parent source.
            //
            SharePointDataSource<LookupParent> src = new SharePointDataSource<LookupParent>(site);
            src.CheckListVersion = false;

            //
            // Subqueries.
            //
            var res1 = (from p in src where p.Child.Number >= 2 select p).AsEnumerable();
            Assert.IsTrue(res1.Count() == 4 && res1.First().Title == "Parent 21" && res1.Last().Title == "Parent 32", "LookupSubquery test failed (1).");
            var res2 = (from p in src where p.Child.Number < 3 && p.Child.Title.Contains("ld 2") && p.Title.Contains("22") select p).AsEnumerable();
            Assert.IsTrue(res2.Count() == 1 && res2.First().Title == "Parent 22", "LookupSubquery test failed (2).");
            var res3 = (from p in src where p.Title.Contains("22") && p.Child.Number < 3 && p.Child.Title.Contains("ld 2") select p).AsEnumerable();
            Assert.IsTrue(res3.Count() == 1 && res3.First().Title == "Parent 22", "LookupSubquery test failed (3).");
            var res4 = (from p in src where p.Child.Number == 5 select p).AsEnumerable();
            Assert.IsTrue(res4.Count() == 0, "LookupSubquery test failed (4).");
        }

        [TestMethod]
        public void LookupLazyLoad()
        {
            //
            // Get lists with sample data.
            //
            SPList child, parent;
            GetLookupLists(out child, out parent);

            //
            // Parent source.
            //
            SharePointDataSource<LookupParent> src = new SharePointDataSource<LookupParent>(site);
            src.CheckListVersion = false;

            //
            // Subqueries.
            //
            var res1 = (from p in src where p.Title == "Parent 22" select p).AsEnumerable();
            Assert.IsTrue(res1.Count() == 1 && res1.First().Child.ID == 2, "LookupLazyLoad test failed (1).");
        }

        private void GetLookupLists(out SPList child, out SPList parent)
        {
            //
            // Child list.
            //
            child = Test.Create<LookupChild>(site.RootWeb);

            //
            // Parent list.
            //
            parent = Test.CreateList<LookupParent>(site.RootWeb);
            parent.Fields.AddLookup("Child", child.ID, false);
            parent.Update();
            SPFieldLookup lookup = new SPFieldLookup(parent.Fields, "Child");
            lookup.LookupField = "Title";
            lookup.Update();
            parent.Update();

            //
            // Add child items.
            //
            SPListItem c = child.Items.Add();
            c["Title"] = "Child 1";
            c["Number"] = 1;
            c.Update();
            c = child.Items.Add();
            c["Title"] = "Child 2";
            c["Number"] = 2;
            c.Update();
            c = child.Items.Add();
            c["Title"] = "Child 3";
            c["Number"] = 3;
            c.Update();

            //
            // Add parent items.
            //
            SPListItem p = parent.Items.Add();
            p["Title"] = "Parent 11";
            p["Child"] = "1;#Child 1";
            p.Update();
            p = parent.Items.Add();
            p["Title"] = "Parent 12";
            p["Child"] = "1;#Child 1";
            p.Update();
            p = parent.Items.Add();
            p["Title"] = "Parent 21";
            p["Child"] = "2;#Child 2";
            p.Update();
            p = parent.Items.Add();
            p["Title"] = "Parent 22";
            p["Child"] = "2;#Child 2";
            p.Update();
            p = parent.Items.Add();
            p["Title"] = "Parent 31";
            p["Child"] = "3;#Child 3";
            p.Update();
            p = parent.Items.Add();
            p["Title"] = "Parent 32";
            p["Child"] = "3;#Child 3";
            p.Update();
        }

        [TestMethod]
        public void Junk()
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
            // Test.
            //
            //var res = (from p in src select p.Age).First(); //24.0
            //var res = (from p in src where p.Age > 100 select p.Age).First(); //empty sequence
            //var res = (from p in src select p.Age).First(p => p > 50); //52.0
            //var res = (from p in src select p).First(); //Bart

            /*
             * TODO: coalescing of two or more adjacent where clauses; last where clause can't be translated in this case
             * 
             * var res = from p in src where p.Age == 24 where p.FirstName == "Bart" orderby p.LastName where p.IsMember select p;
             */
        }

        private static void AssertWhere<T>(SharePointDataSource<T> src, Expression<Func<T, bool>> predicate, int expectedCount, string message)
        {
            IEnumerable<T> res = src.Where<T>(predicate).Select(e => e).AsEnumerable();
            Assert.IsTrue(res.Count() == expectedCount && res.All(predicate.Compile()), message);
        }

        private static void AssertProject<T, R>(SharePointDataSource<T> src, Expression<Func<T, R>> selector, IEnumerable<R> results, string message)
        {
            IEnumerable<R> res = src.Select(selector).AsEnumerable();
            Assert.IsTrue(res.SequenceEqual(results));
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
