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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BdsSoft.SharePoint.Linq
{
    public partial class Visualizer : Form
    {
        public Visualizer(string entity, string caml)
        {
            InitializeComponent();

            this.txtEntity.Text = entity;
            this.txtCaml.Text = caml;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet. Please check back later.", "LINQ to SharePoint Query Visualizer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
