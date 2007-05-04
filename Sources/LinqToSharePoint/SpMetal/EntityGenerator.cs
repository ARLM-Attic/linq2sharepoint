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
 * 0.2.0 - Restructuring of class files in project
 *         Hosting model with events
 */

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;

namespace BdsSoft.SharePoint.Linq.Tools.SpMetal
{
    class Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, string> Lookups { get; set; }
    }

    class ConnectingEventArgs : EventArgs
    {
        public ConnectingEventArgs(string url)
            : base()
        {
            Url = url;
        }

        public string Url { get; set; }
    }

    class ConnectedEventArgs : EventArgs
    {
        public ConnectedEventArgs()
            : base()
        {
            Succeeded = true;
        }

        public ConnectedEventArgs(Exception ex)
            : base()
        {
            Succeeded = false;
            Exception = ex;
        }

        public bool Succeeded { get; set; }
        public Exception Exception { get; set; }
    }

    class LoadingSchemaEventArgs : EventArgs
    {
        public LoadingSchemaEventArgs(string list)
            : base()
        {
            List = list;
        }

        public string List { get; set; }
    }

    class LoadedSchemaEventArgs : EventArgs
    {
        public LoadedSchemaEventArgs()
            : base()
        {
            Succeeded = true;
        }

        public LoadedSchemaEventArgs(Exception ex)
            : base()
        {
            Succeeded = false;
            Exception = ex;
        }

        public bool Succeeded { get; set; }
        public Exception Exception { get; set; }
    }

    class ExportingSchemaEventArgs : EventArgs
    {
        public ExportingSchemaEventArgs(string listName, Guid listID, int version)
            : base()
        {
            List = listName;
            Identifier = listID;
            Version = version;
        }

        public string List { get; set; }
        public Guid Identifier { get; set; }
        public int Version { get; set; }
    }

    class ExportedSchemaEventArgs : EventArgs
    {
        public ExportedSchemaEventArgs(int propertyCount)
            : base()
        {
            Succeeded = true;
            PropertyCount = propertyCount;
        }

        public ExportedSchemaEventArgs(Exception ex)
            : base()
        {
            Succeeded = false;
            Exception = ex;
        }

        public bool Succeeded { get; set; }
        public Exception Exception { get; set; }
        public int PropertyCount { get; set; }
    }

    /// <summary>
    /// Generates code for entity classes based on the list schema exported from SharePoint.
    /// </summary>
    class EntityGenerator
    {
        private List<string> enums = new List<string>();	
        private string language;
        private Dictionary<string, string> lookups = new Dictionary<string, string>();
        private static string LOOKUP = "#LOOKUP{0}#";
        private int lookupCount = 0;
        private HashSet<string> forbiddenTypeNames = new HashSet<string>();

        public event EventHandler<ConnectingEventArgs> Connecting;
        public event EventHandler<ConnectedEventArgs> Connected;
        public event EventHandler<LoadingSchemaEventArgs> LoadingSchema;
        public event EventHandler<LoadedSchemaEventArgs> LoadedSchema;
        public event EventHandler<ExportingSchemaEventArgs> ExportingSchema;
        public event EventHandler<ExportedSchemaEventArgs> ExportedSchema;

        public List<string> Enums
        {
            get { return enums; }
            set { enums = value; }
        }

        /// <summary>
        /// Generates an entity based on the given arguments.
        /// </summary>
        /// <param name="a">Arguments specifying the list to be exported.</param>
        /// <returns>Entity object containing the code and other information about the exported entity; null if the export wasn't successful.</returns>
        public Entity Generate(Args a)
        {
            //
            // Set correct language code fragments.
            //
            if ((language = a.Language) == "CS")
            {
                ENUM = ENUM_CS;
                CLASS = CLASS_CS;
                PROP = PROP_CS;
                PROP_READONLY = PROP_READONLY_CS;
                CHOICEHELPERPROP = CHOICEHELPERPROP_CS;
            }
            else
            {
                ENUM = ENUM_VB;
                CLASS = CLASS_VB;
                PROP = PROP_VB;
                PROP_READONLY = PROP_READONLY_VB;
                CHOICEHELPERPROP = CHOICEHELPERPROP_VB;
            }

            //
            // List definition XML; will be downloaded from server.
            //
            XmlNode lst;

            //
            // Create proxy object referring to the SharePoint lists.asmx service on the specified server.
            //
            Lists l = new Lists();
            l.Url = a.Url.TrimEnd('/') + "/_vti_bin/lists.asmx";

            //
            // Try to connect to server.
            //
            try
            {
                //
                // Send event about connection.
                //
                EventHandler<ConnectingEventArgs> connecting = Connecting;
                if (connecting != null)
                    connecting(this, new ConnectingEventArgs(l.Url));

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

                //
                // Send event about connection completion.
                //
                EventHandler<ConnectedEventArgs> connected = Connected;
                if (connected != null)
                    connected(this, new ConnectedEventArgs());
            }
            catch (Exception ex)
            {
                //
                // Send event about connection failure.
                //
                EventHandler<ConnectedEventArgs> connected = Connected;
                if (connected != null)
                    connected(this, new ConnectedEventArgs(ex));

                return null;
            }

            try
            {
                //
                // Load schema from server using lists.asmx web service and send event about schema loading.
                //
                EventHandler<LoadingSchemaEventArgs> loadingSchema = LoadingSchema;
                if (loadingSchema != null)
                    loadingSchema(this, new LoadingSchemaEventArgs(a.List));
                lst = l.GetList(a.List);

                //
                // Send event about schema loading completion.
                //
                EventHandler<LoadedSchemaEventArgs> loadedSchema = LoadedSchema;
                if (loadedSchema != null)
                    loadedSchema(this, new LoadedSchemaEventArgs());
            }
            catch (Exception ex)
            {
                //
                // Send event about schema loading failure.
                //
                EventHandler<LoadedSchemaEventArgs> loadedSchema = LoadedSchema;
                if (loadedSchema != null)
                    loadedSchema(this, new LoadedSchemaEventArgs(ex));

                return null;
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

            //
            // Send event about schema exporting.
            //
            EventHandler<ExportingSchemaEventArgs> exportingSchema = ExportingSchema;
            if (exportingSchema != null)
                exportingSchema(this, new ExportingSchemaEventArgs(listName, listID, version));

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
                // Primary key field should be imported as well.
                //
                XmlAttribute primaryKey = c.Attributes["PrimaryKey"];
                bool pk = false;
                if (primaryKey != null)
                    pk = bool.Parse(primaryKey.Value);

                //
                // Export only fields that aren't hidden or the primary key field.
                //
                if (!hidden || pk)
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
                        if (pk)
                            extra.AppendFormat(", PrimaryKey{0}true", language == "CS" ? " = " : ":=");
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
                            helper = GetFriendlyName((string)c.Attributes["DisplayName"].Value) + "Other";
                            extra.AppendFormat(", OtherChoice{1}\"{0}\"", helper, language == "CS" ? " = " : ":=");
                        }

                        //
                        // Lookup fields require a LookupField attribute property to be set.
                        //
                        if ((string)c.Attributes["Type"].Value == "Lookup")
                            extra.AppendFormat(", LookupField{1}\"{0}\"", (string)c.Attributes["ShowField"].Value, language == "CS" ? " = " : ":=");

                        //
                        // Generate a property for the current field and append it to the properties output string.
                        //
                        props.AppendFormat((readOnly ? PROP_READONLY : PROP), (description ?? displayName), bclType, GetFriendlyName(displayName), XmlConvert.DecodeName(name), type, id, extra.ToString());

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
            // Send event about schema exporting completion.
            //
            EventHandler<ExportedSchemaEventArgs> exportedSchema = ExportedSchema;
            if (exportedSchema != null)
                exportedSchema(this, new ExportedSchemaEventArgs(n));

            //
            // Build code with class definition containing the properties and the helper enums.
            //
            StringBuilder output = new StringBuilder();
            output.AppendFormat(CLASS, listName, props.ToString(), listName, listID.ToString(), version, (listDescription ?? listName), path);

            //
            // Return entity.
            //
            Entity entity = new Entity();
            entity.Name = listName;
            entity.Code = output.ToString();
            entity.Lookups = lookups;
            return entity;
        }

        /// <summary>
        /// Get a friendly name for a field, usable in C# or VB as a property name.
        /// </summary>
        /// <param name="name">Name to create a friendly name for.</param>
        /// <returns>Friendly name for given name.</returns>
        private string GetFriendlyName(string name)
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

            /* <ADD Version="0.1.3"> */

            //
            // Check for starting with digit case.
            //
            string res = sb.ToString();
            if (res.Length != 0 && char.IsDigit(res[0]))
                res = '_' + res;

            /* </ADD> */

            return res;
        }

        /// <summary>
        /// Keeps only letters and digits from a given string.
        /// </summary>
        /// <param name="s">String to be sanitized.</param>
        /// <returns>Sanitized string.</returns>
        private string SanitizeString(string s)
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
        private string GetType(XmlNode c, out bool additional)
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
                // Counter == int (used for primary key field)
                //
                case "Counter":
                    return (language == "CS" ? "int" : "Integer");
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
                        XmlAttribute fillInChoice = c.Attributes["FillInChoice"];
                        additional = fillInChoice != null && bool.Parse((string)fillInChoice.Value);

                        //
                        // Add enum to the enums collection.
                        //
                        string name = GetFriendlyName((string)c.Attributes["DisplayName"].Value);
                        if (forbiddenTypeNames.Contains(name))
                        {
                            int i = 1;
                            while (forbiddenTypeNames.Contains(name + i))
                                i++;
                            name += i;
                        }
                        forbiddenTypeNames.Add(name);
                            
                        Enums.Add(String.Format(ENUM, name, sb.ToString().TrimEnd(' ', ','), flags ? (language == "CS" ? "[Flags] " : "<Flags()> ") : ""));

                        //
                        // Return enum name, possibly nullable.
                        //
                        return language == "CS" ? (required ? name : name + "?") : String.Format((required ? "{0}" : "Nullable(Of {0})"), name);
                    }
                //
                // Lookup fields require the generation of another linked entity type.
                //
                case "Lookup":
                    {
                        string patch = String.Format(LOOKUP, ++lookupCount);
                        string lookupList = (string)c.Attributes["List"].Value;
                        lookups.Add(patch, lookupList);

                        return patch;
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
        static string PROP_READONLY;
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
@"
/// <summary>
/// {5}
/// </summary>
[List(""{2}"", Id = ""{3}"", Version = {4}, Path = ""{6}"")]
class {0} : SharePointListEntity
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
@"
''' <summary>
''' {5}
''' </summary>
<List(""{2}"", Id:=""{3}"", Version:={4}, Path:=""{6}"")> _
Class {0}
    Inherits SharePointListEntity
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
        /// public string FullName
        /// {
        ///     get { return (string)GetValue("First name"); }
        ///     set { SetValue("First name", value); }
        /// }
        /// ]]></example>
        static string PROP_CS =
@"
    /// <summary>
    /// {0}
    /// </summary>
    [Field(""{3}"", FieldType.{4}, Id = ""{5}""{6})]
    public {1} {2}
    {{
        get {{ return ({1})GetValue(""{2}""); }}
        set {{ SetValue(""{2}"", value); }}
    }}
";

        /// <summary>
        /// Skeleton for read-only entity properties with field mappings.
        /// </summary>
        /// <example><![CDATA[
        /// /// <summary>
        /// /// First name
        /// /// </summary>
        /// [Field("First name", FieldType.Text, ID = "f5164cc0-8a1b-423d-8669-e86bb65a3118")]
        /// public string FullName
        /// {
        ///     get { return (string)GetValue("First name"); }
        /// }
        /// ]]></example>
        static string PROP_READONLY_CS =
@"
    /// <summary>
    /// {0}
    /// </summary>
    [Field(""{3}"", FieldType.{4}, Id = ""{5}""{6})]
    public {1} {2}
    {{
        get {{ return ({1})GetValue(""{2}""); }}
    }}
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
        ///         Return CType(GetValue(""{3}""), {1})
        ///     End Get
        ///     Set(ByVal value As String)
        ///         SetValue(""{3}"", value)
        ///     End Set
        /// End Property
        /// ]]></example>
        static string PROP_VB =
@"
    ''' <summary>
    ''' {0}
    ''' </summary>
    <Field(""{3}"", FieldType.{4}, Id:=""{5}""{6})> _
    Public Property {2}() As {1}
        Get
            Return CType(GetValue(""{2}""), {1})
        End Get
        Set(ByVal value As {1})
            SetValue(""{2}"", value)
        End Set
    End Property
";

        /// <summary>
        /// Skeleton for read-only entity properties with field mappings.
        /// </summary>
        /// <example><![CDATA[
        /// ''' <summary>
        /// ''' First name
        /// ''' </summary>
        /// <Field("First name", FieldType.Text, ID:="f5164cc0-8a1b-423d-8669-e86bb65a3118")> _
        /// Public Property FullName() As String
        ///     Get
        ///         Return CType(GetValue(""{3}""), {1})
        ///     End Get
        /// End Property
        /// ]]></example>
        static string PROP_READONLY_VB =
@"
    ''' <summary>
    ''' {0}
    ''' </summary>
    <Field(""{3}"", FieldType.{4}, Id:=""{5}""{6})> _
    Public ReadOnly Property {2}() As {1}
        Get
            Return CType(GetValue(""{2}""), {1})
        End Get
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
    public string {1} 
    {{
        get {{ return (string)GetValue(""{1}""); }}
        set {{ SetValue(""{1}"", value); }}
    }}
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
            Return CType(GetValue(""{1}""), String)
        End Get
        Set(ByVal value As String)
            SetValue(""{1}"", value)
        End Set
    End Property
";
    }
}
