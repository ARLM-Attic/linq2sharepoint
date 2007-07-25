using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BdsSoft.SharePoint.Linq.Tools.EntityGenerator;

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    public class Context
    {
        public Context()
        {
            Selection = new Selection();
        }

        public Connection ConnectionParameters { get; set; }
        public List<List> Lists { get; set; }
        public Selection Selection { get; set; }
    }

    public class Selection
    {
        public List<List> Lists { get; set; }
    }
}
