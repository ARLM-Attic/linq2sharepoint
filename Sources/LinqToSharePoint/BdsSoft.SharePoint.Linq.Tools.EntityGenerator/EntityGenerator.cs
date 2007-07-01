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
 * 0.2.1 - Use of CodeDom for code generation; move from SpMetal code to EntityGenerator
 */

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;
using System.CodeDom.Compiler;
using System.IO;

namespace BdsSoft.SharePoint.Linq.Tools.EntityGenerator
{
    /// <summary>
    /// Generates code for entity classes based on the list schema exported from SharePoint.
    /// </summary>
    public class EntityGenerator
    {
        #region Events

        /// <summary>
        /// Connecting to the SharePoint site.
        /// </summary>
        public event EventHandler<ConnectingEventArgs> Connecting;

        /// <summary>
        /// Connected to the SharePoint site.
        /// </summary>
        public event EventHandler<ConnectedEventArgs> Connected;

        /// <summary>
        /// Loading schema from the SharePoint site.
        /// </summary>
        public event EventHandler<LoadingSchemaEventArgs> LoadingSchema;

        /// <summary>
        /// Schema loaded from the SharePoint site.
        /// </summary>
        public event EventHandler<LoadedSchemaEventArgs> LoadedSchema;

        /// <summary>
        /// Exporting schema from the SharePoint site.
        /// </summary>
        public event EventHandler<ExportingSchemaEventArgs> ExportingSchema;

        /// <summary>
        /// Schema exported from the SharePoint site.
        /// </summary>
        public event EventHandler<ExportedSchemaEventArgs> ExportedSchema;

        #endregion

        private List<CodeTypeDeclaration> types = new List<CodeTypeDeclaration>();
        private HashSet<string> typeNames = new HashSet<string>();

        private EntityGeneratorArgs args;

        public EntityGenerator(EntityGeneratorArgs args)
        {
            this.args = args;
        }

        private string GetTypeName(string name)
        {
            name = Helpers.GetFriendlyName(name.Trim());

            int j = 0;
            string s = name;
            while (typeNames.Contains(s))
                s = name + j++;
            typeNames.Add(s);
            return s;
        }

        private Dictionary<string, CodeTypeDeclaration> entities = new Dictionary<string, CodeTypeDeclaration>();

        /// <summary>
        /// Generates an entity based on the given arguments.
        /// </summary>
        /// <returns>Entity object containing the code and other information about the exported entity; null if the export wasn't successful.</returns>
        public CodeCompileUnit Generate(params string[] listName)
        {
            //
            // Create code unit in specified namespace.
            //
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace ns = new CodeNamespace(args.Namespace);
            compileUnit.Namespaces.Add(ns);

            //
            // Add required namespace imports.
            //
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            ns.Imports.Add(new CodeNamespaceImport("BdsSoft.SharePoint.Linq"));

            //
            // Generate entity.
            //
            foreach (string list in listName)
                GenerateEntityForList(list);

            //
            // Add generated types to namespace.
            //
            foreach (CodeTypeDeclaration type in types)
                ns.Types.Add(type);

            //
            // Return compile unit.
            //
            return compileUnit;
        }

