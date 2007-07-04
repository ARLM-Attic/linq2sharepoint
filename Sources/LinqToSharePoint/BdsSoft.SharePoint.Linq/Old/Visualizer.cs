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
    /// <summary>
    /// Windows Form for debugger visualizer.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
    public partial class Visualizer : Form
    {
        /// <summary>
        /// Creates a visualizer form for the specified entity and the specified piece of CAML.
        /// </summary>
        /// <param name="entity">Entity for which the query was written.</param>
        /// <param name="caml">CAML representing the query to be visualized.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "caml")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void btnExecute_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet. Please check back later.", "LINQ to SharePoint Query Visualizer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
