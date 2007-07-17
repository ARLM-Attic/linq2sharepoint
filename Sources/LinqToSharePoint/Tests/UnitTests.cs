/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

#region Namespace imports

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
using System.Net;

#endregion

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
        private Uri url = new Uri("http://wss3demo");

        public UnitTests()
        {
            site = new SPSite(url.ToString());
            web = site.RootWeb;
        }

        private SharePointDataContext GetSpContext()
        {
            SharePointDataContext ctx = new SharePointDataContext(site);
            ctx.CheckListVersion = false;
            return ctx;
        }

        private SharePointDataContext GetWsContext()
        {
            SharePointDataContext ctx = new SharePointDataContext(url);
            ctx.CheckListVersion = false;
            ctx.Credentials = CredentialCache.DefaultNetworkCredentials;
            return ctx;
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
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Empty list test.
            //
            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;
                var res = (from p in src select p).AsEnumerable();

                Assert.IsTrue(res.Count() == 0, "Query on empty list did return results " + ctx.Name);
            }
        }

        [TestMethod]
        public void DefaultQuery()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);

            //
            // Test default query.
            //
            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;
                var res = (from p in src select p).AsEnumerable();

                Assert.IsTrue(res.Count() == 1, "Query did not return results " + ctx.Name);
            }
        }

        [TestMethod]
        public void First()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            People p2 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // Test default query.
            //
            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;
                var res = (from p in src orderby p.FirstName select p).First();

                Assert.IsTrue(res != null && res.FirstName == "Bart", "Wrong result; source contains an item " + ctx.Name);
            }
        }

        [TestMethod]
        public void FirstOrDefault()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            People p2 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // Test default query.
            //
            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;
                var res = (from p in src where p.Age == 25 select p).FirstOrDefault();

                Assert.IsTrue(res == null, "Query should return null " + ctx.Name);
            }
        }

        [TestMethod]
        public void GetEntityById()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);

            //
            // Get entity by id.
            //
            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;

                People _p1 = src.GetEntityById(1);
                Assert.IsTrue(_p1.FirstName == p1.FirstName && _p1.LastName == p1.LastName && _p1.Age == p1.Age && _p1.IsMember == p1.IsMember && _p1.ShortBiography == p1.ShortBiography, "Invalid entity returned by GetEntityById method " + ctx.Name);
            }
        }

        [TestMethod]
        public void EnsureEntityIdentity()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);

            //
            // Get entity twice.
            //
            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;
                var res1 = (from p in src where p.Age == 24 select p).First();
                var res2 = (from p in src where p.IsMember select p).First();

                //
                // Should refer to the same object.
                //
                Assert.IsTrue(object.ReferenceEquals(res1, res2), "Entity identity check failed (EnsureEntityIdentity) " + ctx.Name);
            }
        }

        [TestMethod]
        public void StringEquality()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // List source.
            //
            //var src = new SharePointList<People>(spContext);

            //
            // Using ==
            //
            AssertWhere<People>(p => p.FirstName == "Bart", 1, "Equality check failed (==)");
            AssertWhere<People>(p => p.FirstName != "Bart", 1, "Inequality check failed (!=)");

            //
            // Using .Equals
            //
            AssertWhere<People>(p => p.FirstName.Equals("Bart"), 1, "Equality check failed (.Equals)");
            AssertWhere<People>(p => !p.FirstName.Equals("Bart"), 1, "Inequality check failed (!.Equals)");

            //
            // Using == inverse order
            //
            AssertWhere<People>(p => "Bart" == p.FirstName, 1, "Equality check failed (== inverse order)");
            AssertWhere<People>(p => "Bart" != p.FirstName, 1, "Inquality check failed (!= inverse order)");

            //
            // Using .Equals inverse order (NOT SUPPORTED)
            //
            //AssertWhere(src, p => "Bart".Equals(p.FirstName), 1, "Equality check failed (.Equals inverse order)");

            //
            // Using == with .ToString
            //
            AssertWhere<People>(p => p.FirstName.ToString() == "Bart".ToString(), 1, "Equality check failed (== with .ToString)");

            //
            // Using .Equals with .ToString
            //
            AssertWhere<People>(p => p.FirstName.ToString().Equals("Bart".ToString()), 1, "Equality check failed (.Equals with .ToString)");

            //
            // Using == with .ToString
            //
            AssertWhere<People>(p => p.FirstName.ToString() == "Bart".ToString(), 1, "Equality check failed (== with .ToString inverse order)");

            //
            // Using .Equals with .ToString
            //
            AssertWhere<People>(p => p.FirstName.ToString().Equals("Bart".ToString()), 1, "Equality check failed (.Equals with .ToString inverse order)");
        }

        [TestMethod]
        public void LtLeqGtGeq()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // Gt
            //
            AssertWhere<People>(p => p.Age > 50, 1, "Invalid Gt result");
            AssertWhere<People>(p => 50 < p.Age, 1, "Invalid Gt result (inverse)");
            AssertWhere<People>(p => !(p.Age <= 50), 1, "Invalid Gt result (negated)");
            AssertWhere<People>(p => !(50 >= p.Age), 1, "Invalid Gt result (negated) (inverse)");

            //
            // Geq
            //
            AssertWhere<People>(p => p.Age >= 52, 1, "Invalid Geq result");
            AssertWhere<People>(p => 52 <= p.Age, 1, "Invalid Geq result (inverse)");
            AssertWhere<People>(p => !(p.Age < 52), 1, "Invalid Geq result (negated)");
            AssertWhere<People>(p => !(52 > p.Age), 1, "Invalid Geq result (negated) (inverse)");

            //
            // Lt
            //
            AssertWhere<People>(p => p.Age < 25, 1, "Invalid Lt result");
            AssertWhere<People>(p => 25 > p.Age, 1, "Invalid Lt result (inverse)");
            AssertWhere<People>(p => !(p.Age >= 25), 1, "Invalid Lt result (negated)");
            AssertWhere<People>(p => !(25 <= p.Age), 1, "Invalid Lt result (negated) (inverse)");

            //
            // Leq
            //
            AssertWhere<People>(p => p.Age <= 24, 1, "Invalid Leq result");
            AssertWhere<People>(p => 24 >= p.Age, 1, "Invalid Leq result (inverse)");
            AssertWhere<People>(p => !(p.Age > 24), 1, "Invalid Leq result (negated)");
            AssertWhere<People>(p => !(24 < p.Age), 1, "Invalid Leq result (negated) (inverse)");
        }

        [TestMethod]
        public void BoolPredicate()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // Test queries.
            //
            AssertWhere<People>(p => p.IsMember, 1, "Invalid result for Boolean check (true)");
            AssertWhere<People>(p => !p.IsMember, 1, "Invalid result for Boolean check (false)");
        }

        [TestMethod]
        public void Nullable()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, SecondAge = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, SecondAge = null, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            People p3 = new People() { FirstName = "William", LastName = "Gates", Age = 52, SecondAge = null, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);
            Test.Add(lst, p3);

            //
            // Test queries.
            //
            AssertWhere<People>(p => p.SecondAge.HasValue, 1, "Invalid result for nullable query (1)");
            AssertWhere<People>(p => !p.SecondAge.HasValue, 2, "Invalid result for nullable query (2)");
            AssertWhere<People>(p => p.SecondAge != null, 1, "Invalid result for nullable query (3)");
            AssertWhere<People>(p => p.SecondAge == null, 2, "Invalid result for nullable query (4)");
            AssertWhere<People>(p => p.SecondAge == 24, 1, "Invalid result for nullable query (5)");
            AssertWhere<People>(p => p.SecondAge.Value == 24, 1, "Invalid result for nullable query (6)");
            AssertWhere<People>(p => p.SecondAge >= 25, 0, "Invalid result for nullable query (7)");
        }

        [TestMethod]
        public void StartsWith()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // StartsWith empty string.
            //
            AssertWhere<People>(p => p.LastName.StartsWith(""), 2, "StartsWith with empty string failed");

            //
            // StartsWith with valid parameter.
            //
            AssertWhere<People>(p => p.LastName.StartsWith("De"), 1, "StartsWith with valid parameter failed");

            //
            // StartsWith without results.
            //
            AssertWhere<People>(p => p.FirstName.StartsWith("C"), 0, "Empty StartsWith failed");
        }

        [TestMethod]
        public void Contains()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // Contains empty string.
            //
            AssertWhere<People>(p => p.ShortBiography.Contains(""), 2, "Contains with empty string failed");

            //
            // Contains with valid parameter.
            //
            AssertWhere<People>(p => p.ShortBiography.Contains("Corp"), 1, "Contains with valid parameter failed");

            //
            // Contains without results.
            //
            AssertWhere<People>(p => p.ShortBiography.Contains("azerty"), 0, "Empty Contains failed");
        }

        [TestMethod]
        public void NullChecks()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = null, LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // Test queries.
            //
            AssertWhere<People>(p => p.FirstName == null, 1, "Invalid IsNull result");
            AssertWhere<People>(p => null == p.FirstName, 1, "Invalid IsNull result (inverse)");

            AssertWhere<People>(p => p.FirstName != null, 1, "Invalid IsNotNull result");
            AssertWhere<People>(p => null != p.FirstName, 1, "Invalid IsNotNull result (inverse)");

            AssertWhere<People>(p => !(p.FirstName == null), 1, "Invalid IsNotNull result 2");
            AssertWhere<People>(p => !(null == p.FirstName), 1, "Invalid IsNotNull result 2 (inverse)");

            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;

                var res1 = (from p in src where p.FirstName.Equals(null) select p).AsEnumerable();
                Assert.IsTrue(res1.Count() == 1 && res1.First().ID == 1, "Invalid IsNull result (.Equals) " + ctx.Name);
                var res2 = (from p in src where !p.FirstName.Equals(null) select p).AsEnumerable();
                Assert.IsTrue(res2.Count() == 1 && res2.First().ID == 2, "Invalid IsNotNull result (.Equals) " + ctx.Name);
            }

            //
            // Use of variable.
            //
            string s = null;
            AssertWhere<People>(p => p.FirstName == s, 1, "Invalid IsNull result (variable)");
            AssertWhere<People>(p => s == p.FirstName, 1, "Invalid IsNull result (variable) (inverse)");

            AssertWhere<People>(p => p.FirstName != s, 1, "Invalid IsNotNull result (variable)");
            AssertWhere<People>(p => s != p.FirstName, 1, "Invalid IsNotNull result (variable) (inverse)");
        }

        [TestMethod]
        public void ProjectSingleton()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // Projections.
            //
            AssertProject<People, double>(p => p.Age, new double[] { 24, 52 }, "Integer projection failure");
            AssertProject<People, string>(p => p.FirstName + " " + p.LastName, new string[] { "Bart De Smet", "Bill Gates" }, "String projection failure");
        }

        [TestMethod]
        public void ComplexProjection()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;

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
                                      CharactersInBio = Helper(p.ShortBiography)
                                  }).AsEnumerable().First();
                Assert.IsTrue(res.FirstName == "Bart" && res.Ages[0] == 24 && res.Ages2[0] == 24 && res.DoubleAge == 48 && res.Member == true && res.NoMember == false && res.NameLower == "bart de smet" && res.LastName == "De Smet" && res.CharactersInBio == 15);
            }
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
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            //
            // Take operation.
            //
            AssertTake<People>(1, 1, "Take failed (1).");
            AssertTake<People>(0, 0, "Take failed (2).");
            AssertTake<People>(3, 2, "Take failed (3).");
        }

        [TestMethod]
        public void Ordering()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            People p3 = new People() { FirstName = "Bart", LastName = "Simpson", Age = 15, IsMember = false, ShortBiography = "Funny guy" };
            People p4 = new People() { FirstName = "Ray", LastName = "Ozzie", Age = 52, IsMember = false, ShortBiography = "Chief Software Architect at Microsoft Corporation" };
            People p5 = new People() { FirstName = "Anders", LastName = "Hejlsberg", Age = 47, IsMember = true, ShortBiography = "C# language architect" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);
            Test.Add(lst, p3);
            Test.Add(lst, p4);
            Test.Add(lst, p5);

            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;

                //
                // Ascending.
                //
                var res1 = (from p in src orderby p.FirstName select p).AsEnumerable().ToArray();
                Assert.IsTrue(res1.First().FirstName == "Anders" && res1.Last().FirstName == "Ray", "Ascending order failed " + ctx.Name);

                //
                // Descending.
                //
                var res2 = (from p in src orderby p.LastName descending select p).AsEnumerable().ToArray();
                Assert.IsTrue(res2.First().LastName == "Simpson" && res2.Last().LastName == "De Smet", "Descending order failed " + ctx.Name);

                //
                // Multiple.
                //
                var res3 = (from p in src orderby p.Age ascending, p.FirstName descending select p).AsEnumerable().ToArray();
                Assert.IsTrue(res3.First().LastName == "Simpson" && res3.Last().LastName == "Gates", "OrderBy/ThenBy failed " + ctx.Name);
            }
        }

        [TestMethod]
        public void AndOr()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            People p3 = new People() { FirstName = "Bart", LastName = "Simpson", Age = 15, IsMember = false, ShortBiography = "Funny guy" };
            People p4 = new People() { FirstName = "Ray", LastName = "Ozzie", Age = 52, IsMember = false, ShortBiography = "Chief Software Architect at Microsoft Corporation" };
            People p5 = new People() { FirstName = "Anders", LastName = "Hejlsberg", Age = 47, IsMember = true, ShortBiography = "C# language architect" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);
            Test.Add(lst, p3);
            Test.Add(lst, p4);
            Test.Add(lst, p5);

            //
            // And.
            //
            AssertWhere<People>(p => p.FirstName == "Bart" && p.Age >= 24, 1, "And failed (1).");
            AssertWhere<People>(p => p.ShortBiography.Contains("Microsoft") && p.Age == 52, 2, "And failed (2).");
            AssertWhere<People>(p => p.FirstName.StartsWith("B") && p.IsMember && p.ShortBiography.Contains("founder"), 1, "And failed (3).");

            //
            // Or.
            //
            AssertWhere<People>(p => p.Age == 24 || p.Age == 52, 3, "Or failed (1).");
            AssertWhere<People>(p => p.Age == 52 || p.FirstName == "Bart", 4, "Or failed (2).");
            AssertWhere<People>(p => p.Age >= 40 || p.Age <= 20 || p.Age == 24, 5, "Or failed (3).");

            //
            // Mixed.
            //
            AssertWhere<People>(p => p.FirstName.StartsWith("B") && (p.Age == 24 || p.Age == 52), 2, "And/Or failed (1).");
            AssertWhere<People>(p => p.FirstName.StartsWith("B") || (p.FirstName == "Anders" && p.Age >= 40), 4, "And/Or failed (2).");

            //
            // De Morgan.
            //
            AssertWhere<People>(p => !(p.FirstName == "Bart" && p.Age <= 24), 3, "De Morgan failed (1).");
            AssertWhere<People>(p => !(p.FirstName == "Bill" || p.LastName == "De Smet"), 3, "De Morgan failed (2).");
        }

        [TestMethod]
        public void BooleanOptimizations()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            People p3 = new People() { FirstName = "Bart", LastName = "Simpson", Age = 15, IsMember = false, ShortBiography = "Funny guy" };
            People p4 = new People() { FirstName = "Ray", LastName = "Ozzie", Age = 52, IsMember = false, ShortBiography = "Chief Software Architect at Microsoft Corporation" };
            People p5 = new People() { FirstName = "Anders", LastName = "Hejlsberg", Age = 47, IsMember = true, ShortBiography = "C# language architect" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);
            Test.Add(lst, p3);
            Test.Add(lst, p4);
            Test.Add(lst, p5);

            //
            // Optimize.
            //
            AssertWhere<People>(p => 1 == 1, 5, "Boolean optimization failed (1).");
            AssertWhere<People>(p => 1 == 0, 0, "Boolean optimization failed (2).");
            AssertWhere<People>(p => p.FirstName == "Bart" && 1 == 1, 2, "Boolean optimization failed (3).");
            AssertWhere<People>(p => p.FirstName == "Bart" && 1 == 0, 0, "Boolean optimization failed (4).");
            AssertWhere<People>(p => p.FirstName == "Bart" || 1 == 1, 5, "Boolean optimization failed (5).");
            AssertWhere<People>(p => p.FirstName == "Bart" || 1 == 0, 2, "Boolean optimization failed (6).");
        }

        [TestMethod]
        public void MultiWhere()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

            foreach (var ctx in GetContexts<People>())
            {
                //
                // List source.
                //
                var src = ctx.List;

                //
                // Multipe where clauses.
                //
                var res1 = (from p in src where p.Age >= 24 where p.FirstName == "Bart" select p).AsEnumerable();
                Assert.IsTrue(res1.Count() == 1 && res1.First().FirstName == "Bart", "Invalid result for multiple where clauses (1) " + ctx.Name + ".");

                var res2 = (from p in src where p.FirstName == "Bart" where p.Age >= 24 select p).AsEnumerable();
                Assert.IsTrue(res2.Count() == 1 && res2.First().FirstName == "Bart", "Invalid result for multiple where clauses (2) " + ctx.Name + ".");

                var res3 = (from p in src where 1 == 1 where p.FirstName == "Bart" select p).AsEnumerable();
                Assert.IsTrue(res3.Count() == 1 && res3.First().FirstName == "Bart", "Invalid result for multiple where clauses (3) " + ctx.Name + ".");

                var res4 = (from p in src where p.FirstName == "Bart" where 1 == 1 select p).AsEnumerable();
                Assert.IsTrue(res4.Count() == 1 && res4.First().FirstName == "Bart", "Invalid result for multiple where clauses (4) " + ctx.Name + ".");

                var res5 = (from p in src where 1 == 0 where p.FirstName == "Bart" select p).AsEnumerable();
                Assert.IsTrue(res5.Count() == 0, "Invalid result for multiple where clauses (5) " + ctx.Name + ".");

                var res6 = (from p in src where p.FirstName == "Bart" where 1 == 0 select p).AsEnumerable();
                Assert.IsTrue(res6.Count() == 0, "Invalid result for multiple where clauses (6) " + ctx.Name + ".");
            }
        }

        private List<Context<T>> GetContexts<T>() where T : class
        {
            return new List<Context<T>>() { 
                       new Context<T>() { Name = "[SP]", List = new SharePointList<T>(GetSpContext()) },
                       new Context<T>() { Name = "[WS]", List = new SharePointList<T>(GetWsContext()) }
                   };
        }

        class Context<T> where T : class
        {
            public SharePointList<T> List { get; set; }
            public string Name { get; set; }
        }

        [TestMethod]
        public void Choice()
        {
            //
            // Create list.
            //
            var lst = Test.CreateList<ChoiceTest>(site.RootWeb);

            //
            // Add fields.
            //
            lst.Fields.Add("Options", SPFieldType.Choice, true);
            lst.Update();
            SPFieldChoice fld = new SPFieldChoice(lst.Fields, "Options");
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

            foreach (var ctx in GetContexts<ChoiceTest>())
            {
                var src = ctx.List;

                //
                // Queries.
                //
                var res1 = (from c in src where c.Options == Options.A select c).AsEnumerable();
                Assert.IsTrue(res1.Count() == 1 && res1.First().Title == "1", "Test for Choice fields failed (1) " + ctx.Name);
                var res2 = (from c in src where c.Options == Options.CD select c).AsEnumerable();
                Assert.IsTrue(res2.Count() == 1 && res2.First().Title == "2", "Test for Choice fields failed (2) " + ctx.Name);
                var res3 = (from c in src where c.Options == Options.A || c.Options == Options.CD select c).AsEnumerable();
                Assert.IsTrue(res3.Count() == 2, "Test for Choice fields failed (3) " + ctx.Name);
            }
        }

        [TestMethod]
        public void MultiChoice()
        {
            //
            // Create list.
            //
            var lst = Test.CreateList<ChoiceTest2>(site.RootWeb);

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

            foreach (var ctx in GetContexts<ChoiceTest2>())
            {
                var src = ctx.List;

                //
                // Queries.
                //
                var res1 = (from c in src where c.Options == Options2.A select c).AsEnumerable();
                Assert.IsTrue(res1.Count() == 2 && res1.First().Title == "1" && res1.Last().Title == "3", "Test for MultiChoice fields failed (1) " + ctx.Name);
                var res2 = (from c in src where c.Options == Options2.CD select c).AsEnumerable();
                Assert.IsTrue(res2.Count() == 2 && res2.First().Title == "2" && res2.Last().Title == "3", "Test for MultiChoice fields failed (2) " + ctx.Name);
                var res3 = (from c in src where c.Options == (Options2.A | Options2.B) select c).AsEnumerable();
                Assert.IsTrue(res3.Count() == 0, "Test for MultiChoice fields failed (3) " + ctx.Name);
                var res4 = (from c in src where c.Options == Options2.A || c.Options == Options2.B select c).AsEnumerable();
                Assert.IsTrue(res4.Count() == 3, "Test for MultiChoice fields failed (4) " + ctx.Name);
                var res5 = (from c in src where c.Options != Options2.A select c).AsEnumerable();
                Assert.IsTrue(res5.Count() == 2, "Test for MultiChoice fields failed (5) " + ctx.Name);
            }
        }

        [TestMethod]
        public void MultiChoiceWithFillIn()
        {
            //
            // Create list.
            //
            var lst = Test.CreateList<ChoiceTest3>(site.RootWeb);

            //
            // Add fields.
            //
            lst.Fields.Add("Options", SPFieldType.MultiChoice, true);
            lst.Update();
            SPFieldMultiChoice fld = new SPFieldMultiChoice(lst.Fields, "Options");
            fld.FillInChoice = true;
            fld.Choices.Add("A");
            fld.Choices.Add("B");
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
            item["Options"] = "C";
            item.Update();
            item = lst.Items.Add();
            item["Title"] = "3";
            item["Options"] = "A;#C";
            item.Update();

            foreach (var ctx in GetContexts<ChoiceTest3>())
            {
                var src = ctx.List;

                //
                // Queries.
                //
                var res1 = (from c in src where c.Options == Options3.A select c).AsEnumerable();
                Assert.IsTrue(res1.Count() == 2 && res1.First().Title == "1" && res1.Last().Title == "3", "Test for MultiChoiceWithFillIn fields failed (1) " + ctx.Name);
                var res2 = (from c in src where c.OptionsOther == "C" select c).AsEnumerable();
                Assert.IsTrue(res2.Count() == 2 && res2.First().Title == "2" && res2.Last().Title == "3" && res2.Last().OptionsOther == "C", "Test for MultiChoiceWithFillIn fields failed (2) " + ctx.Name);
                var res3 = (from c in src where c.Options == Options3.A && c.OptionsOther == "C" select c).AsEnumerable();
                Assert.IsTrue(res3.Count() == 1 && res3.First().Title == "3", "Test for MultiChoiceWithFillIn fields failed (3) " + ctx.Name);
            }
        }

        [TestMethod]
        public void ConstantBooleanPredicate()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);

            //
            // List source (context doesn't matter).
            //
            var src = new SharePointList<People>(GetSpContext());

            //
            // Query.
            //
            int a = 123;
            var res = (from p in src where a == 123 select p).AsEnumerable();
            Assert.IsTrue(res.Count() == 1, "Test for ConstantBooleanPredicate failed.");
        }

        [TestMethod]
        public void LookupSubquery()
        {
            //
            // Get lists with sample data.
            //
            SPList child, parent;
            GetLookupLists(out child, out parent);

            foreach (var ctx in GetContexts<LookupParent>())
            {
                var src = ctx.List;

                //
                // Subqueries.
                //
                var res1 = (from p in src where p.Child.Number >= 2 select p).AsEnumerable();
                Assert.IsTrue(res1.Count() == 4 && res1.First().Title == "Parent 21" && res1.Last().Title == "Parent 32", "LookupSubquery test failed (1) " + ctx.Name);
                var res2 = (from p in src where p.Child.Number < 3 && p.Child.Title.Contains("ld 2") && p.Title.Contains("22") select p).AsEnumerable();
                Assert.IsTrue(res2.Count() == 1 && res2.First().Title == "Parent 22", "LookupSubquery test failed (2) " + ctx.Name);
                var res3 = (from p in src where p.Title.Contains("22") && p.Child.Number < 3 && p.Child.Title.Contains("ld 2") select p).AsEnumerable();
                Assert.IsTrue(res3.Count() == 1 && res3.First().Title == "Parent 22", "LookupSubquery test failed (3) " + ctx.Name);
                var res4 = (from p in src where p.Child.Number == 5 select p).AsEnumerable();
                Assert.IsTrue(res4.Count() == 0, "LookupSubquery test failed (4) " + ctx.Name);
            }
        }

        [TestMethod]
        public void LookupSubquery2()
        {
            //
            // Get lists with sample data.
            //
            SPList child, parent;
            GetLookupLists(out child, out parent);

            {
                //
                // Sources.
                //
                var ctx = GetSpContext();
                var parents = new SharePointList<LookupParent>(ctx);
                var children = new SharePointList<LookupChild>(ctx);

                //
                // Get child item.
                //
                var c1 = (from c in children where c.Number == 1 select c).First();

                //
                // Get parent items.
                //
                var res = (from p in parents where p.Child == c1 select p).AsEnumerable();
                Assert.IsTrue(res.Count() == 2 && res.First().Title == "Parent 11" && res.Last().Title == "Parent 12", "LookupSubquery2 test failed (SP).");
            }
            {
                //
                // Sources.
                //
                var ctx = GetWsContext();
                var parents = new SharePointList<LookupParent>(ctx);
                var children = new SharePointList<LookupChild>(ctx);

                //
                // Get child item.
                //
                var c1 = (from c in children where c.Number == 1 select c).First();

                //
                // Get parent items.
                //
                var res = (from p in parents where p.Child == c1 select p).AsEnumerable();
                Assert.IsTrue(res.Count() == 2 && res.First().Title == "Parent 11" && res.Last().Title == "Parent 12", "LookupSubquery2 test failed (WS).");
            }
        }

        [TestMethod]
        public void LookupLazyLoad()
        {
            //
            // Get lists with sample data.
            //
            SPList child, parent;
            GetLookupLists(out child, out parent);

            foreach (var ctx in GetContexts<LookupParent>())
            {
                var src = ctx.List;

                //
                // Subqueries.
                //
                var res1 = (from p in src where p.Title == "Parent 22" select p).AsEnumerable();
                Assert.IsTrue(res1.Count() == 1 && res1.First().Child.ID == 2, "LookupLazyLoad test failed (1) " + ctx.Name);
            }
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
        public void LookupMulti()
        {
            //
            // Get lists with sample data.
            //
            SPList child, parent;
            GetLookupMultiLists(out child, out parent);

            foreach (var ctx in GetContexts<LookupMultiParent>())
            {
                var src = ctx.List;

                var children = new SharePointList<LookupChild>(ctx.List.Context);

                //
                // Queries.
                //
                var child1 = (from c in children where c.Number == 1 select c).AsEnumerable().First();
                var res1 = (from p in src where p.Childs.Contains(child1) select p).AsEnumerable();
                Assert.IsTrue(res1.Count() == 1 && res1.First().Title == "Parent 1" , "LookupMulti test failed (1) " + ctx.Name);

                var child2 = children.GetEntityById(2);
                var res2 = (from p in src where p.Childs.Contains(child2) select p).AsEnumerable();
                Assert.IsTrue(res2.Count() == 2 && res2.First().Title == "Parent 1" && res2.Last().Title == "Parent 2", "LookupMulti test failed (2) " + ctx.Name);

                var res3 = (from p in src where p.Childs.Contains((from c in children where c.Number == 3 select c).First()) select p).AsEnumerable();
                Assert.IsTrue(res3.Count() == 3, "LookupMulti test failed (3) " + ctx.Name);

                var child4 = children.GetEntityById(4);
                var res4 = (from p in src where p.Childs.Contains(child4) select p).AsEnumerable();
                Assert.IsTrue(res4.Count() == 0, "LookupMulti test failed (4) " + ctx.Name);
            }
        }

        private void GetLookupMultiLists(out SPList child, out SPList parent)
        {
            //
            // Child list.
            //
            child = Test.Create<LookupChild>(site.RootWeb);

            //
            // Parent list.
            //
            parent = Test.CreateList<LookupMultiParent>(site.RootWeb);
            parent.Fields.AddLookup("Childs", child.ID, false);
            parent.Update();
            SPFieldLookup lookup = new SPFieldLookup(parent.Fields, "Childs");
            lookup.LookupField = "Title";
            lookup.AllowMultipleValues = true;
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
            c = child.Items.Add();
            c["Title"] = "Child 4";
            c["Number"] = 4;
            c.Update();

            //
            // Add parent items.
            //
            SPListItem p;
            SPFieldLookupValueCollection val;

            val = new SPFieldLookupValueCollection();
            val.Add(new SPFieldLookupValue(1, "Child 1"));
            val.Add(new SPFieldLookupValue(2, "Child 2"));
            val.Add(new SPFieldLookupValue(3, "Child 3"));
            p = parent.Items.Add();
            p["Title"] = "Parent 1";
            p["Childs"] = val;
            p.Update();

            val = new SPFieldLookupValueCollection();
            val.Add(new SPFieldLookupValue(2, "Child 2"));
            val.Add(new SPFieldLookupValue(3, "Child 3"));
            p = parent.Items.Add();
            p["Title"] = "Parent 2";
            p["Childs"] = val;
            p.Update();

            val = new SPFieldLookupValueCollection();
            val.Add(new SPFieldLookupValue(3, "Child 3"));
            p = parent.Items.Add();
            p["Title"] = "Parent 3";
            p["Childs"] = val;
            p.Update();
        }

        [TestMethod]
        public void DateTime()
        {
            //
            // Create list DateTimeTest.
            //
            var lst = Test.Create<DateTimeTest>(site.RootWeb);

            //
            // Add items.
            //
            System.DateTime now = System.DateTime.Now; //new System.DateTime(2007, 5, 21, 11, 11, 0);
            DateTimeTest t1 = new DateTimeTest() { Name = "Yesterday", DateTime = System.DateTime.Today.AddDays(-1) };
            DateTimeTest t2 = new DateTimeTest() { Name = "Today", DateTime = System.DateTime.Today };
            DateTimeTest t3 = new DateTimeTest() { Name = "Tomorrow", DateTime = System.DateTime.Today.AddDays(1) };
            DateTimeTest t4 = new DateTimeTest() { Name = "Now", DateTime = now };
            Test.Add(lst, t1);
            Test.Add(lst, t2);
            Test.Add(lst, t3);
            Test.Add(lst, t4);

            //
            // Queries.
            //
            foreach (var ctx in GetContexts<DateTimeTest>())
            {
                var src = ctx.List;

                var res1 = (from t in src where t.DateTime == now select t).AsEnumerable(); //KNOWN ISSUE with Eq check on hh:MM:ss (2032)
                Assert.IsTrue(res1.Count() == 2, "DateTime test failure (1) " + ctx.Name);
                var res2 = (from t in src where t.DateTime > System.DateTime.Today select t).AsEnumerable();
                Assert.IsTrue(res2.Count() == 1, "DateTime test failure (2) " + ctx.Name);
                var res3 = (from t in src where t.DateTime < System.DateTime.Today select t).AsEnumerable();
                Assert.IsTrue(res3.Count() == 1, "DateTime test failure (3) " + ctx.Name);
                var res4 = (from t in src where t.DateTime == System.DateTime.Today select t).AsEnumerable();
                Assert.IsTrue(res4.Count() == 2, "DateTime test failure (4) " + ctx.Name);
                var res5 = (from t in src where t.DateTime <= System.DateTime.Today select t).AsEnumerable();
                Assert.IsTrue(res5.Count() == 3, "DateTime test failure (5) " + ctx.Name);
            }

            //
            // TODO: Check for eligible use of Now.
            //
        }

        [TestMethod]
        public void DateRangesOverlap()
        {
            //
            // Create list DateRangesOverlapTest.
            //
            var lst = Test.Create<DateRangesOverlapTest>(site.RootWeb);

            //
            // Add items.
            //
            DateRangesOverlapTest t1 = new DateRangesOverlapTest();
            t1.Title = "Appointment";
            t1.From = System.DateTime.Now.AddDays(-1);
            t1.To = System.DateTime.Now.AddDays(1);
            Test.Add(lst, t1);

            foreach (var ctx in GetContexts<DateRangesOverlapTest>())
            {
                var src = ctx.List;

                //
                // Query.
                //
                var res1 = (from d in src where CamlMethods.DateRangesOverlap(CamlMethods.Now, d.From, d.To) select d).AsEnumerable();
                Assert.IsTrue(res1.Count() == 1 && res1.First().Title == "Appointment", "DateRangesOverlap test failure (1) " + ctx.Name);
                var res2 = (from d in src where CamlMethods.DateRangesOverlap(CamlMethods.Today, d.From, d.To) select d).AsEnumerable();
                Assert.IsTrue(res2.Count() == 1 && res2.First().Title == "Appointment", "DateRangesOverlap test failure (2) " + ctx.Name);
            }
        }

        [TestMethod]
        public void LambdaFree()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);

            //
            // List source (doesn't matter for the LambdaFree check).
            //
            var src = new SharePointList<People>(GetSpContext());

            //
            // Query that tries to capture as much lambda free checks as possible.
            //
            int a = 1;
            int b = 2;
            object o = a;
            Func<int, int, int> action = (i, j) => i + j;
            var res = (from p in src where p.Age == LambdaFreeHelper(action(a, b), a + b, !(a == b), a + b == 3 ? a : b, new { a, b, C = new List<int> { a, b } }, new List<int>() { a, b }, new TimeSpan(a, b, 0), new int[] { a, b }, o is int) select p).AsEnumerable();
            Assert.IsTrue(res.Count() == 1, "LambdaFree check failed.");
        }

        double LambdaFreeHelper(params object[] o)
        {
            return 24;
        }

        [TestMethod]
        public void Pruning()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);

            foreach (var ctx in GetContexts<People>())
            {
                var src = ctx.List;

                bool t = true;
                bool f = false;

                //
                // Queries.
                //
                var res1 = (from p in src where t select p).AsEnumerable();
                Assert.IsTrue(res1.Count() == 1, "Pruning check failed (1) " + ctx.Name);
                var res2 = (from p in src where t && f select p).AsEnumerable();
                Assert.IsTrue(res2.Count() == 0, "Pruning check failed (2) " + ctx.Name);
                var res3 = (from p in src where t || f select p).AsEnumerable();
                Assert.IsTrue(res3.Count() == 1, "Pruning check failed (3) " + ctx.Name);
            }
        }

        /*
        [TestMethod]
        public void EntityRef()
        {
            EntityRef<People> p = new EntityRef<People>();
        }
         */

        [TestMethod]
        public void Url()
        {
            //
            // Create list UrlTest.
            //
            var lst = Test.Create<UrlTest>(site.RootWeb);

            //
            // Add items.
            //
            UrlValue url1 = new UrlValue("http://www.bartdesmet.net", "Bart's homepage");
            UrlValue url2 = new UrlValue("http://www.codeplex.com/LINQtoSharePoint", null);
            UrlValue url3 = new UrlValue(null, null);
            UrlTest u1 = new UrlTest() { Title = "Bart", Homepage = url1 };
            UrlTest u2 = new UrlTest() { Title = "Project", Homepage = url2 };
            UrlTest u3 = new UrlTest() { Title = "Null", Homepage = url3 };
            UrlTest u4 = new UrlTest() { Title = "NullToo" };
            Test.Add(lst, u1);
            Test.Add(lst, u2);
            Test.Add(lst, u3);
            Test.Add(lst, u4);

            foreach (var ctx in GetContexts<UrlTest>())
            {
                var src = ctx.List;

                //
                // Queries.
                //
                var res1 = (from u in src where u.ID == 1 select u.Homepage).First();
                Assert.IsTrue(res1.Description == "Bart's homepage" && res1.Url == "http://www.bartdesmet.net", "Test for Url failed (1) " + ctx.Name);
                var res2 = (from u in src where u.ID == 2 select u.Homepage).First();
                Assert.IsTrue(res2.Url == "http://www.codeplex.com/LINQtoSharePoint", "Test for Url failed (2) " + ctx.Name);
                var res3 = (from u in src where u.ID == 3 select u.Homepage).First();
                Assert.IsTrue(res3 == null, "Test for Url failed (3) " + ctx.Name);
                var res4 = (from u in src where u.ID == 4 select u.Homepage).First();
                Assert.IsTrue(res4 == null, "Test for Url failed (4) " + ctx.Name);
            }
        }

        [TestMethod]
        public void TestVbStringCompare()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);
            People p2 = new People() { FirstName = "John", LastName = "De Smet", Age = 54, IsMember = false, ShortBiography = "Family" };
            Test.Add(lst, p2);

            //
            // Get query with Visual Basic string compare.
            //
            var res = TestHelpersVb.Helpers.StrCompare(site).AsEnumerable();
            Assert.IsTrue(res.Count() == 1 && res.First().FirstName == "Bart", "Visual Basic string compare parse failure in query predicate.");
        }

        [TestMethod]
        public void TestVbStrComp()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            Test.Add(lst, p1);
            People p2 = new People() { FirstName = "John", LastName = "De Smet", Age = 54, IsMember = false, ShortBiography = "Family" };
            Test.Add(lst, p2);

            //
            // Get query with Visual Basic string compare.
            //
            var res = TestHelpersVb.Helpers.StrCmp(site).AsEnumerable();
            Assert.IsTrue(res.Count() == 1 && res.First().FirstName == "Bart", "Visual Basic string compare parse failure in query predicate.");
        }

        [TestMethod]
        public void RowLimit()
        {
            //
            // Create list RowLimit.
            //
            var lst = Test.Create<RowLimit>(site.RootWeb);

            //
            // Add items.
            //
            for (int i = 0; i < 120; i++)
            {
                Test.Add(lst, new RowLimit() { Title = "Test " + (i + 1) });
            }

            //
            // Test default query.
            //
            foreach (var ctx in GetContexts<RowLimit>())
            {
                var src = ctx.List;
                var res = (from p in src select p).AsEnumerable();

                Assert.IsTrue(res.Count() == 120, "Query did not return enough results " + ctx.Name);
            }
        }

        private static void AssertWsEqualsSp<T>(IQueryable<T> ws, IQueryable<T> sp, Expression<Func<T, bool>> predicate, string message)
        {
            var res1 = ws.Where(predicate).AsEnumerable();
            //var res1b = res1.ToArray();
            var res2 = sp.Where(predicate).AsEnumerable();
            //var res2b = res2.ToArray();
            //Assert.IsTrue(res1.SequenceEqual(res2), message);
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Junk()
        {
            //
            // Create list People.
            //
            var lst = Test.Create<People>(site.RootWeb);

            //
            // Add items.
            //
            People p1 = new People() { FirstName = "Bart", LastName = "De Smet", Age = 24, IsMember = true, ShortBiography = "Project founder" };
            People p2 = new People() { FirstName = "Bill", LastName = "Gates", Age = 52, IsMember = false, ShortBiography = "Microsoft Corporation founder" };
            Test.Add(lst, p1);
            Test.Add(lst, p2);

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

        private void AssertWhere<T>(Expression<Func<T, bool>> predicate, int expectedCount, string message) where T : class
        {
            var sp = new SharePointList<T>(GetSpContext());
            var ws = new SharePointList<T>(GetWsContext());

            Func<T, bool> pred = predicate.Compile();

            {
                IEnumerable<T> res = sp.Where<T>(predicate).Select(e => e).AsEnumerable();
                Assert.IsTrue(res.Count() == expectedCount && res.All(pred), message);
            }
            {
                IEnumerable<T> res = ws.Where<T>(predicate).Select(e => e).AsEnumerable();
                Assert.IsTrue(res.Count() == expectedCount && res.All(pred), message);
            }
        }

        private void AssertTake<T>(int take, int expectedCount, string message) where T : class
        {
            var sp = new SharePointList<T>(GetSpContext());
            var ws = new SharePointList<T>(GetWsContext());

            var res1 = (from p in sp select p).Take(take).AsEnumerable();
            Assert.IsTrue(res1.Count() == expectedCount, message);

            var res2 = (from p in ws select p).Take(take).AsEnumerable();
            Assert.IsTrue(res2.Count() == expectedCount, message);
        }

        private void AssertProject<T, R>(Expression<Func<T, R>> selector, IEnumerable<R> results, string message) where T : class
        {
            var sp = new SharePointList<T>(GetSpContext());
            var ws = new SharePointList<T>(GetWsContext());

            {
                IEnumerable<R> res = sp.Select(selector).AsEnumerable();
                Assert.IsTrue(res.SequenceEqual(results));
            }
            {
                IEnumerable<R> res = ws.Select(selector).AsEnumerable();
                Assert.IsTrue(res.SequenceEqual(results));
            }
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
                            new Field() { Name = "ShortBiography", Type = SPFieldType.Note, Required = false },
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
