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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BdsSoft.SharePoint.Linq.Tools.EntityGenerator;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    partial class Finish : UserControl, IWizardStep
    {
        private WizardContext ctx;

        public Finish(WizardContext context)
        {
            this.ctx = context;

            InitializeComponent();
        }

        private void Finish_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (List list in ctx.ResultContext.Lists)
            {
                sb.AppendLine("Generate entity for list " + list.Name);
            }
            txtSummary.Text = sb.ToString();
            txtSummary.Select(0, 0);
        }

        public string Title
        {
            get { return "Entity mapping generation completed"; }
        }

        public bool CanNext
        {
            get { return false; }
        }

        public event EventHandler StateChanged;
        public event EventHandler Working;
        public event EventHandler WorkCompleted;

        public void Cancel()
        {
        }
    }
}
