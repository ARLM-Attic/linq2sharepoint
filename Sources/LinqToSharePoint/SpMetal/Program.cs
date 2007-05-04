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
 */

/*
 * TODO:
 * - Lookup and LookupMulti support
 * - LookupField attribute parameter from ShowField
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

namespace BdsSoft.SharePoint.Linq.Tools.SpMetal
{
    class Program
    {
        static List<string> enums = new List<string>();

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
                Console.WriteLine("No inputs specified\n");

                string file = Assembly.GetEntryAssembly().GetName().Name + ".exe";
                Console.WriteLine("Usage: {0} -url:<url> -list:<list> [-out:<file>] [-language:<language>]", file);
                Console.WriteLine("       {0} [-user:<user> -password:<password> [-domain:<domain>]]", new string(' ', file.Length));
                Console.WriteLine();

                Console.WriteLine("  -url:<url>            URL to the root of the SharePoint site");
                Console.WriteLine("  -list:<list>          Name of the list");
                Console.WriteLine("  -out:<file>           Output file");
                Console.WriteLine("  -language:<language>  Code language used for output (VB or CS)");
                Console.WriteLine("                        (Default: CS)");
                Console.WriteLine();

                Console.WriteLine("  -user:<user>          User name for connection to SharePoint site");
                Console.WriteLine("  -password:<password>  Password for connection to SharePoint site");
                Console.WriteLine("  -domain:<domain>      Domain for connection to SharePoint site");

                return;
            }

            //
            // Entity generator.
            //
            EntityGenerator gen = new EntityGenerator();
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

            //
            // Create a queue of entities that allows to process Lookup field entities.
            //
            Queue<Entity> entities = new Queue<Entity>();
            HashSet<string> encounteredEntities = new HashSet<string>();
            Dictionary<string, string> map = new Dictionary<string, string>();

            //
            // Generate the requested entity.
            //
            HashSet<string> forbiddenTypeNames = new HashSet<string>();
            Entity entity = gen.Generate(a);

            //
            // Infer file name from list name if not specified on the command-line (v0.2.0.0).
            //
            if (a.File == null || a.File.Length == 0)
            {
                string file = entity.Name + (a.Language == "CS" ? ".cs" : ".vb");
                foreach (char c in Path.GetInvalidFileNameChars())
                    file = file.Replace(c.ToString(), "");
                a.File = file;
            }

            //
            // Build the code; import the namespaces required.
            //
            StringBuilder allCode = new StringBuilder();
            if (a.Language == "CS")
                allCode.Append(IMPORTS_CS);
            else
                allCode.Append(IMPORTS_VB);

            //
            // Resolve all Lookup field references, recursively.
            //
            entities.Enqueue(entity);
            encounteredEntities.Add(entity.Name);
            while (entities.Count > 0)
            {
                entity = entities.Dequeue();

                //
                // Patch the entity's Lookup fields by generating sub-entities.
                //
                StringBuilder code = new StringBuilder();
                code.Append(entity.Code);
                foreach (string patch in entity.Lookups.Keys)
                {
                    string lookupList = entity.Lookups[patch];
                    string lookupEntityName;
                    if (map.ContainsKey(lookupList))
                    {
                        lookupEntityName = map[lookupList];
                    }
                    else
                    {
                        a.List = lookupList;
                        Entity lookupEntity = gen.Generate(a);
                        lookupEntityName = lookupEntity.Name;
                        map.Add(lookupList, lookupEntity.Name);
                        entities.Enqueue(lookupEntity);
                    }
                    code.Replace(patch, lookupEntityName);
                }
                entity.Code = code.ToString();

                //
                // Append the code to the accumulated code buffer.
                //
                allCode.Append(entity.Code);
            }

            foreach (string e in gen.Enums)
                allCode.Append(e);

            //
            // Write to output file.
            //
            Console.Write("Writing file {0}... ", a.File);

            using (StreamWriter sw = File.CreateText(a.File))
            {
                sw.WriteLine(allCode.ToString());
                Console.WriteLine("Done");
            }
        }

        static string IMPORTS_VB = 
@"Imports System
Imports BdsSoft.SharePoint.Linq
";

        static string IMPORTS_CS =
@"using System;
using BdsSoft.SharePoint.Linq;
";
    }
}