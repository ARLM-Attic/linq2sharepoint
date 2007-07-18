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

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    partial class Welcome : UserControl, IWizardStep
    {
        private Context context;

        public Welcome(Context context)
        {
            this.context = context;

            InitializeComponent();
        }

        public string Title
        {
            get { return "Welcome to the LINQ to SharePoint Entity Wizard"; }
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
        }
    }
}
