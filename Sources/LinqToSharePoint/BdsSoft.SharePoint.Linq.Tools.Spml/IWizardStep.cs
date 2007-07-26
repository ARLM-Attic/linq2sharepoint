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
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    interface IWizardStep
    {
        string Title { get; }
        bool CanNext { get; }
        event EventHandler StateChanged;
        event EventHandler Working;
        event EventHandler WorkCompleted;
        void Cancel();
    }
}
