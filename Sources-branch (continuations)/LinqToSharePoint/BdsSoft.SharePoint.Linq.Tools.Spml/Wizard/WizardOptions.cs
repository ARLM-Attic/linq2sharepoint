/*
 * LINQ to SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BdsSoft.SharePoint.Linq.Tools.EntityGenerator;

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    public partial class WizardOptions : Form
    {
        public WizardOptions()
        {
            InitializeComponent();
        }

        private void WizardOptions_Load(object sender, EventArgs e)
        {
            chkPluralize.Checked = List.AutoPluralize;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List.AutoPluralize = chkPluralize.Checked;
        }
    }
}
