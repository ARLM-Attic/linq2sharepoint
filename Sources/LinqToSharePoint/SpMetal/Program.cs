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
 * 0.1.0 - Alpha
 * 0.1.1 - Added Visual Basic language support and -language command line switch
 *         Bug fix for DateTime? and Nullable(Of DateTime)
 * 0.1.3 - Bug fix for FillInChoice attribute presence check (not required)
 *         Bug fix for (Multi)Choice fields containing choices starting with a digit
 * 0.2.0 - Generates entity types deriving from SharePointEntityType
 *         Read-only property generation for read-only fields
 *         Support for Counter field type for primary key values
 *         Infer file name for entity class from list's name, not the command-line parameter
 *         Restructuring of class files in project
 *         Hosting model with events
 *         Support for Lookup fields with recursive entity type generation
 * 0.2.1 - Use of EntityGenerator back-end
 *         Refactoring of argument parsing to Args class.
 * 0.2.2 - New entity model
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using EG = BdsSoft.SharePoint.Linq.Tools.EntityGenerator;
using BdsSoft.SharePoint.Linq.Tools.EntityGenerator;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Diagnostics;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.SpMetal
{
    class Program
    {
        /// <summary>
        /// Entry point for SpMetal.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        static void Main(string[] args)
        {
            //
            // Title including assembly version number.
            //
            Console.WriteLine("Bart De Smet SpMetal SharePoint List Definition Export version {0}", Assembly.GetEntryAssembly().GetName().Version);
            Console.WriteLine("Copyright (C) Bart De Smet 2007. All rights reserved.\n");

            //
            // Parse arguments.
            //
            Args a = Args.Parse(args);

            //
            // Invalid arguments: display help information and exit.
            //
            if (a == null)
            {
                ShowSyntaxInfo();
                return;
            }

            //
            // Entity generator.
            //
            EG.EntityGenerator gen = new EG.EntityGenerator(
                                         new EG.EntityGeneratorArgs()
                                         {
                                             RunMode = a.RunMode,
                                             Connection = new Connection()
                                                          {
                                                              CustomAuthentication = a.User != null,
                                                              Url = a.Url,
                                                              User = a.User,
                                                              Password = a.Password,
                                                              Domain = a.Domain
                                                          },
                                             Namespace = a.Namespace,
                                             Language = (a.Language == "VB" ? EG.Language.VB : EG.Language.CSharp)
                                         }
                                     );

            //
            // Get SPML first.
            //
            XmlDocument spml = new XmlDocument();

            //
            // Check run mode: online or offline.
            //
            if ((a.RunMode & RunMode.Online) == RunMode.Online)
            {
                try
                {
                    spml.InnerXml = gen.GenerateSpml(a.Context, a.List).InnerXml;
                }
                catch (EntityGeneratorException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else if ((a.RunMode & RunMode.Offline) == RunMode.Offline)
            {
                try
                {
                    spml.InnerXml = File.ReadAllText(a.In);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
            else
                Debug.Assert(false);

            //
            // Just save the SPML.
            //
            if ((a.RunMode & RunMode.Export) == RunMode.Export)
            {
                //
                // Write to output file.
                //
                using (FileStream fs = File.Open(a.Xml, FileMode.Create, FileAccess.Write))
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    using (XmlWriter writer = XmlWriter.Create(fs, settings))
                    {
                        spml.WriteTo(writer);
                    }
                }
            }
            //
            // Create and save entity code.
            //
            else if ((a.RunMode & RunMode.CodeGen) == RunMode.CodeGen)
            {
                //
                // Generate code in the appropriate language.
                //
                CodeCompileUnit compileUnit = gen.GenerateCode(spml);
                CodeDomProvider cdp = CodeDomProvider.CreateProvider(a.Language);
                StringBuilder code = new StringBuilder();
                TextWriter tw = new StringWriter(code);
                cdp.GenerateCodeFromCompileUnit(compileUnit, tw, null);

                //
                // Infer file name from list name if not specified on the command-line (v0.2.0.0).
                //
                if (a.Code == null || a.Code.Length == 0)
                {
                    string file;

                    if (!string.IsNullOrEmpty(a.Context))
                        file = a.Context + "SharePointDataContext";
                    else
                    {
                        XmlAttribute ctx = spml["SharePointDataContext"].Attributes["Name"];
                        if (ctx != null && !string.IsNullOrEmpty(ctx.Value))
                            file = ctx.Value + "SharePointDataContext";
                        else if (!string.IsNullOrEmpty(a.List))
                            file = a.List; //TODO: non-GUID file name
                        else
                            file = Path.GetRandomFileName();
                    }

                    file += (a.Language == "CS" ? ".cs" : ".vb");
                    foreach (char c in Path.GetInvalidFileNameChars())
                        file = file.Replace(c.ToString(), "");
                    a.Code = file;
                }

                //
                // Write to output file.
                //
                using (StreamWriter sw = File.CreateText(a.Code))
                {
                    sw.WriteLine(code.ToString());
                }
            }
            else
                Debug.Assert(false);
        }

        /// <summary>
        /// Hooks up event handler for the entity generator in order to report progress on the console output.
        /// </summary>
        /// <param name="gen">Entity generator to hook up event handlers for.</param>
        private static void SetupEventHandlers(EG.EntityGenerator gen)
        {
            gen.Connecting += delegate(object sender, ConnectingEventArgs e) { Console.Write("Connecting to server... "); };
            gen.Connected += delegate(object sender, ConnectedEventArgs e)
            {
                if (e.Succeeded)
                    Console.WriteLine("Done");
                else
                {
                    Console.WriteLine("Failed\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Exception.Message);
                    Console.ResetColor();
                    Environment.Exit(-1);
                }
            };
            gen.LoadingSchema += delegate(object sender, LoadingSchemaEventArgs e) { Console.Write("Loading schema... "); };
            gen.LoadedSchema += delegate(object sender, LoadedSchemaEventArgs e)
            {
                if (e.Succeeded)
                    Console.WriteLine("Done\n");
                else
                {
                    Console.WriteLine("Failed\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Exception.Message);
                    Console.ResetColor();
                    Environment.Exit(-1);
                }
            };
            gen.ExportingSchema += delegate(object sender, ExportingSchemaEventArgs e) { Console.Write("Processing list {0} ({1}) version {2}... ", e.List, e.Identifier, e.Version); };
            gen.ExportedSchema += delegate(object sender, ExportedSchemaEventArgs e)
            {
                if (e.Succeeded)
                {
                    Console.WriteLine("Done");
                    Console.WriteLine("Exported {0} properties\n", e.PropertyCount);
                }
                else
                {
                    Console.WriteLine("Failed\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Exception.Message);
                    Console.ResetColor();
                    Environment.Exit(-1);
                }
            };
        }

        /// <summary>
        /// Shows syntax information on the console, in response to a -? or -help call or when invalid arguments have been supplied.
        /// </summary>
        private static void ShowSyntaxInfo()
        {
            Console.WriteLine("No inputs specified\n");

            string file = Assembly.GetEntryAssembly().GetName().Name + ".exe";
            Console.WriteLine("Usage: {0} [options]", file);

            //Console.WriteLine("Usage: {0} -url:<url> [-list:<list>]", file);
            //Console.WriteLine("       {0} [-in:<file>] [-xml:<file>]", new string(' ', file.Length));
            //Console.WriteLine("       {0} [-code:<file> [-language:<lang>] [-namespace:<ns>]]", new string(' ', file.Length));
            //Console.WriteLine("       {0} [-user:<user> -password:<password> [-domain:<domain>]]", new string(' ', file.Length));
            Console.WriteLine();

            Console.WriteLine("Options:");
            Console.WriteLine("  -url:<url>            URL to the root of the SharePoint site");
            Console.WriteLine("  -user:<user>          User name for connection to SharePoint site");
            Console.WriteLine("  -password:<password>  Password for connection to SharePoint site");
            Console.WriteLine("  -domain:<domain>      Domain for connection to SharePoint site");
            Console.WriteLine("  -list:<list>          Name of the list to export (* = all lists)");
            Console.WriteLine("  -context:<ctx>        Name of the context to create");
            Console.WriteLine("  -in:<file>            Input file with SPML for code generation");
            Console.WriteLine("  -xml:<file>           Output file for SPML generation");
            Console.WriteLine("  -code:<file>          Output file for code generation (= -out:<file>)");
            Console.WriteLine("  -language:<lang>      Code language used for output: VB or CS (default)");
            Console.WriteLine("  -namespace:<ns>       Namespace to put generated code in");
            Console.WriteLine();
            Console.WriteLine("Syntax:");
            Console.WriteLine("  {0} [{{online}}|{{offline}}]", file);
            Console.WriteLine("  {online}  := -url:<url> -list:<list> [-context:<ctx>] {connect} {option}");
            Console.WriteLine("  {offline} := -in:<file> {codegen}");
            Console.WriteLine("  {connect} := [-user:<user> -password:<password> [-domain:<domain>]]");
            Console.WriteLine("  {option}  := [{codegen}|{export}]");
            Console.WriteLine("  {codegen} := [-code:<file>] [-language:<lang>] [-namespace:<ns>]");
            Console.WriteLine("  {export}  := [-xml:<file>]");
            Console.WriteLine();
            Console.WriteLine("Samples:");
            Console.WriteLine("To export one list to code (online codegen).");
            Console.WriteLine("  {0} -url:http://wss -list:Products -language:CS -code:Products.cs", file);
            Console.WriteLine();
            Console.WriteLine("To generate an SPML mapping definition for all lists (online export).");
            Console.WriteLine("  {0} -url:http://wss3demo -list:* -xml:Northwind.dbml", file);
            Console.WriteLine();
            Console.WriteLine("To generate code for an SPML mapping definition (offline codegen).");
            Console.WriteLine("  {0} -in:Northwind.spml -language:CS -code:Northwind.cs", file);
        }
    }
}