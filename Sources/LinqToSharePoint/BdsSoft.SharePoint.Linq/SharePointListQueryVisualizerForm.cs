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
using System.IO;

namespace BdsSoft.SharePoint.Linq
{
    /// <summary>
    /// Windows Form for the SharePointListSource debugger visualizer.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer")]
    public partial class SharePointListQueryVisualizerForm : Form
    {
        private ParseErrorCollection _errors;

        /// <summary>
        /// Creates a visualizer form for the specified entity and the specified piece of CAML.
        /// </summary>
        /// <param name="entity">Entity for which the query was written.</param>
        /// <param name="caml">CAML representing the query to be visualized.</param>
        /// <param name="linq">Textual representation of the LINQ query expression tree.</param>
        /// <param name="errors">Set of parse errors.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "caml")]
        public SharePointListQueryVisualizerForm(string entity, string caml, string linq, ParseErrorCollection errors)
        {
            _errors = errors;

            InitializeComponent();

            if (!string.IsNullOrEmpty(entity))
                this.txtEntity.Text = entity;

            if (!string.IsNullOrEmpty(caml))
                this.txtCaml.Text = caml;

            if (!string.IsNullOrEmpty(linq))
                this.txtLinq.Text = linq;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SharePointListQueryVisualizerForm_Load(object sender, EventArgs e)
        {
            if (_errors != null && _errors.Count > 0)
            {
                foreach (ParseError error in _errors)
                {
                    txtLinq.Select(error.StartIndex, error.EndIndex - error.StartIndex + 1);
                    txtLinq.SelectionColor = Color.Red;

                    string tag = "<ParseError ID=\"" + error.ErrorId + "\" />";
                    int iCaml = txtCaml.Text.IndexOf(tag);
                    if (iCaml >= 0)
                    {
                        txtCaml.Select(iCaml, tag.Length);
                        txtCaml.SelectionColor = Color.Red;
                    }
                }

                txtLinq.Select(0, 0);
                txtCaml.Select(0, 0);
                txtLinq.MouseMove += new MouseEventHandler(txtLinq_MouseMove);
                txtLinq.MouseLeave += new EventHandler(txtLinq_MouseLeave);
            }
        }

        void txtLinq_MouseLeave(object sender, EventArgs e)
        {
            ClearSelections();
        }

        private ToolTip tip = new ToolTip();

        private int? currentError = null;

        private void txtLinq_MouseMove(object sender, MouseEventArgs e)
        {
            int i = txtLinq.GetCharIndexFromPosition(e.Location);

            foreach (ParseError error in _errors)
            {
                if (error.StartIndex <= i && error.EndIndex >= i)
                {
                    tip.SetToolTip(txtLinq, error.Message);

                    if (currentError == null || currentError != error.ErrorId)
                    {
                        ClearSelections();
                        
                        txtLinq.Select(error.StartIndex, error.EndIndex - error.StartIndex + 1);
                        txtLinq.SelectionFont = new Font(txtLinq.SelectionFont, FontStyle.Underline);

                        txtLinq.Select(0, 0);
                        txtCaml.Select(0, 0);

                        txtLinq.Cursor = Cursors.Hand;

                        string tag = "<ParseError ID=\"" + error.ErrorId + "\" />";
                        int iCaml = txtCaml.Text.IndexOf(tag);
                        if (iCaml >= 0)
                        {
                            txtCaml.Select(iCaml, tag.Length);
                            txtCaml.SelectionFont = new Font(txtCaml.SelectionFont, FontStyle.Underline);
                        }

                        currentError = error.ErrorId;
                    }

                    return;
                }
            }

            currentError = null;
            tip.RemoveAll();

            txtLinq.Cursor = Cursors.Default;

            ClearSelections();
        }

        private void ClearSelections()
        {
            txtLinq.Select(0, txtLinq.Text.Length);
            txtLinq.SelectionFont = new Font(txtLinq.Font, FontStyle.Regular);
            txtCaml.Select(0, txtCaml.Text.Length);
            txtCaml.SelectionFont = new Font(txtCaml.Font, FontStyle.Regular);

            txtLinq.Select(0, 0);
            txtCaml.Select(0, 0);
        }

        private void txtLinq_MouseClick(object sender, MouseEventArgs e)
        {
            int i = txtLinq.GetCharIndexFromPosition(e.Location);

            if (txtLinq.Cursor == Cursors.Hand && currentError != null)
            {
                // TODO: Show additional info about the error.
                MessageBox.Show("Not implemented yet.");
            }
        }
    }
}
