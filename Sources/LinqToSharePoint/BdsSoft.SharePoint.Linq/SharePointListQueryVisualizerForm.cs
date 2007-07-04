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
    /// Windows Form for the SharePointListSource debugger visualizer.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
    public partial class SharePointListQueryVisualizerForm : Form
    {
        /// <summary>
        /// Creates a visualizer form for the specified entity and the specified piece of CAML.
        /// </summary>
        /// <param name="entity">Entity for which the query was written.</param>
        /// <param name="caml">CAML representing the query to be visualized.</param>
        public SharePointListQueryVisualizerForm(string entity, string caml)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(entity))
                this.txtEntity.Text = entity;
            
            if (!string.IsNullOrEmpty(caml))
                this.txtCaml.Text = caml;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