        private CodeTypeDeclaration GenerateEntityForList(string listName)
        {
            //
            // Get general information of the list.
            //
            XmlNode lst = GetListDefinition(listName);
            List list = List.FromCaml(lst);

            //
            // Check for duplicates.
            //
            if (entities.ContainsKey(list.Id.ToString()))
                return entities[list.Id.ToString()];

            //
            // Send event about schema exporting.
            //
            EventHandler<ExportingSchemaEventArgs> exportingSchema = ExportingSchema;
            if (exportingSchema != null)
                exportingSchema(this, new ExportingSchemaEventArgs(list.Name, list.Id, list.Version));

            //
            // CodeDOM entity type for list definition.
            //
            CodeTypeDeclaration listType = new CodeTypeDeclaration(GetTypeName(list.Name));
            listType.Attributes = MemberAttributes.Public;
            listType.BaseTypes.Add(new CodeTypeReference(typeof(SharePointListEntity)));
            listType.IsClass = true;

            //
            // Keep mapping.
            //
            entities.Add(list.Id.ToString(), listType);

            //
            // Custom attribute for list entity type.
            //
            CodeAttributeDeclaration listAttribute = new CodeAttributeDeclaration(new CodeTypeReference(typeof(ListAttribute)));
            listAttribute.Arguments.Add(new CodeAttributeArgument(new CodePrimitiveExpression(list.Name)));
            listAttribute.Arguments.Add(new CodeAttributeArgument("Id", new CodePrimitiveExpression(list.Id.ToString())));
            listAttribute.Arguments.Add(new CodeAttributeArgument("Version", new CodePrimitiveExpression(list.Version)));
            listAttribute.Arguments.Add(new CodeAttributeArgument("Path", new CodePrimitiveExpression(list.Path)));
            listType.CustomAttributes.Add(listAttribute);

            //
            // List entity type documentation comments.
            //
            listType.Comments.Add(new CodeCommentStatement("<summary>", true));
            listType.Comments.Add(new CodeCommentStatement(list.Description ?? list.Name, true));
            listType.Comments.Add(new CodeCommentStatement("</summary>", true));

            //
            // Generate field definitions.
            //
            int n = 0;
            foreach (Field field in list.Fields)
            {
                //
                // Export only fields that aren't hidden or the primary key field.
                //
                if (!field.IsHidden || field.IsPrimaryKey)
                {
                    //
                    // Is the underlying type recognized and supported by the mapper?
                    //
                    if (field.FieldType != FieldType.None)
                    {
                        //
                        // Build FieldAttribute attribute.
                        //
                        List<CodeAttributeArgument> fieldAttributeArgs = new List<CodeAttributeArgument>();
                        fieldAttributeArgs.Add(new CodeAttributeArgument(new CodePrimitiveExpression(XmlConvert.DecodeName(field.Name))));
                        fieldAttributeArgs.Add(
                            new CodeAttributeArgument(
                                new CodeFieldReferenceExpression(
                                    new CodeTypeReferenceExpression(typeof(FieldType)),
                                    Enum.GetName(typeof(FieldType), field.FieldType)
                                )
                            )
                        );
                        fieldAttributeArgs.Add(new CodeAttributeArgument("Id", new CodePrimitiveExpression(field.Id.ToString())));

                        //
                        // Read-only and calculated field require additional mapping attribute parameters.
                        //
                        if (field.IsPrimaryKey)
                            fieldAttributeArgs.Add(new CodeAttributeArgument("PrimaryKey", new CodePrimitiveExpression(true)));
                        if (field.IsReadOnly)
                            fieldAttributeArgs.Add(new CodeAttributeArgument("ReadOnly", new CodePrimitiveExpression(true)));
                        if (field.IsCalculated)
                            fieldAttributeArgs.Add(new CodeAttributeArgument("Calculated", new CodePrimitiveExpression(true)));

                        //
                        // Create helper field and refer to it in case a multi-choice fields with fill-in choice was detected.
                        // The helper field has the same name as the .NET type (which will be an enum) suffixed with "Other".
                        //
                        string helper = null;
                        if (field.FillInChoiceEnabled)
                        {
                            helper = Helpers.GetFriendlyName(field.DisplayName + "Other");
                            fieldAttributeArgs.Add(new CodeAttributeArgument("OtherChoice", new CodePrimitiveExpression(helper)));
                        }

                        //
                        // Runtime type for entity property. Will be supplied in type-specific switching logic below.
                        //
                        CodeTypeReference bclTypeRef;

                        //
                        // Type-specific generation actions; generate other entities or choice enums if required.
                        //
                        switch (field.FieldType)
                        {
                            case FieldType.Choice:
                            case FieldType.MultiChoice:
                                bool flags = field.FieldType == FieldType.MultiChoice;
                                if (field.IsRequired)
                                    bclTypeRef = new CodeTypeReference(GenerateChoiceEnum(field, flags));
                                else
                                {
                                    bclTypeRef = new CodeTypeReference(typeof(Nullable<>));
                                    bclTypeRef.TypeArguments.Add(new CodeTypeReference(GenerateChoiceEnum(field, flags)));
                                }
                                break;
                            case FieldType.Lookup:
                            case FieldType.LookupMulti:
                                {
                                    if (entities.ContainsKey(field.LookupList))
                                        bclTypeRef = new CodeTypeReference(entities[field.LookupList].Name);
                                    else
                                        bclTypeRef = new CodeTypeReference(GenerateEntityForList(field.LookupList).Name);
                                    fieldAttributeArgs.Add(new CodeAttributeArgument("LookupField", new CodePrimitiveExpression(field.LookupField)));

                                    //
                                    // LookupMulti fields are mapped on IList<T> properties.
                                    //
                                    if (field.FieldType == FieldType.LookupMulti)
                                    {
                                        CodeTypeReference t = bclTypeRef;
                                        bclTypeRef = new CodeTypeReference(typeof(IList<>));
                                        bclTypeRef.TypeArguments.Add(t);
                                    }
                                }
                                break;
                            default:
                                bclTypeRef = new CodeTypeReference(field.RuntimeType);
                                break;
                        }

                        //
                        // LookupMulti fields shouldn't be settable. The underlying IList<T> type will allow changes to the collection though.
                        //
                        bool readOnly = field.FieldType != FieldType.LookupMulti ? field.IsReadOnly : true;

                        //
                        // Create field property definition.
                        //
                        string fieldName = Helpers.GetFriendlyName(field.DisplayName);
                        CodeMemberProperty fieldProperty = GetFieldMemberProperty(field.DisplayName, field.Description, readOnly, bclTypeRef, fieldName, fieldAttributeArgs);
                        listType.Members.Add(fieldProperty);

                        //
                        // Generate additional helper property if needed.
                        //
                        if (field.FillInChoiceEnabled)
                        {
                            //
                            // Fill-in choice field is of type Text. Create FieldAttribute accordingly.
                            //
                            List<CodeAttributeArgument> helperFieldAttributeArgs = new List<CodeAttributeArgument>();
                            helperFieldAttributeArgs.Add(new CodeAttributeArgument(new CodePrimitiveExpression(XmlConvert.DecodeName(field.Name))));
                            helperFieldAttributeArgs.Add(
                                new CodeAttributeArgument(
                                    new CodeFieldReferenceExpression(
                                        new CodeTypeReferenceExpression(typeof(FieldType)),
                                        "Text"
                                    )
                                )
                            );

                            //
                            // Use same field Id as the helper's parent.
                            //
                            helperFieldAttributeArgs.Add(new CodeAttributeArgument("Id", new CodePrimitiveExpression(field.Id.ToString())));

                            //
                            // Create field definition for helper.
                            //
                            CodeMemberProperty helperField = GetFieldMemberProperty(field.DisplayName, field.DisplayName +  " 'Fill-in' value", false, new CodeTypeReference(typeof(string)), helper, helperFieldAttributeArgs);
                            listType.Members.Add(helperField);
                        }

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
            // Keep entity type definition.
            //
            types.Add(listType);

            //
            // Return type definition.
            //
            return listType;
        }

        private string GenerateChoiceEnum(Field field, bool flags)
        {
            //
            // Generate name for enum.
            //
            string name = GetTypeName(field.Name);

            //
            // Multi-choice values are mapped onto flag enums. A variable is kept for the flag value which should be a power of two.
            //
            int flagValue = 1;

            //
            // Create enum definition.
            //
            CodeTypeDeclaration enumType = new CodeTypeDeclaration(name);
            enumType.Attributes = MemberAttributes.Public;
            enumType.IsEnum = true;
            if (flags)
                enumType.CustomAttributes.Add(new CodeAttributeDeclaration("Flags"));

            //
            // Populate the enum with the choices available in the list field definition.
            //
            HashSet<string> choices = new HashSet<string>();
            foreach (string c in field.Choices)
            {
                //
                // Get friendly name for choice value.
                //
                string choice = Helpers.GetFriendlyName(c);

                //
                // Detect duplicate values; shouldn't occur in most cases.
                //
                int j = 0;
                while (choices.Contains(choice))
                    choice += (++j).ToString();
                choices.Add(choice);

                //
                // Create field definition and set flag value in case a flags enum is generated.
                //
                CodeMemberField choiceField = new CodeMemberField(typeof(uint), choice);
                if (flags)
                    choiceField.InitExpression = new CodePrimitiveExpression(flagValue);

                //
                // Add a enum field mapping in case the field name doesn't match the underlying SharePoint choice value textual represention.
                //
                if (choice != c)
                {
                    choiceField.CustomAttributes.Add(
                        new CodeAttributeDeclaration(
                            new CodeTypeReference(typeof(ChoiceAttribute)),
                            new CodeAttributeArgument(new CodePrimitiveExpression(c))
                        )
                    );
                }

                //
                // Add the created field to the enum.
                //
                enumType.Members.Add(choiceField);

                //
                // Update the flags value by multiplying it by two.
                //
                flagValue *= 2;
            }

            //
            // Keep enum definition.
            //
            types.Add(enumType);

            //
            // Return enum type name.
            //
            return name;
        }

        private CodeMemberProperty GetFieldMemberProperty(string displayName, string description, bool readOnly, CodeTypeReference bclTypeRef, string fieldName, List<CodeAttributeArgument> fieldAttributeArgs)
        {
            // ^-^ v0.2.1 CodeDOM ^-^
            CodeMemberProperty field = new CodeMemberProperty();
            field.Name = fieldName;
            field.Type = bclTypeRef;
            field.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            field.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeCastExpression(
                        bclTypeRef,
                        new CodeMethodInvokeExpression(
                            new CodeBaseReferenceExpression(),
                            "GetValue",
                            new CodePrimitiveExpression(fieldName)
                        )
                    )
                )
            );
            if (readOnly)
                field.HasSet = false;
            else
            {
                field.SetStatements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeBaseReferenceExpression(),
                        "SetValue",
                        new CodePrimitiveExpression(fieldName),
                        new CodePropertySetValueReferenceExpression()
                    )
                );
            }
            field.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(FieldAttribute)),
                    fieldAttributeArgs.ToArray()
                )
            );
            field.Comments.Add(new CodeCommentStatement("<summary>", true));
            field.Comments.Add(new CodeCommentStatement(description ?? displayName, true));
            field.Comments.Add(new CodeCommentStatement("</summary>", true));
            return field;
        }

        private XmlNode GetListDefinition(string list)
        {
            //
            // List definition XML; will be downloaded from server.
            //
            XmlNode lst;

            //
            // Create proxy object referring to the SharePoint lists.asmx service on the specified server.
            //
            Lists l = new Lists();
            l.Url = args.Url.TrimEnd('/') + "/_vti_bin/lists.asmx";

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
                if (args.User == null)
                    l.Credentials = CredentialCache.DefaultNetworkCredentials;
                //
                // Use specified credentials.
                //
                else
                {
                    if (args.Domain == null)
                        l.Credentials = new NetworkCredential(args.User, args.Password);
                    else
                        l.Credentials = new NetworkCredential(args.User, args.Password, args.Domain);
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
                    loadingSchema(this, new LoadingSchemaEventArgs(list));
                lst = l.GetList(list);

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
            
            return lst;
        }
    }
}
