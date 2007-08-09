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
            var ctx = new NorthwindSharePointDataContext();
            ctx.Log = Console.Out;
            var res = from p in ctx.Products where p.Category.CategoryName != "Dairy Products" select p.ProductName;
            foreach (var p in res)
                Console.WriteLine(p);
        }
    }
}
