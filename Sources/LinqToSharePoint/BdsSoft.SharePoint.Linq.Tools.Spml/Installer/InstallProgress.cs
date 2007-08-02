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
                        InstallVsTemplate();
                        InstallSpmlXsd();
                        RegisterSpml();
                        ConfigVs();
                    }
                    break;
                case InstallerMode.Uninstall:
                    {
                        UninstallVsTemplate();
                        UninstallSpmlXsd();
                        UnregisterSpml();
                        ConfigVs();
                    }
                    break;
            }
        }

        static string spml = ".spml";
        static string clas = "LINQtoSharePoint.SPML.1.0";
        static string desc = "LINQ to SharePoint Entity Mapping";

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
                    kIcon.SetValue(null, Path.Combine(_target, @"SpMetal.exe") + ",0");
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

        private void UnregisterSpml()
        {
            Registry.ClassesRoot.DeleteSubKey(spml, false);
            Registry.ClassesRoot.DeleteSubKey(clas, false);
        }

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
            string assemblyName = String.Format("BdsSoft.SharePoint.Linq.Tools.EntityGenerator, Version={0}, Culture=neutral, PublicKeyToken={1}", version, publicKeyToken);
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

        private void InstallVsTemplate()
        {
            string csSrc = Path.Combine(_target, @"Item packages\LINQtoSharePointCS.zip");
            string vbSrc = Path.Combine(_target, @"Item packages\LINQtoSharePointVB.zip");
            string csTgt = Path.Combine(_vsPath, @"ItemTemplates\CSharp\1033\LINQtoSharePointCS.zip");
            string vbTgt = Path.Combine(_vsPath, @"ItemTemplates\VisualBasic\1033\LINQtoSharePointVB.zip");

            if (!File.Exists(csTgt))
                File.Copy(csSrc, csTgt);
            if (!File.Exists(vbTgt))
                File.Copy(vbSrc, vbTgt);
        }

        private void ConfigVs()
        {
            string devenv = Path.Combine(_vsPath, "devenv.exe");
            Process p = Process.Start(devenv, "/InstallVSTemplates");
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
