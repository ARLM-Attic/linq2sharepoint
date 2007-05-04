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
 * NOTE: This class is used as a first collection of tests that will be replaced by a more complete unit test framework over time.
 *       Because of the tight coupling with the SharePoint configuration and list definitions on a test machine, this class will be
 *       of little use to project "readers" except for its value as a set of sample queries.
 */

using System;
using System.IO;
using System.Linq;
using System.Net;

using BdsSoft.SharePoint.Linq;
using Microsoft.SharePoint;

namespace Demo
{
    class Program
    {
        static string URL = "http://vsmar2007ctp";

        static void Main(string[] args)
        {
            SharePointDataSource<Demo> users = GetWsDataSource<Demo>(); //GetSpDataSource<Demo>();
            users.Log = Console.Out;
            users.CheckListVersion = false;


            SharePointDataSource<Demo2> demo2 = GetWsDataSource<Demo2>();
            demo2.Log = Console.Out;
            demo2.CheckListVersion = false;


            SharePointDataSource<Users2> users2 = GetWsDataSource<Users2>();
            users2.Log = Console.Out;
            users2.CheckListVersion = false;


            Console.WriteLine("BY ID 1");
            Console.WriteLine("-------\n");
            Demo d1 = users.GetEntityById(1, true);
            if (d1 != null)
                Console.WriteLine(d1.Title);
            Console.WriteLine();


            Console.WriteLine("BY ID 1");
            Console.WriteLine("-------\n");
            d1 = users.GetEntityById(1, true); //from cache
            if (d1 != null)
                Console.WriteLine(d1.Title);
            Console.WriteLine();


            Console.WriteLine("BY ID 2");
            Console.WriteLine("-------\n");
            Demo d2 = users.GetEntityById(2, true);
            if (d2 != null)
                Console.WriteLine(d2.Title);
            Console.WriteLine();


            Console.WriteLine("BY ID 3");
            Console.WriteLine("-------\n");
            Demo d3 = users.GetEntityById(3, true);
            if (d3 != null)
                Console.WriteLine(d3.Title);
            Console.WriteLine();


            Console.WriteLine("QUERY 1");
            Console.WriteLine("-------\n");

            var res1 = from u in users
                       select u;

            foreach (var s in res1)
                Console.WriteLine("{0} {1} = {2} is " + s.MembershipType, s.FirstName, s.LastName, s.FullName);
            Console.WriteLine();


            Console.WriteLine("QUERY 2");
            Console.WriteLine("-------\n");

            var res2 = from u in users
                       select u.FullName;

            foreach (var s in res2)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 3");
            Console.WriteLine("-------\n");

            var res3 = from u in users
                       select new { Name = u.FullName, Age = u.Age };

            foreach (var s in res3)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 4");
            Console.WriteLine("-------\n");

            var res4 = from u in users
                       orderby u.Age descending
                       select new { Name = u.FullName, Age = u.Age };

            foreach (var s in res4)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 5");
            Console.WriteLine("-------\n");

            var res5 = from u in users
                       orderby u.FirstName ascending, u.LastName descending
                       select new { Name = u.FullName, Age = u.Age };

            foreach (var s in res5)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 6");
            Console.WriteLine("-------\n");

            var res6 = from u in users
                       where u.FirstName.Equals("Bart")
                       select u.FirstName;

            foreach (var s in res6)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 7");
            Console.WriteLine("-------\n");

            var res7 = from u in users
                       where (u.FirstName == "Bart" && u.Age >= 24) || (u.LastName.StartsWith("De") && u.LastName.Contains("Smet"))
                       select u.FullName;

            foreach (var s in res7)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 8");
            Console.WriteLine("-------\n");

            var res8 = from u in users
                       where u.Age >= 24
                       select u.FullName;

            foreach (var s in res8)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 9");
            Console.WriteLine("-------\n");

            var res9 = from u in users
                       where 24 <= (int)u.Age //convert will be filtered away
                       select u.FullName;

            foreach (var s in res9)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 10");
            Console.WriteLine("--------\n");

            var res10 = from u in users
                        where !(u.Age > 24)
                        select u.FullName;

            foreach (var s in res10)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 11");
            Console.WriteLine("--------\n");

            var res11 = from u in users
                        where !(24 < u.Age)
                        select u.FullName;

            foreach (var s in res11)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 12");
            Console.WriteLine("--------\n");

            var res12 = from u in users
                        select u.Age.Value * 2; //u.Age.Value is recognized as reference to Age field

            foreach (var s in res12)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 13");
            Console.WriteLine("--------\n");

            var res13 = from u in users
                        select QueryHelper(u);

            foreach (var s in res13)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 14");
            Console.WriteLine("--------\n");

            var res14 = (from u in users
                         select u).Take(1);

            foreach (var s in res14)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 15");
            Console.WriteLine("--------\n");

            var res15 = (from usr in users
                         where usr.Age >= 24 && (usr.FirstName.StartsWith("B") || usr.FirstName == "Johan") && usr.Age != null
                         orderby usr.Age descending
                         orderby usr.FirstName ascending
                         select new { usr.FirstName, usr.LastName, usr.Age, usr.MemberSince }).Take(1);

            foreach (var s in res15)
                Console.WriteLine(s);


            Console.WriteLine("QUERY 16");
            Console.WriteLine("--------\n");

            DateTime startDate = new DateTime(2007, 01, 01);
            string firstNameStartsWith = "B";
            int maximumAge1 = 24;
            string lastNameContains = "Smet";
            int minimumAge2 = 50;
            var res16 = from usr in users
                        where ((usr.FirstName.StartsWith(firstNameStartsWith) && !(usr.Age > maximumAge1))
                                  || (usr.LastName.Contains(lastNameContains) && usr.Age >= minimumAge2))
                              && usr.Member == true
                              && usr.MemberSince >= startDate
                              && usr.ShortBio != null
                        orderby usr.MemberSince descending
                        orderby usr.Age ascending
                        select new
                               {
                                   Name = usr.FirstName + " " + usr.LastName,
                                   usr.Age,
                                   usr.Homepage,
                                   usr.ShortBio,
                                   usr.Activities,
                                   MembershipInfo = new { usr.MembershipType, usr.MemberSince },
                                   FoodPreferences = new { usr.FavoriteFood, usr.FavoriteFoodOther }
                               };

            foreach (var s in res16)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 17");
            Console.WriteLine("--------\n");

            var res17 = from u in users
                        select "Constant";

            foreach (var s in res17)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 18");
            Console.WriteLine("--------\n");

            var res18 = from u in users
                        select String.Format("{0} is {1}", u.FirstName, u.Age);

            foreach (var s in res18)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 19");
            Console.WriteLine("--------\n");

            var res19 = from u in users
                        select u.FirstName + " is " + u.Age;

            foreach (var s in res19)
                Console.WriteLine(s);
            Console.WriteLine();


            try
            {
                Console.WriteLine("QUERY 20");
                Console.WriteLine("--------\n");

                var res20 = from u in users
                            where SomeBool(u.FirstName)
                            select u;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }


            try
            {
                Console.WriteLine("QUERY 21");
                Console.WriteLine("--------\n");

                var res21 = from u in users
                            where u.FirstName.EndsWith("t")
                            select u;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }


            try
            {
                Console.WriteLine("QUERY 22");
                Console.WriteLine("--------\n");

                var res22 = from u in users
                            where !u.FirstName.StartsWith("B")
                            select u;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }


            Console.WriteLine("QUERY 23");
            Console.WriteLine("--------\n");

            var res23 = from u in users
                        where u.FavoriteFood <= FavoriteFood.Pizza
                        select u.FullName;

            foreach (var s in res23)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 24");
            Console.WriteLine("--------\n");

            var res24 = from u in users
                        where u.Activities != Activities.Adventure
                        select u.FullName;

            foreach (var s in res24)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 25");
            Console.WriteLine("--------\n");

            var res25 = from u in users
                        where u.FavoriteFoodOther == "Spaghetti"
                        select u.FullName;

            foreach (var s in res25)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 26");
            Console.WriteLine("--------\n");

            var res26 = from u in users
                        where u.FavoriteFood != FavoriteFood.Pizza
                        select new { u.FullName, u.FavoriteFood, u.FavoriteFoodOther };

            foreach (var s in res26)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 27");
            Console.WriteLine("--------\n");

            var res27 = from u in users
                        where !!(u.Homepage != null)
                        select u.FullName;

            foreach (var s in res27)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 28");
            Console.WriteLine("--------\n");

            var res28 = from u in users
                        where !!(u.Homepage == null)
                        select u.FullName;

            foreach (var s in res28)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 29");
            Console.WriteLine("--------\n");

            var res29 = from u in users
                        where u.Activities == (Activities.Adventure | Activities.Quiz) //SEMANTICS!
                        select u.FullName;

            foreach (var s in res29)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 30");
            Console.WriteLine("--------\n");

            var res30 = from u in users
                        where u.Activities == (Activities.Adventure | Activities.Quiz | Activities.Culture) //SEMANTICS!
                        select u.FullName;

            foreach (var s in res30)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 31");
            Console.WriteLine("--------\n");

            var res31 = from u in users
                        where u.Test == Test.LaurelHardy
                        select u.FullName;

            foreach (var s in res31)
                Console.WriteLine(s);
            Console.WriteLine();


            try
            {
                Console.WriteLine("QUERY 32");
                Console.WriteLine("--------\n");

                var res32 = from u in users
                            where (u.Test & Test.LaurelHardy) == Test.LaurelHardy
                            select u.FullName;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }


            Console.WriteLine("QUERY 33");
            Console.WriteLine("--------\n");

            var res33 = from u in users
                        where u.FirstName == "Bart".ToString()
                        select u.FullName;

            foreach (var s in res33)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 34");
            Console.WriteLine("--------\n");

            var res34 = from u in users
                        where u.FirstName.ToString().ToString() == "Bart"
                        select u.FullName;

            foreach (var s in res34)
                Console.WriteLine(s);
            Console.WriteLine();


            try
            {
                Console.WriteLine("QUERY 35");
                Console.WriteLine("--------\n");

                var res35 = from u in users
                            where u.FirstName == u.LastName
                            select u.FullName;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }


            Console.WriteLine("QUERY 36");
            Console.WriteLine("--------\n");

            var res36 = from u in users
                        where (int)u.Age == 24 && (uint)u.Age == 24 && (long)u.Age == 24
                        select u.FullName;

            foreach (var s in res36)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 37");
            Console.WriteLine("--------\n");

            var res37 = from u in users
                        where u.Homepage == "http://www.bartdesmet.net"
                        select u.FullName;

            foreach (var s in res37)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 38");
            Console.WriteLine("--------\n");

            var res38 = from u in users
                        where u.Homepage == new Uri("http://www.bartdesmet.net") // ToString of Uri results in trailing slash
                        select u.FullName;

            foreach (var s in res38)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 39");
            Console.WriteLine("--------\n");

            try
            {
                var res39 = from u in users
                            orderby u.FirstName + "-" ascending
                            //orderby u.Age + 1 ascending
                            //orderby u.Age.ToString()
                            //orderby QueryHelper(u) ascending
                            select u;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }


            Console.WriteLine("QUERY 40");
            Console.WriteLine("--------\n");

            var res40 = from u in users
                        orderby u.FirstName.ToString() ascending
                        select u.FullName;

            foreach (var s in res40)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 41");
            Console.WriteLine("--------\n");

            try
            {
                string dummy = "Bart";
                var res41 = from u in users
                            orderby dummy.Length ascending
                            select u.FullName;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }


            Console.WriteLine("QUERY 42");
            Console.WriteLine("--------\n");

            try
            {
                var res42 = from u in users
                            where u.FirstName.StartsWith(u.LastName)
                            select u;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }


            Console.WriteLine("QUERY 43");
            Console.WriteLine("--------\n");

            try
            {
                var res43 = from u in users
                            where u.Age >= u.AccountBalance
                            select u;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }

            Console.WriteLine("QUERY 44");
            Console.WriteLine("--------\n");

            var res44 = from u in users
                        where u.FavoriteFoodOther == "Lasagne"
                        select u.FullName;

            foreach (var s in res44)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 45");
            Console.WriteLine("--------\n");

            var res45 = from u in users
                        where u.Age.Value >= 24
                        select u.FullName;

            foreach (var s in res45)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 46");
            Console.WriteLine("--------\n");

            var res46 = from u in users
                        where !(24 != u.Age.Value)
                        select u.FullName;

            foreach (var s in res46)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 47");
            Console.WriteLine("--------\n");

            var res47 = from u in users
                        where u.Age.HasValue == true && u.Age.HasValue != false && !(u.Age.HasValue != true) && !(u.Age.HasValue == false)
                        select u.FullName;

            foreach (var s in res47)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 48");
            Console.WriteLine("--------\n");

            var res48 = from u in users
                        where !(u.Member.HasValue && !u.Member.HasValue)
                        select u.FullName;

            foreach (var s in res48)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 49");
            Console.WriteLine("--------\n");

            var res49 = from u in users
                        where !(u.Member.Value && !u.Member.Value)
                        select u.FullName;

            foreach (var s in res49)
                Console.WriteLine(s);
            Console.WriteLine();


            Console.WriteLine("QUERY 50");
            Console.WriteLine("--------\n");

            var res50 = from u in users
                        orderby u.Age.Value descending
                        select u;

            foreach (var s in res50)
                Console.WriteLine(s.Age);
            Console.WriteLine();


            Console.WriteLine("QUERY 51");
            Console.WriteLine("--------\n");

            try
            {
                var res51 = from u in users
                            orderby u.Age.HasValue descending
                            select u;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not OK\n");
                Console.ResetColor();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("OK - " + ex.Message + "\n");
            }


            Console.WriteLine("QUERY 52");
            Console.WriteLine("--------\n");

            var res52 = from u in users
                        where u.FirstName == null && u.LastName != null
                        select u;

            foreach (var u in res52)
                Console.WriteLine(u.FullName);
            Console.WriteLine();


            Console.WriteLine("QUERY 53");
            Console.WriteLine("--------\n");

            var res53 = from u in users
                        where u.ShortBio.StartsWith("<div>Bart")
                        select new { u.FullName, u.ShortBio };

            foreach (var u in res53)
                Console.WriteLine(u);
            Console.WriteLine();


            Console.WriteLine("QUERY 54");
            Console.WriteLine("--------\n");

            var res54 = from u in users
                        where u.FirstName.StartsWith("")
                        || !u.FullName.Equals(null)
                        || (u.FirstName.Contains(null) && (u.FirstName.StartsWith(null) || u.LastName.Contains(""))) //will be optimized
                        select u;

            foreach (var u in res54)
                Console.WriteLine(u.Title);
            Console.WriteLine();


            Console.WriteLine("QUERY 55");
            Console.WriteLine("--------\n");

            var res55 = from d in demo2
                        where (d.Profile.Title.ToString().StartsWith("Bart") && (d.Profile.Age.HasValue && d.Profile.Age.Value == 24)) && d.Title.StartsWith("Bart")
                        select d;

            foreach (var i in res55)
                Console.WriteLine(i.Title);
            Console.WriteLine();


            Console.WriteLine("QUERY 56");
            Console.WriteLine("--------\n");

            var res56 = from d in demo2
                        where d.Profile.Title.Contains("De Smet")
                        select d.Title;

            foreach (var i in res56)
                Console.WriteLine(i);
            Console.WriteLine();


            Console.WriteLine("QUERY 57");
            Console.WriteLine("--------\n");

            var res57 = from d in demo2
                        where d.Profile != users2.GetEntityById(1, true) //will do key matching with Lookup list
                        && d.Profile != (from u in users2 where u.Title.StartsWith("Bart") select u).AsEnumerable().First()
                        select d;

            foreach (var d in res57)
            {
                Users2 u2 = d.Profile;
                u2 = d.Profile;
                Console.WriteLine(u2.Title);
            }
            Console.WriteLine();


            Console.WriteLine("QUERY 58");
            Console.WriteLine("--------\n");

            var res58 = from d in demo2
                        select d.Profile; //will trigger subquery during iteration

            foreach (var i in res58)
                Console.WriteLine(i.Title + " is " + i.Age);
            Console.WriteLine();
        }

        static string QueryHelper(Demo d)
        {
            return "Dummy";
        }

        static bool SomeBool(string s)
        {
            return false;
        }

        static SharePointDataSource<T> GetWsDataSource<T>()
        {
            SharePointDataSource<T> src = new SharePointDataSource<T>(new Uri(URL));
            src.Credentials = CredentialCache.DefaultNetworkCredentials;
            return src;
        }

        static SharePointDataSource<T> GetSpDataSource<T>()
        {
            SPSite s = new SPSite(URL);
            //SPList l = s.RootWeb.GetList("/Lists/" + list);
            return new SharePointDataSource<T>(s);
        }
    }
}