﻿/*
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
 * 0.2.0 - SPML introduction
 * 0.2.2 - Added language tracking to provide to the entity generator
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using VSLangProj80;
using System.CodeDom.Compiler;
using BdsSoft.SharePoint.Linq.Tools.EntityGenerator;
using System.CodeDom;
using System.IO;
using System.Xml;
using EnvDTE;
using System.Diagnostics;
using System.Xml.Schema;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    /// <summary>
    /// Code generator for SPML files. Translates an SPML definition into a set of entity classes in either C# or VB, using the Entity Generator.
    /// </summary>
    [ComVisible(true)]
    [Guid("8943CA47-BF10-4f25-9E5C-AE42A21338D9")]
    [CodeGeneratorRegistration(typeof(SpmlCodeGenerator), "C# LINQ to SharePoint Entity Class Generator", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true, GeneratorRegKeyName = "LINQtoSharePointGenerator")]
    [CodeGeneratorRegistration(typeof(SpmlCodeGenerator), "VB LINQ to SharePoint Entity Class Generator", vsContextGuids.vsContextGuidVBProject, GeneratesDesignTimeSource = true, GeneratorRegKeyName = "LINQtoSharePointGenerator")]
    [ProvideObject(typeof(SpmlCodeGenerator))]
    public class SpmlCodeGenerator : BaseCodeGeneratorWithSite
    {
        #region Methods

        /// <summary>
        /// Get the default extension for the generated code file.
        /// </summary>
        /// <returns>The language's default language extension, prefixed with ".designer".</returns>
        protected override string GetDefaultExtension()
        {
            return ".designer" + base.GetDefaultExtension();
        }

        /// <summary>
        /// Generates code for the SPML definition that's passed in.
        /// </summary>
        /// <param name="inputFileContent">SPML definition.</param>
        /// <returns>Code in the appropriate source language.</returns>
        protected override byte[] GenerateCode(string inputFileContent)
        {
            //
            // Ensure references to BdsSoft.SharePoint.Linq and Microsoft.SharePoint are present in the project.
            //
            this.GetVSProject().References.Add("BdsSoft.SharePoint.Linq");
            this.GetVSProject().References.Add("Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71E9BCE111E9429C");

            //
            // Get SPML definition.
            //
            XmlDocument spml = new XmlDocument();
            spml.LoadXml(inputFileContent);

            //
            // Set arguments.
            //
            EntityGeneratorArgs args = new EntityGeneratorArgs();
            args.Namespace = this.FileNameSpace;

            string lang = GetProject().CodeModel.Language;
            Debug.Assert(lang == CodeModelLanguageConstants.vsCMLanguageCSharp || lang == CodeModelLanguageConstants.vsCMLanguageVB);

            if (lang == CodeModelLanguageConstants.vsCMLanguageCSharp)
                args.Language = BdsSoft.SharePoint.Linq.Tools.EntityGenerator.Language.CSharp;
            else if (lang == CodeModelLanguageConstants.vsCMLanguageVB)
                args.Language = BdsSoft.SharePoint.Linq.Tools.EntityGenerator.Language.VB;
            else
                throw new NotSupportedException("Specified language not supported.");

            //
            // Generate the code.
            //
            return GenerateCode(new BdsSoft.SharePoint.Linq.Tools.EntityGenerator.EntityGenerator(args), spml);
        }

        #endregion

        #region Helper methods

        private byte[] GenerateCode(BdsSoft.SharePoint.Linq.Tools.EntityGenerator.EntityGenerator gen, XmlDocument spml)
        {
            //
            // Generate code and report warnings/errors if any.
            //
            CodeCompileUnit compileUnit;
            try
            {
                compileUnit = gen.GenerateCode(spml);
            }
            catch (EntityGeneratorException ex)
            {
                if (ex.Data.Contains("messages"))
                    foreach (var s in (List<ValidationEventArgs>)ex.Data["messages"])
                        this.GeneratorError(s.Severity == XmlSeverityType.Warning, 0, s.Message, (uint)s.Exception.LineNumber, (uint)s.Exception.LinePosition);

                return null;
            }

            //
            // Use CodeDOM to generate source code.
            //
            CodeDomProvider provider = GetCodeProvider();
            StringBuilder code = new StringBuilder();
            TextWriter tw = new StringWriter(code);
            provider.GenerateCodeFromCompileUnit(compileUnit, tw, null);
            tw.Flush();

            //
            // Do the right encoding to write to the target output file.
            //
            Encoding enc = Encoding.GetEncoding(tw.Encoding.WindowsCodePage);
            byte[] preamble = enc.GetPreamble();
            int preambleLength = preamble.Length;
            byte[] body = enc.GetBytes(code.ToString());
            Array.Resize<byte>(ref preamble, preambleLength + body.Length);
            Array.Copy(body, 0, preamble, preambleLength, body.Length);

            //
            // Return generated code.
            //
            return preamble;
        }

        #endregion
    }
}
