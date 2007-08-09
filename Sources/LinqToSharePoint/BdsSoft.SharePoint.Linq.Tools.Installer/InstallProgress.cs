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
using System.Reflection;
using Microsoft.Win32;
using System.Globalization;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Installer
{
    public partial class InstallProgress : Form
    {
        private InstallerMode _mode;
        private string _installDir;
        private string _vsPath;

        public InstallProgress(InstallerMode mode, string target, string vsPath)
        {
            _mode = mode;
            _installDir = target;
            _vsPath = vsPath;

            InitializeComponent();
        }

        #region Event handlers

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (_mode)
            {
                case InstallerMode.Install:
                    {
                        InstallVsTemplate();
                        InstallSpmlXsd();
                        InstallDebuggerVisualizer();
                        RegisterSpml();
                        ConfigVs();
                    }
                    break;
                case InstallerMode.Uninstall:
                    {
                        UninstallVsTemplate();
                        UninstallSpmlXsd();
                        UninstallDebuggerVisualizer();
                        UnregisterSpml();
                        ConfigVs();
                    }
                    break;
            }
        }

        private void InstallProgress_Load(object sender, EventArgs e)
        {
            installationWorker.RunWorkerAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                this.DialogResult = DialogResult.Cancel;
            else
                this.DialogResult = DialogResult.OK;
        }

        #endregion

        #region SPML

        static string spml = ".spml";
        static string clas = "LINQtoSharePoint.SPML.1.0";
        static string desc = "LINQ to SharePoint Entity Mapping";

        #region Registration

        private void RegisterSpml()
        {
            #region HKEY_CLASSES_ROOT\.spml
            {
                RegistryKey kSpml = Registry.ClassesRoot.CreateSubKey(spml, RegistryKeyPermissionCheck.ReadWriteSubTree);
                kSpml.SetValue(null, clas);
                kSpml.Close();
            }
            #endregion

            #region HKEY_CLASSES_ROOT\LINQtoSharePoint.SPML.1.0
            {
                RegistryKey kClas = Registry.ClassesRoot.CreateSubKey(clas, RegistryKeyPermissionCheck.ReadWriteSubTree);
                kClas.SetValue(null, desc);
                kClas.SetValue("AlwaysShowExt", "1");

                #region HKEY_CLASSES_ROOT\LINQtoSharePoint.SPML.1.0\DefaultIcon
                {
                    RegistryKey kIcon = kClas.CreateSubKey("DefaultIcon");
                    kIcon.SetValue(null, Path.Combine(_installDir, @"SpMetal.exe") + ",0");
                    kIcon.Close();
                }
                #endregion

                #region HKEY_CLASSES_ROOT\LINQtoSharePoint.SPML.1.0\shell
                {
                    RegistryKey kShel = kClas.CreateSubKey("shell");

                    #region HKEY_CLASSES_ROOT\LINQtoSharePoint.SPML.1.0\shell\Open
                    {
                        RegistryKey kOpen = kShel.CreateSubKey("Open");

                        #region HKEY_CLASSES_ROOT\LINQtoSharePoint.SPML.1.0\shell\Open\Command
                        {
                            RegistryKey kComm = kOpen.CreateSubKey("Command");
                            kComm.SetValue(null, "\"" + Path.Combine(_vsPath, "devenv.exe") + "\" /dde \"%1\"");
                            kComm.Close();
                        }
                        #endregion

                        #region HKEY_CLASSES_ROOT\LINQtoSharePoint.SPML.1.0\shell\Open\ddeexec
                        {
                            RegistryKey kDdex = kOpen.CreateSubKey("ddeexec");
                            kDdex.SetValue(null, "Open(\"%1\")");

                            #region HKEY_CLASSES_ROOT\LINQtoSharePoint.SPML.1.0\shell\Open\ddeexec\Application
                            {
                                RegistryKey kAppn = kDdex.CreateSubKey("Application");
                                kAppn.SetValue(null, "VisualStudio.9.0");
                                kAppn.Close();
                            }
                            #endregion

                            #region HKEY_CLASSES_ROOT\LINQtoSharePoint.SPML.1.0\shell\Open\ddeexec\Topic
                            {
                                RegistryKey kTopc = kDdex.CreateSubKey("Topic");
                                kTopc.SetValue(null, "system");
                                kTopc.Close();
                            }
                            #endregion

                            kDdex.Close();
                        }
                        #endregion

                        kOpen.Close();
                    }
                    #endregion

                    kShel.Close();
                }
                #endregion

                kClas.Close();
            }
            #endregion
        }

        private static void UnregisterSpml()
        {
            if (Registry.ClassesRoot.OpenSubKey(spml) != null)
                Registry.ClassesRoot.DeleteSubKeyTree(spml);
            if (Registry.ClassesRoot.OpenSubKey(clas) != null)
                Registry.ClassesRoot.DeleteSubKeyTree(clas);
        }

        #endregion

        #region XSD

        private void InstallSpmlXsd()
        {
            //
            // Get version and public key token from current assembly.
            //
            AssemblyName current = Assembly.GetExecutingAssembly().GetName();

            StringBuilder sb = new StringBuilder();
            foreach (byte b in current.GetPublicKeyToken())
                sb.AppendFormat("{0:x2}", b);

            string publicKeyToken = sb.ToString();
            string version = current.Version.ToString();

            //
            // Get entity generator assembly which contains the XSD for SPML.
            //
            string assemblyName = String.Format(CultureInfo.InvariantCulture, "BdsSoft.SharePoint.Linq.Tools.EntityGenerator, Version={0}, Culture=neutral, PublicKeyToken={1}", version, publicKeyToken);
            Assembly assembly = Assembly.Load(assemblyName);

            //
            // Load the XSD for SPML.
            //
            string xsd;
            using (Stream s = assembly.GetManifestResourceStream(assembly.GetName().Name + ".SPML.xsd"))
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    xsd = sr.ReadToEnd();
                }
            }

            //
            // Write the XSD to the Visual Studio folder with XML schemas.
            //
            using (StreamWriter sw = File.CreateText(GetSpmlXsdPath()))
            {
                sw.Write(xsd);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void UninstallSpmlXsd()
        {
            try
            {
                File.Delete(GetSpmlXsdPath());
            }
            catch { }
        }

        private string GetSpmlXsdPath()
        {
            string path = _vsPath; //%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\IDE\
            path = path.TrimEnd('\\'); //%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\IDE
            path = path.Substring(0, path.LastIndexOf('\\')); //%ProgramFiles%\Microsoft Visual Studio 9.0\Common7
            path = path.Substring(0, path.LastIndexOf('\\')); //%ProgramFiles%\Microsoft Visual Studio 9.0
            path = Path.Combine(path, "Xml"); //%ProgramFiles%\Microsoft Visual Studio 9.0\Xml
            path = Path.Combine(path, "Schemas"); //%ProgramFiles%\Microsoft Visual Studio 9.0\Xml\Schemas
            path = Path.Combine(path, "SPML.xsd"); //%ProgramFiles%\Microsoft Visual Studio 9.0\Xml\Schemas\SPML.xsd
            return path;
        }

        #endregion

        #endregion

        #region Template

        private void InstallVsTemplate()
        {
            string csSrc = Path.Combine(_installDir, @"Item packages\LINQtoSharePointCS.zip");
            string vbSrc = Path.Combine(_installDir, @"Item packages\LINQtoSharePointVB.zip");
            string csTgt = Path.Combine(_vsPath, @"ItemTemplates\CSharp\1033\LINQtoSharePointCS.zip");
            string vbTgt = Path.Combine(_vsPath, @"ItemTemplates\VisualBasic\1033\LINQtoSharePointVB.zip");

            if (!File.Exists(csTgt))
                File.Copy(csSrc, csTgt);
            if (!File.Exists(vbTgt))
                File.Copy(vbSrc, vbTgt);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void UninstallVsTemplate()
        {
            string csTgt = Path.Combine(_vsPath, @"ItemTemplates\CSharp\1033\LINQtoSharePointCS.zip");
            string vbTgt = Path.Combine(_vsPath, @"ItemTemplates\VisualBasic\1033\LINQtoSharePointVB.zip");

            try
            {
                File.Delete(csTgt);
                File.Delete(vbTgt);
            }
            catch { }
        }

        #endregion

        #region Debugger visualizer

        private string dbg = "BdsSoft.SharePoint.Linq.Tools.DebuggerVisualizer.dll";

        private void InstallDebuggerVisualizer()
        {
            string src = Path.Combine(_installDir, dbg);
            string tgt = Path.Combine(GetDebuggerVisualizerPath(), dbg);

            if (!File.Exists(tgt))
                File.Copy(src, tgt);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void UninstallDebuggerVisualizer()
        {
            string tgt = Path.Combine(GetDebuggerVisualizerPath(), dbg);

            try
            {
                File.Delete(tgt);
            }
            catch { }
        }

        private string GetDebuggerVisualizerPath()
        { 
            string path = _vsPath; //%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\IDE\
            path = path.TrimEnd('\\'); //%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\IDE
            path = path.Substring(0, path.LastIndexOf('\\')); //%ProgramFiles%\Microsoft Visual Studio 9.0\Common7
            path = Path.Combine(path, "Packages"); //%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\Packages
            path = Path.Combine(path, "Debugger"); //%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\Packages\Debugger
            path = Path.Combine(path, "Visualizers"); //%ProgramFiles%\Microsoft Visual Studio 9.0\Common7\Packages\Debugger\Visualizers
            return path;
        }

        #endregion

        #region VS2008 configuration

        private void ConfigVs()
        {
            string devenv = Path.Combine(_vsPath, "devenv.exe");
            Process p = Process.Start(devenv, "/InstallVSTemplates");
            p.WaitForExit();
        }

        #endregion
    }

    public enum InstallerMode
    {
        Install,
        Uninstall
    }
}
