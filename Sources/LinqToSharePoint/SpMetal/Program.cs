/*
 * LINQ-to-SharePoint
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
 */

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
            Args a = Parse(args);

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
                                         new EG.EntityGeneratorArgs() { 
                                             Domain = a.Domain, 
                                             User = a.User, 
                                             Password = a.Password, 
                                             Url = a.Url,
                                             Namespace = a.Namespace }
                                         );
            
            //
            // Set up event handlers for status information.
            //
            SetupEventHandlers(gen);

            //
            // Generate code in the appropriate language.
            //
            CodeCompileUnit compileUnit = gen.Generate(a.List);
            CodeDomProvider cdp = CodeDomProvider.CreateProvider(a.Language);
            StringBuilder code = new StringBuilder();
            TextWriter tw = new StringWriter(code);
            cdp.GenerateCodeFromCompileUnit(compileUnit, tw, null);

            //
            // Infer file name from list name if not specified on the command-line (v0.2.0.0).
            //
            if (a.File == null || a.File.Length == 0)
            {
                string file = a.List + (a.Language.Replace("CS", "C#") == "C#" ? ".cs" : ".vb");
                foreach (char c in Path.GetInvalidFileNameChars())
                    file = file.Replace(c.ToString(), "");
                a.File = file;
            }

            //
            // Write to output file.
            //
            Console.Write("Writing file {0}... ", a.File);

            using (StreamWriter sw = File.CreateText(a.File))
            {
                sw.WriteLine(code.ToString());
                Console.WriteLine("Done");
            }
        }

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

        #region Argument parsing

        private static void ShowSyntaxInfo()
        {
            Console.WriteLine("No inputs specified\n");

            string file = Assembly.GetEntryAssembly().GetName().Name + ".exe";
            Console.WriteLine("Usage: {0} -url:<url> -list:<list> [-out:<file>]", file);
            Console.WriteLine("       {0} [-language:<language>] [-namespace:<namespace>", new string(' ', file.Length));
            Console.WriteLine("       {0} [-user:<user> -password:<password> [-domain:<domain>]]", new string(' ', file.Length));
            Console.WriteLine();

            Console.WriteLine("  -url:<url>              URL to the root of the SharePoint site");
            Console.WriteLine("  -list:<list>            Name of the list");
            Console.WriteLine("  -out:<file>             Output file");
            Console.WriteLine("  -language:<language>    Code language used for output (VB or CS)");
            Console.WriteLine("                          (Default: CS)");
            Console.WriteLine("  -namespace:<namespace>  Namespace to put generated code in");
            Console.WriteLine();

            Console.WriteLine("  -user:<user>            User name for connection to SharePoint site");
            Console.WriteLine("  -password:<password>    Password for connection to SharePoint site");
            Console.WriteLine("  -domain:<domain>        Domain for connection to SharePoint site");
        }

        /// <summary>
        /// Parses the given command-line arguments.
        /// </summary>
        /// <param name="args">Arguments to be parsed.</param>
        /// <returns>An instance of Args containing the argument values; null if the argument list is invalid.</returns>
        private static Args Parse(string[] args)
        {
            //
            // Valid argument count, length and prefixes?
            //
            if (!CheckArgs(args))
                return null;

            Args res = new Args();

            //
            // Url shouldn't be empty and should start with either http:// or https://.
            //
            string url = FindArg(args, "url");
            if (url == null || url.Length == 0)
                return null;
            res.Url = url;
            url = url.ToLower();
            if (!(url.StartsWith("http://") && url.Length > 7) && !(url.StartsWith("https://") && url.Length > 8))
                return null;

            //
            // List shouldn't be empty.
            //
            string list = FindArg(args, "list");
            if (list == null || list.Length == 0)
                return null;
            res.List = list;

            //
            // Output language can be set optionally and should be CS or VB (case-insensitive).
            //
            string language = FindArg(args, "language");
            if (language != null && language.Length != 0)
            {
                language = language.ToUpper();

                if (language != "CS" && language != "VB" && language != "C#")
                    return null;
                else
                    res.Language = language.Replace("CS", "C#");
            }
            else
                res.Language = "C#";

            //
            // Out can optionally be empty; in that case, take the lists's name suffixed with the language extension.
            // v0.2.0.0 - File name will be deferred from the loaded list name, not the -list parameter which could be a GUID too.
            //
            res.File = FindArg(args, "out");

            //
            // v0.2.1.0 - Namespace support.
            //
            res.Namespace = FindArg(args, "namespace");

            //
            // Only process authentication arguments if a user argument is present.
            //
            string user = FindArg(args, "user");
            if (user != null && user.Length != 0)
            {
                //
                // Password required if user has been specified. Password can be the empty string.
                //
                string password = FindArg(args, "password");
                if (password != null)
                {
                    res.User = user;
                    res.Password = password;

                    //
                    // Optional domain name.
                    //
                    string domain = FindArg(args, "domain");
                    if (domain != null && domain.Length != 0)
                        res.Domain = domain;
                }
                else
                    return null;
            }

            return res;
        }

        /// <summary>
        /// Checks that all arguments have a valid prefix ('/' or '-') and have a suitable minimal length.
        /// </summary>
        /// <param name="args">Arguments to be checked.</param>
        /// <returns>True if valid; false otherwise.</returns>
        private static bool CheckArgs(string[] args)
        {
            //
            // Minimum required: -url and -list
            //
            if (args.Length < 2)
                return false;

            foreach (string arg in args)
            {
                //
                // Check for valid prefix.
                //
                if (!arg.StartsWith("-") && !arg.StartsWith("/"))
                    return false;

                //
                // Minimal length as in "-out:x"
                //
                if (arg.Length <= 5)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Finds an argument with the specified name.
        /// </summary>
        /// <param name="args">Arguments collection to search in.</param>
        /// <param name="prefix">Name of the argument requested.</param>
        /// <returns>Value of the argument, i.e. the portion after the ':' without optional double quotes; null if the argument isn't found.</returns>
        private static string FindArg(string[] args, string prefix)
        {
            //
            // Prefix should end with :
            //
            prefix += ":";
            int n = prefix.Length;

            foreach (string arg in args)
            {
                //
                // Trim the '-' or '/'.
                //
                string a = arg.Substring(1);

                //
                // Case-insensitive prefix-match. Returns the "-trimmed version of the argument value.
                //
                if (a.ToLower().StartsWith(prefix) && a.Length >= n)
                    return a.Substring(n).Trim('\"');
            }

            return null;
        }

        #endregion
    }
}