using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics;
using BdsSoft.SharePoint.Linq.Tools.DebuggerVisualizer;
using Microsoft.VisualStudio.DebuggerVisualizers;
using BdsSoft.SharePoint.Linq;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("BdsSoft.SharePoint.Linq.Tools.DebuggerVisualizer")]
[assembly: AssemblyDescription("Debugger visualizer for LINQ to SharePoint queries.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Bart De Smet")]
[assembly: AssemblyProduct("LINQ to SharePoint")]
[assembly: AssemblyCopyright("Copyright © Bart De Smet 2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a9eadc76-34d0-4d7b-80a7-b5819b575549")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.2.3.0")]
[assembly: AssemblyFileVersion("0.2.3.0")]

[assembly: DebuggerVisualizer(typeof(SharePointListQueryVisualizer), typeof(VisualizerObjectSource), Target = typeof(SharePointListQuery<>), Description = "LINQ to SharePoint Query Visualizer")]