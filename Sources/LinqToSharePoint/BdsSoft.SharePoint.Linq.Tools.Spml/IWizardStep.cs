using System;
using System.Collections.Generic;
using System.Text;

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
