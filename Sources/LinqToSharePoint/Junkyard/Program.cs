using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Junkyard
{
    class Program
    {
        static void Main()
        {
            /*
             * TODO for advanced lookup subqueries:
             * - Query execution
             * - Test various query shapes
             * - Coalescing
             */

            /*
            var ctx = new NorthwindSharePointDataContext();
            ctx.Log = Console.Out;
            var res = from p in ctx.Products where p.Category.CategoryName != "Dairy Products" select p.ProductName;
            foreach (var p in res)
                Console.WriteLine(p);
             */

            /*
            var ctx = new NorthwindSharePointDataContext();
            ctx.Log = Console.Out;
            ctx.CheckListVersion = false;
            //var res = from o in ctx.Orders where o.Product.Category.CategoryName != "Dairy Products"  && o.Product.UnitPrice > 10 && o.Product.Category.ID < 3 select o.Title;
            //foreach (var o in res)
            //    Console.WriteLine(o);

            var res = from l in ctx.NestedSubqueriesMulti where l.Products.Any(p => p.Category.CategoryName == "Beverages") select l;
            foreach (var l in res)
                Console.WriteLine(l.Title);
             */
        }
    }
}
