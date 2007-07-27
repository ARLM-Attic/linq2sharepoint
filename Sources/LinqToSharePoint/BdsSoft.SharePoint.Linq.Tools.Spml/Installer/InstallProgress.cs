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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    public partial class InstallProgress : Form
    {
        private InstallerMode _mode;
        private string _target;
        private string _vsPath;

        public InstallProgress(InstallerMode mode, string target, string vsPath)
        {
            _mode = mode;
            _target = target;
            _vsPath = vsPath;

            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (_mode)
            {
                case InstallerMode.Install:
                    {
                        string csSrc = Path.Combine(_target, @"Item packages\LINQtoSharePointCS.zip");
                        string vbSrc = Path.Combine(_target, @"Item packages\LINQtoSharePointVB.zip");
                        string csTgt = Path.Combine(_vsPath, @"ItemTemplates\CSharp\1033\LINQtoSharePointCS.zip");
                        string vbTgt = Path.Combine(_vsPath, @"ItemTemplates\VisualBasic\1033\LINQtoSharePointVB.zip");

                        if (!File.Exists(csTgt))
                            File.Copy(csSrc, csTgt);
                        if (!File.Exists(vbTgt))
                            File.Copy(vbSrc, vbTgt);

                        ConfigVs();
                    }
                    break;
                case InstallerMode.Uninstall:
                    {
                        string csTgt = Path.Combine(_vsPath, @"ItemTemplates\CSharp\1033\LINQtoSharePointCS.zip");
                        string vbTgt = Path.Combine(_vsPath, @"ItemTemplates\VisualBasic\1033\LINQtoSharePointVB.zip");

                        try
                        {
                            File.Delete(csTgt);
                            File.Delete(vbTgt);
                        }
                        catch { }

                        ConfigVs();
                    }
                    break;
            }
        }

        private void ConfigVs()
        {
            string devenv = Path.Combine(_vsPath, "devenv.exe");
            Process p = Process.Start(devenv, "/setup");
            p.WaitForExit();
        }

        private void InstallProgress_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                this.DialogResult = DialogResult.Cancel;
            else
                this.DialogResult = DialogResult.OK;
        }
    }

    public enum InstallerMode
    {
        Install,
        Uninstall
    }
}
