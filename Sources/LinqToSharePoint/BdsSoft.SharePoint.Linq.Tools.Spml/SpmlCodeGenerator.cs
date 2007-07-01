/*
 * LINQ-to-SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using VSLangProj80;
using System.CodeDom.Compiler;
using EG = BdsSoft.SharePoint.Linq.Tools.EntityGenerator;
using System.CodeDom;
using System.IO;
using System.Xml;

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    [ComVisible(true)]
    [Guid("8943CA47-BF10-4f25-9E5C-AE42A21338D9")]
    [CodeGeneratorRegistration(typeof(SpmlCodeGenerator), "C# LINQ to SharePoint Entity Class Generator", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true, GeneratorRegKeyName = "LINQtoSharePointGenerator")]
    [CodeGeneratorRegistration(typeof(SpmlCodeGenerator), "VB LINQ to SharePoint Entity Class Generator", vsContextGuids.vsContextGuidVBProject, GeneratesDesignTimeSource = true, GeneratorRegKeyName = "LINQtoSharePointGenerator")]
    [ProvideObject(typeof(SpmlCodeGenerator))]
    public class SpmlCodeGenerator : BaseCodeGeneratorWithSite
    {
        protected override byte[] GenerateCode(string inputFileContent)
        {
            if (this.CodeGeneratorProgress != null)
                this.CodeGeneratorProgress.Progress(0, 100);

            //
            // Ensure reference to BdsSoft.SharePoint.Linq is present in the project.
            //
            this.GetVSProject().References.Add("BdsSoft.SharePoint.Linq");

            if (this.CodeGeneratorProgress != null)
                this.CodeGeneratorProgress.Progress(5, 100);

            //
            // Parse SPML.
            //
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(inputFileContent);
            XmlElement root = doc["SharePointDataContext"];
            string url = root.Attributes["Url"].InnerText;
            List<string> lists = new List<string>();
            foreach (XmlNode list in root["Lists"].ChildNodes)
                lists.Add(list.Attributes["Name"].InnerText);

            EG.EntityGeneratorArgs args = new EG.EntityGeneratorArgs();
            args.Namespace = this.FileNameSpace;

            args.Url = url;

            EG.EntityGenerator gen = new EG.EntityGenerator(args);
            gen.Connecting += delegate(object sender, EG.ConnectingEventArgs e)
                {
                    if (this.CodeGeneratorProgress != null)
                        this.CodeGeneratorProgress.Progress(10, 100);
                };
            gen.Connected += delegate(object sender, EG.ConnectedEventArgs e)
                {
                    if (this.CodeGeneratorProgress != null)
                        this.CodeGeneratorProgress.Progress(15, 100);
                };

            uint progress = 20;
            uint step = (100 - progress) / (4 * (uint)lists.Count);

            gen.ExportingSchema += delegate(object sender, EG.ExportingSchemaEventArgs e)
                {
                    if (this.CodeGeneratorProgress != null)
                        this.CodeGeneratorProgress.Progress(progress, 100);
                    progress += step;
                };
            gen.ExportedSchema += delegate(object sender, EG.ExportedSchemaEventArgs e)
                {
                    if (this.CodeGeneratorProgress != null)
                        this.CodeGeneratorProgress.Progress(progress, 100);
                    progress += step;
                };
            gen.LoadingSchema += delegate(object sender, EG.LoadingSchemaEventArgs e)
                {
                    if (this.CodeGeneratorProgress != null)
                        this.CodeGeneratorProgress.Progress(progress, 100);
                    progress += step;
                };
            gen.LoadedSchema += delegate(object sender, EG.LoadedSchemaEventArgs e)
                {
                    if (this.CodeGeneratorProgress != null)
                        this.CodeGeneratorProgress.Progress(progress, 100);
                    progress += step;
                };

            CodeCompileUnit compileUnit = gen.Generate(lists.ToArray());

            CodeDomProvider provider = GetCodeProvider();
            StringBuilder code = new StringBuilder();
            TextWriter tw = new StringWriter(code);
            provider.GenerateCodeFromCompileUnit(compileUnit, tw, null);

            if (this.CodeGeneratorProgress != null)
                this.CodeGeneratorProgress.Progress(100, 100);

            tw.Flush();

            Encoding enc = Encoding.GetEncoding(tw.Encoding.WindowsCodePage);

            byte[] preamble = enc.GetPreamble();
            int preambleLength = preamble.Length;

            //Convert the writer contents to a byte array
            byte[] body = enc.GetBytes(code.ToString());

            //Prepend the preamble to body (store result in resized preamble array)
            Array.Resize<byte>(ref preamble, preambleLength + body.Length);
            Array.Copy(body, 0, preamble, preambleLength, body.Length);

            //Return the combined byte array
            return preamble;
        }
    }
}
