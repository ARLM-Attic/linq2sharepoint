using System;
using System.Linq;
using BdsSoft.SharePoint.Linq;
using System.Data.Linq;
using System.Linq.Expressions;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = MyCompiledQuery.Compile((DemoSharePointDataContext ctx) => from p in ctx.Products select p );
            var res = f(new DemoSharePointDataContext(new Uri("http://wss3demo")));

            /*
            NorthwindDataContext nw = new NorthwindDataContext();
            var prod = from p in nw.Products where p.UnitPrice < 100 select p;
            Console.WriteLine(prod.GetType().ToString());

            DemoSharePointDataContext ctx = new DemoSharePointDataContext(new Uri("http://wss3demo"));
            //ctx.Log = Console.Out;

            var res = from p in ctx.Products
                      where p.Category.CategoryName.StartsWith("Con")
                            && (p.Supplier.Country == "USA" && p.Supplier.Region == "LA"
                                || p.Supplier.Country == "Canada" && p.Supplier.Region == "Québec")
                      select p;
            foreach (var p in res)
                Console.WriteLine("{0} in category {1} costs {2} per unit", p.ProductName, p.Category.CategoryName, p.UnitPrice);

            //double? price = null;
            //var res = from p in ctx.Products where p.UnitPrice <= price && (p.Category.CategoryName == "Beverages" || p.Category.CategoryName == "Seafood") && p.Supplier.Country == "USA" orderby p.UnitPrice descending select p;
            var q = CompiledQuery.Compile(
                (NorthwindDataContext db, decimal? price) => (from p in db.Products where p.UnitPrice <= price && (p.Category.CategoryName == "Beverages" || p.Category.CategoryName == "Seafood") && p.Supplier.Country == "USA" orderby p.UnitPrice descending select p));
            foreach (var p in q(nw, 100))
                Console.WriteLine("{0} in category {1} costs {2} and is supplied by {3}.", p.ProductName, p.Category.CategoryName, p.UnitPrice, p.Supplier.CompanyName);
             */
        }
    }

    public sealed class MyCompiledQuery
    {
        public static Func<TArg0, TResult> Compile<TArg0, TResult>(Expression<Func<TArg0, TResult>> query)
        {
            return query.Compile();
        }
    }
}
