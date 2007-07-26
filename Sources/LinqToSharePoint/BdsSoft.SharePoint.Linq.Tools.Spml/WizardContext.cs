/*
 * LINQ to SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

/*
 * Version history:
 * 
 * 0.2.2 - Introduction of entity wizard
 * 0.2.3 - Rename to WizardContext
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BdsSoft.SharePoint.Linq.Tools.EntityGenerator;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    public class WizardContext
    {
        public WizardContext()
        {
            Selection = new Selection();
        }

        public WizardConnection ConnectionParameters { get; set; }
        public List<List> Lists { get; set; }
        public Selection Selection { get; set; }
    }

    public class Selection
    {
        public List<List> Lists { get; set; }
    }
}
