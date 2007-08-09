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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using Microsoft.Win32;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;
using System.Globalization;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    [RunInstaller(true)]
    public partial class ProjectItemInstaller : Installer
    {
        public ProjectItemInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            string instal = GetVsPath();
            string target = this.Context.Parameters["TargetDir"];

            if (instal != null && target != null)
            {
                InstallProgress progress = new InstallProgress(InstallerMode.Install, target, instal);
                MessageBoxOptions options = CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading : 0;
                if (progress.ShowDialog() != DialogResult.OK)
                    MessageBox.Show("Visual Studio 2008 configuration failed.", "LINQ to SharePoint Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, options);
            }
        }

        private static string GetVsPath()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\VisualStudio\\9.0", false);
            if (key != null)
                return (string)key.GetValue("InstallDir");
            return null;
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            string instal = GetVsPath();
            
            if (instal != null)
            {
                InstallProgress progress = new InstallProgress(InstallerMode.Uninstall, null, instal);
                progress.ShowDialog();
            }
        }
    }
}
