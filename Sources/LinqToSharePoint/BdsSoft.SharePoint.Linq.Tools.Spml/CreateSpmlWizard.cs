/*
 * LINQ to SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    public class CreateSpmlWizard : IWizard
    {
        private string _tool;
        //private string _namespace;
        private bool _ok;

        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
            projectItem.Properties.Item("CustomTool").Value = _tool;
            //projectItem.Properties.Item("CustomToolNamespace").Value = _namespace;
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            _DTE dte = automationObject as _DTE;
            IDEWindow window = new IDEWindow(new IntPtr(dte.MainWindow.HWnd));

            if (runKind == WizardRunKind.AsNewItem)
            {
                _tool = replacementsDictionary["$CustomTool$"];
                //_namespace = "test";// replacementsDictionary["$rootnamespace$"];

                Wizard start = new Wizard();
                DialogResult res = start.ShowDialog(window);
                _ok = res == DialogResult.OK;

                if (_ok)
                {
                    replacementsDictionary.Add("$WssUrl$", start.Context.WssUrl);
                }
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return _ok;
        }
    }

    internal class IDEWindow : IWin32Window
    {
        private IntPtr _handle;

        public IDEWindow(IntPtr handle)
        {
            _handle = handle;
        }

        public IntPtr Handle
        {
            get { return _handle; }
        }
    }
}
