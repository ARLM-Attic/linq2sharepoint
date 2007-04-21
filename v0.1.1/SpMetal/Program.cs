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
    /// <summary>
    /// Internal helper class to parse and validate the command-line arguments.
    /// </summary>
    internal class Args
    {
        /// <summary>
        /// Url to the SharePoint site.
        /// </summary>
        public string Url;

        /// <summary>
        /// Name of the list on the SharePoint site to query for.
        /// </summary>
        public string List;

        /// <summary>
        /// Output file (optional).
        /// </summary>
        /// <remarks>If no output file is specified, the list's name will be used with an extension of .cs.</remarks>
        public string File;

        /// <summary>
        /// User name to connect to the SharePoint web services (optional).
        /// </summary>
        /// <remarks>If not specified, integrated authentication will be used using default network credentials.</remarks>
        public string User;

        /// <summary>
        /// Password to connect to the SharePoint web services (optional).
        /// </summary>
        public string Password;

        /// <summary>
        /// Domain name to connect to the SharePoint web services (optional).
        /// </summary>
        public string Domain;

        /// <summary>
        /// Output code language.
        /// </summary>
        public string Language;

        /// <summary>
        /// Parses the given command-line arguments.
        /// </summary>
        /// <param name="args">Arguments to be parsed.</param>
        /// <returns>An instance of Args containing the argument values; null if the argument list is invalid.</returns>
        internal static Args Parse(string[] args)
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

                if (language != "CS" && language != "VB")
                    return null;
                else
                    res.Language = language;
            }
            else
                res.Language = "CS";

            //
            // Out can optionally be empty; in that case, take the lists's name suffixed with the language extension.
            //
            string file = FindArg(args, "out");
            if (file == null || file.Length == 0)
            {
                file = list + (res.Language == "CS" ? ".cs" : ".vb");
                foreach (char c in Path.GetInvalidFileNameChars())
                    file = file.Replace(c.ToString(), "");
            }
            res.File = file;

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
    }

    class Program
    {
        static List<string> enums = new List<string>();

        static string language;

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
            // Set correct language code fragments.
            //
            if ((language = a.Language) == "CS")
            {
                ENUM = ENUM_CS;
                CLASS = CLASS_CS;
                PROP = PROP_CS;
                CHOICEHELPERPROP = CHOICEHELPERPROP_CS;
            }
            else
            {
                ENUM = ENUM_VB;
                CLASS = CLASS_VB;
                PROP = PROP_VB;
                CHOICEHELPERPROP = CHOICEHELPERPROP_VB;
            }

            //
            // List definition XML; will be downloaded from server.
            //
            XmlNode lst;

            //
            // Try to connect to server.
            //
            try
            {
                Console.Write("Connecting to server... ");

                //
                // Create proxy object referring to the SharePoint lists.asmx service on the specified server.
                //
                Lists l = new Lists();
                l.Url = a.Url.TrimEnd('/') + "/_vti_bin/lists.asmx";

                //
                // Integrated authentication using current network credentials.
                //
                if (a.User == null)
                    l.Credentials = CredentialCache.DefaultNetworkCredentials;
                //
                // Use specified credentials.
                //
                else
                {
                    if (a.Domain == null)
                        l.Credentials = new NetworkCredential(a.User, a.Password);
                    else
                        l.Credentials = new NetworkCredential(a.User, a.Password, a.Domain);
                }

                Console.WriteLine("Done");

                //
                // Load schema from server using lists.asmx web service.
                //
                Console.Write("Loading schema... ");
                lst = l.GetList(a.List);

                Console.WriteLine("Done\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Environment.Exit(-1);
                return;
            }

            //
            // Get general information of the list.
            //
            string listName = GetFriendlyName((string)lst.Attributes["Title"].Value);
            string listDescription = (string)lst.Attributes["Description"].Value;
            if (listDescription == "")
                listDescription = null;
            Guid listID = new Guid((string)lst.Attributes["ID"].Value);
            int version = int.Parse(lst.Attributes["Version"].Value);
            string path = (string)lst.Attributes["RootFolder"].Value;

            Console.Write("Processing list {0} ({1}) version {2}... ", listName, listID, version);

            //
            // Get fields.
            //
            XmlElement fields = lst["Fields"];
            StringBuilder props = new StringBuilder();
            int n = 0;
            foreach (XmlNode c in fields.ChildNodes)
            {
                //
                // Field ID (GUID).
                //
                XmlAttribute aID = c.Attributes["ID"];
                string id = "";
                if (aID != null)
                    id = new Guid(aID.Value).ToString();

                //
                // Field name.
                //
                string name = (string)c.Attributes["Name"].Value;
                string displayName = (string)c.Attributes["DisplayName"].Value;

                //
                // Field description.
                //
                XmlAttribute aDescription = c.Attributes["Description"];
                string description = null;
                if (aDescription != null)
                    description = (string)aDescription.Value;

                //
                // Field hidden?
                //
                XmlAttribute aHidden = c.Attributes["Hidden"];
                bool hidden = false;
                if (aHidden != null)
                    hidden = bool.Parse(aHidden.Value);

                //
                // Field read-only?
                //
                XmlAttribute aReadOnly = c.Attributes["ReadOnly"];
                bool readOnly = false;
                if (aReadOnly != null)
                    readOnly = bool.Parse(aReadOnly.Value);

                //
                // Field type.
                //
                string type = (string)c.Attributes["Type"].Value;
                bool calc = false;

                //
                // Calculated field. Use underlying type for mapping.
                //
                if (type == "Calculated")
                {
                    type = (string)c.Attributes["ResultType"].Value;
                    calc = true;
                }

                //
                // Field inherited from base type?
                //
                XmlAttribute aFromBaseType = c.Attributes["FromBaseType"];
                bool fromBaseType = false;
                if (aFromBaseType != null)
                    fromBaseType = bool.Parse(aFromBaseType.Value);

                //
                // Export only field that aren't hidden and aren't inherited from the base type.
                //
                if (!hidden && !fromBaseType)
                {
                    //
                    // Get underlying .NET type textual name for C# class generation.
                    // Additional field might be required for multi-choice fields with fill-in choice.
                    //
                    bool additional;
                    string bclType = GetType(c, out additional);

                    //
                    // Is the underlying type recognized and supported by the mapper?
                    //
                    if (bclType != null)
                    {
                        //
                        // Read-only and calculated field require additional mapping attribute parameters.
                        //
                        StringBuilder extra = new StringBuilder();
                        if (readOnly)
                            extra.AppendFormat(", ReadOnly{0}true", language == "CS" ? " = " : ":=");
                        if (calc)
                            extra.AppendFormat(", Calculated{0}true", language == "CS" ? " = " : ":=");

                        //
                        // Create helper field and refer to it in case a multi-choice fields with fill-in choice was detected.
                        // The helper field has the same name as the .NET type (which will be an enum) suffixed with "Other".
                        //
                        string helper = null;
                        if (additional)
                        {
                            helper = GetFriendlyName(bclType) + "Other";
                            extra.AppendFormat(", OtherChoice{1}\"{0}\"", helper, language == "CS" ? " = " : ":=");
                        }

                        //
                        // Generate a property for the current field and append it to the properties output string.
                        //
                        props.AppendFormat(PROP, (description ?? displayName), bclType, GetFriendlyName(displayName), XmlConvert.DecodeName(name), type, id, extra.ToString());

                        //
                        // Generate additional helper property if needed.
                        //
                        if (additional)
                            props.AppendFormat(CHOICEHELPERPROP, displayName, helper, XmlConvert.DecodeName(name), id);

                        //
                        // Keep field count.
                        //
                        n++;
                    }
                }
            }

            //
            // Print statistical information.
            //
            Console.WriteLine("Done");
            Console.WriteLine("Exported {0} properties and {1} helper enums", n, enums.Count);
            Console.WriteLine();

            //
            // Build file with class definition containing the properties and the helper enums.
            //
            Console.Write("Writing file {0}... ", a.File);
            StringBuilder output = new StringBuilder();
            output.AppendFormat(CLASS, listName, props.ToString(), listName, listID.ToString(), version, (listDescription ?? listName), path);

            foreach (string e in enums)
                output.Append(e);

            //
            // Write to output file.
            //
            using (StreamWriter sw = File.CreateText(a.File))
            {
                sw.WriteLine(output.ToString());
                Console.WriteLine("Done");
            }
        }

        /// <summary>
        /// Get a friendly name for a field, usable in C# as a property name.
        /// </summary>
        /// <param name="name">Name to create a friendly name for.</param>
        /// <returns>Friendly name for given name.</returns>
        static string GetFriendlyName(string name)
        {
            //
            // Find parts of the name, separated by a space, dash or underscore, and create a Pascal-cased name.
            //
            StringBuilder sb = new StringBuilder();
            foreach (string s in name.Split(' ', '-', '_'))
            {
                //
                // Keep only letters and digits.
                //
                string s2 = SanitizeString(s);

                //
                // Process string if not empty.
                //
                if (s2 != String.Empty)
                {
                    //
                    // First letter of a part should be capitalized.
                    //
                    sb.Append(char.ToUpper(s2[0]));

                    //
                    // Leave original casing for the remainder.
                    //
                    if (s2.Length > 1)
                        sb.Append(s2.Substring(1));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Keeps only letters and digits from a given string.
        /// </summary>
        /// <param name="s">String to be sanitized.</param>
        /// <returns>Sanitized string.</returns>
        static string SanitizeString(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
                if (char.IsLetterOrDigit(c))
                    sb.Append(c);

            return sb.ToString();
        }

        /// <summary>
        /// Get C# string represention of the .NET type corresponding with a SharePoint list field definition.
        /// </summary>
        /// <param name="c">SharePoint list field definition.</param>
        /// <param name="additional">Output parameter to indicate the need for an additional helper field, used in multi-choice fields with fill-in option.</param>
        /// <returns>String representation of the .NET type corresponding with the given SharePoint list field definition.</returns>
        static string GetType(XmlNode c, out bool additional)
        {
            //
            // Default case: no additional helper field needed.
            //
            additional = false;

            //
            // Find SharePoint-type of the field.
            //
            string type = (string)c.Attributes["Type"].Value;

            //
            // Check whether the field is required or not. Will be used to make value types nullable if not required.
            //
            XmlAttribute aRequired = c.Attributes["Required"];
            bool required = false;
            if (aRequired != null)
                required = bool.Parse(aRequired.Value);

            //
            // Convert calculated type to underlying result type for representation in the entity type.
            //
            if (type == "Calculated")
                type = (string)c.Attributes["ResultType"].Value;

            //
            // Analyze the types.
            //
            switch (type)
            {
                //
                // Boolean == bool or bool?
                //
                case "Boolean":
                    return (language == "CS" ? "bool" + (required ? "" : "?") : String.Format((required ? "{0}" : "Nullable(Of {0})"), "Boolean"));
                //
                // Text == string
                // Note == string
                //
                case "Text":
                case "Note":
                    return (language == "CS" ? "string" : "String");
                //
                // DateTime == System.DateTime
                //
                case "DateTime":
                    return (language == "CS" ? "System.DateTime" + (required ? "" : "?") : String.Format((required ? "{0}" : "Nullable(Of {0})"), "System.DateTime"));
                //
                // Number == double or double?
                // Currency == double or double?
                //
                case "Number":
                case "Currency":
                    return (language == "CS" ? "double" + (required ? "" : "?") : String.Format((required ? "{0}" : "Nullable(Of {0})"), "Double"));
                //
                // URL == Url (helper object from LINQ-to-SharePoint)
                //
                case "URL":
                    return "Url";
                //
                // Choice and MultiChoice are mapped on helper enums, (optionally) together with a helper string field for fill-in choices.
                // The type is made nullable if a value is not required.
                //
                case "Choice":
                case "MultiChoice":
                    {
                        //
                        // Multi-choice values are mapped onto flag enums. A variable is kept for the flag value which should be a power of two.
                        //
                        bool flags = type.StartsWith("Multi");
                        int flagValue = 1;

                        //
                        // Populate the enum with the choices available in the list field definition.
                        //
                        StringBuilder sb = new StringBuilder();
                        HashSet<string> choices = new HashSet<string>();
                        foreach (XmlNode choice in c["CHOICES"])
                        {
                            //
                            // Get choice text and find the friendly name that will be used for the field in C#.
                            //
                            string ct = choice.InnerText;
                            string s = GetFriendlyName(ct);

                            //
                            // Detect duplicate values; shouldn't occur in most cases.
                            //
                            int j = 0;
                            while (choices.Contains(s))
                                s += (++j).ToString();
                            choices.Add(s);

                            //
                            // Add a enum field mapping in case the C# name doesn't match the underlying SharePoint choice value textual represention.
                            //
                            if (ct != s)
                                sb.AppendFormat(language == "CS" ? "[Choice(\"{0}\")] " : "    <Choice(\"{0}\")> ", ct);

                            //
                            // Set flag value in case a flags enum is generated; update the flags value by multiplying it by two.
                            //
                            sb.Append((language == "VB" && ct == s ? "    " : "") + s + (flags ? " = " + flagValue : "") + (language == "CS" ? ", " : "\r\n"));
                            flagValue *= 2;
                        }

                        //
                        // Additional fill-in field needed if FillInChoice is set.
                        //
                        additional = bool.Parse((string)c.Attributes["FillInChoice"].Value);

                        //
                        // Add enum to the enums collection.
                        //
                        string name = GetFriendlyName((string)c.Attributes["DisplayName"].Value);
                        enums.Add(String.Format(ENUM, name, sb.ToString().TrimEnd(' ', ','), flags ? (language == "CS" ? "[Flags] " : "<Flags()> ") : ""));

                        //
                        // Return enum name, possibly nullable.
                        //
                        return String.Format((required ? "{0}" : "Nullable(Of {0})"), name);
                    }
                //
                // Currently no support for User and UserMulti fields.
                //
                case "User":
                case "UserMulti":
                //
                // Ignore other types too.
                //
                default:
                    return null;
            }
        }

        static string ENUM;
        static string CLASS;
        static string PROP;
        static string CHOICEHELPERPROP;

        /// <summary>
        /// Skeleton for enums.
        /// </summary>
        /// <example><![CDATA[
        /// [Flags] enum Beverages : uint { Coke = 1, Beer = 2, Milk = 4, Coffee = 8, Wine = 16 }
        /// enum Colors { Black, Red, Blue, Yellow, Green, White }
        /// ]]></example>
        static string ENUM_CS =
@"
{2}enum {0} : uint {{ {1} }}
";

        /// <summary>
        /// Skeleton for enums.
        /// </summary>
        /// <example><![CDATA[
        /// [Flags] Enum Beverages As UInteger
        ///    Coke = 1
        ///    Beer = 2
        ///    Milk = 4
        ///    Coffee = 8
        ///    Wine = 16
        /// End Enum
        /// Enum Colors
        ///    Black
        ///    Red
        ///    Blue
        ///    Yellow
        ///    Green
        ///    White
        /// End Enum
        /// ]]></example>
        static string ENUM_VB =
@"
{2}Enum {0} As UInteger
{1}
End Enum
";

        /// <summary>
        /// Skeleton for the entity class.
        /// </summary>
        /// <example><![CDATA[
        /// using System;
        /// using BdsSoft.SharePoint.Linq;
        /// 
        /// /// <summary>
        /// /// Members
        /// /// </summary>
        /// [List("Demo", Id = "34c90895-fbf3-4da7-a260-4b3ddc67146d", Version = 36, Path = "/Lists/Demo")]
        /// class Demo
        /// {
        ///     //
        ///     // Fields (see PROP_CS)
        ///     //
        /// }
        /// ]]></example>
        static string CLASS_CS =
@"using System;
using BdsSoft.SharePoint.Linq;

/// <summary>
/// {5}
/// </summary>
[List(""{2}"", Id = ""{3}"", Version = {4}, Path = ""{6}"")]
class {0}
{{{1}}}
";
        /// <summary>
        /// Skeleton for the entity class.
        /// </summary>
        /// <example><![CDATA[
        /// Imports System
        /// Imports BdsSoft.SharePoint.Linq
        /// 
        /// ''' <summary>
        /// ''' Members
        /// ''' </summary>
        /// <List("Demo", Id:="34c90895-fbf3-4da7-a260-4b3ddc67146d", Version:=36, Path:="/Lists/Demo")> _
        /// Class Demo
        ///     '
        ///     ' Fields (see PROP_VB)
        ///     '
        /// End Class
        /// ]]></example>
        static string CLASS_VB =
@"Imports System
Imports BdsSoft.SharePoint.Linq

''' <summary>
''' {5}
''' </summary>
<List(""{2}"", Id:=""{3}"", Version:={4}, Path:=""{6}"")> _
Class {0}
{1}
End Class
";

        /// <summary>
        /// Skeleton for entity properties with field mappings.
        /// </summary>
        /// <example><![CDATA[
        /// /// <summary>
        /// /// First name
        /// /// </summary>
        /// [Field("First name", FieldType.Text, ID = "f5164cc0-8a1b-423d-8669-e86bb65a3118")]
        /// public string FullName { get; set; }
        /// ]]></example>
        static string PROP_CS =
@"
    /// <summary>
    /// {0}
    /// </summary>
    [Field(""{3}"", FieldType.{4}, Id = ""{5}""{6})]
    public {1} {2} {{ get; set; }}
";

        /// <summary>
        /// Skeleton for entity properties with field mappings.
        /// </summary>
        /// <example><![CDATA[
        /// Private _FirstName As String
        /// 
        /// ''' <summary>
        /// ''' First name
        /// ''' </summary>
        /// <Field("First name", FieldType.Text, ID:="f5164cc0-8a1b-423d-8669-e86bb65a3118")> _
        /// Public Property FullName() As String
        ///     Get
        ///         Return _FirstName
        ///     End Get
        ///     Set(ByVal value As String)
        ///         _FirstName = value
        ///     End Set
        /// End Property
        /// ]]></example>
        static string PROP_VB =
@"
    Private _{2} As {1}

    ''' <summary>
    ''' {0}
    ''' </summary>
    <Field(""{3}"", FieldType.{4}, Id:=""{5}""{6})> _
    Public Property {2}() As {1}
        Get
            Return _{2}
        End Get
        Set(ByVal value As {1})
            _{2} = value
        End Set
    End Property
";

        /// <summary>
        /// Skeleton for helper fields for multi-choice data types.
        /// </summary>
        /// <example><![CDATA[
        /// /// <summary>
        /// /// Beverages helper for value Beverages.Other
        /// /// </summary>
        /// [Field(""{2}"", FieldType.Text, Id = ""{3}"")]
        /// public string BeveragesOtherValue { get; set; }
        /// ]]></example>
        static string CHOICEHELPERPROP_CS =
@"
    /// <summary>
    /// {0} 'Fill-in' value
    /// </summary>
    [Field(""{2}"", FieldType.Text, Id = ""{3}"")]
    public string {1} {{ get; set; }}
";

        /// <summary>
        /// Skeleton for helper fields for multi-choice data types.
        /// </summary>
        /// <example><![CDATA[
        /// ''' <summary>
        /// ''' Beverages helper for value Beverages.Other
        /// ''' </summary>
        /// <Field(""{2}"", FieldType.Text, Id:=""{3}"")> _
        /// public string BeveragesOtherValue { get; set; }
        /// ]]></example>
        static string CHOICEHELPERPROP_VB =
@"
    Private _{1} As String

    ''' <summary>
    ''' {0} 'Fill-in' value
    ''' </summary>
    <Field(""{2}"", FieldType.Text, Id:=""{3}"")> _
    Public Property {1} As String
        Get
            Return _{1}
        End Get
        Set(ByVal value As String)
            _{1} = value
        End Set
    End Property
";
    }
}