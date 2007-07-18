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
                if (progress.ShowDialog() != DialogResult.OK)
                    MessageBox.Show("Visual Studio 2008 configuration failed.", "LINQ to SharePoint Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GetVsPath()
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
