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
            var res = (from l in ctx.LookupMultiTest select l).AsEnumerable().SingleOrDefault();
            Console.WriteLine(res.Categories.IsLoaded);
            Console.WriteLine(res.Categories[0].CategoryName);
            Console.WriteLine(res.Categories.IsLoaded);
        }
    }
}
