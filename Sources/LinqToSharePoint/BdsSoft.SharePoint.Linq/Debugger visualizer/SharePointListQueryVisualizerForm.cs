﻿/*
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
        #region Private members

        /// <summary>
        /// Name of the entity the query is using.
        /// </summary>
        private string _entity;

        /// <summary>
        /// Generated CAML fragment for the query.
        /// </summary>
        private string _caml;

        /// <summary>
        /// Textual representation of the LINQ expression tree for the query.
        /// </summary>
        private string _linq;

        /// <summary>
        /// Errors generated by the query parser.
        /// </summary>
        private ParseErrorCollection _errors;

        /// <summary>
        /// Positions of error indicators in CAML fragment.
        /// </summary>
        private Dictionary<int, Position> _camlPositions;

        /// <summary>
        /// Current error selected by hovering over the textboxes. If null, no error is selected.
        /// </summary>
        private ParseError currentError = null;

        /// <summary>
        /// Tooltip to provide error information on the LINQ textbox.
        /// </summary>
        private ToolTip linqTip = new ToolTip();

        /// <summary>
        /// Tooltip to provide error information on the CAML textbox.
        /// </summary>
        private ToolTip camlTip = new ToolTip();

        #endregion

        #region Constructors

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
            _entity = entity;
            _caml = caml;
            _linq = linq;
            _errors = errors;

            InitializeComponent();
            linqTip.InitialDelay = 0;
            camlTip.InitialDelay = 0;

            Init();
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Helper method to initialize the textboxes.
        /// </summary>
        private void Init()
        {
            //
            // Show the entity.
            //
            if (!string.IsNullOrEmpty(_entity))
                this.txtEntity.Text = _entity;

            //
            // Show the generated CAML.
            //
            if (!string.IsNullOrEmpty(_caml))
                this.txtCaml.Text = _caml;

            //
            // Show the LINQ query expression.
            //
            if (!string.IsNullOrEmpty(_linq))
                this.txtLinq.Text = _linq;
        }

        /// <summary>
        /// Helper method to mark an error in the UI.
        /// </summary>
        /// <param name="id">ID of the error to mark. Used for marking in the CAML textbox.</param>
        /// <param name="error">Error object of the error to mark. Used for marking in the LINQ expression textbox.</param>
        /// <param name="selected">Indicates whether the error is currently selected or not.</param>
        private void MarkError(int id, ParseError error, bool selected)
        {
            //
            // Select the expression part indicated by the error and mark it.
            //
            txtLinq.Select(error.StartIndex, error.EndIndex - error.StartIndex + 1);
            txtLinq.SelectionColor = Color.Red;
            if (selected)
                txtLinq.SelectionFont = new Font(txtLinq.SelectionFont, FontStyle.Underline);

            //
            // Locate the ParseError placeholder in the CAML query and mark it.
            //
            string tag = "<ParseError ID=\"" + id + "\" />";
            int iCaml = txtCaml.Text.IndexOf(tag);
            if (iCaml >= 0)
            {
                txtCaml.Select(iCaml, tag.Length);
                txtCaml.SelectionColor = Color.Red;
                if (selected)
                    txtCaml.SelectionFont = new Font(txtCaml.SelectionFont, FontStyle.Underline);
                else
                    _camlPositions.Add(id, new Position() { Start = iCaml, End = iCaml + tag.Length - 1 });
            }
        }

        /// <summary>
        /// Helper method to clear selections in the LINQ and CAML textboxes.
        /// </summary>
        private void ClearSelections()
        {
            //
            // Select all and undo marking.
            //
            txtLinq.Select(0, txtLinq.Text.Length);
            txtLinq.SelectionFont = new Font(txtLinq.Font, FontStyle.Regular);
            txtCaml.Select(0, txtCaml.Text.Length);
            txtCaml.SelectionFont = new Font(txtCaml.Font, FontStyle.Regular);

            //
            // Unselect for pretty display.
            //
            txtLinq.Select(0, 0);
            txtCaml.Select(0, 0);

            //
            // Hide tooltip.
            //
            linqTip.RemoveAll();
            camlTip.RemoveAll();

            //
            // Set default cursor.
            //
            txtLinq.Cursor = Cursors.Default;
            txtCaml.Cursor = Cursors.Default;

            //
            // Set to no current error.
            //
            currentError = null;
        }

        #endregion

        #region Event handlers

        private void SharePointListQueryVisualizerForm_Load(object sender, EventArgs e)
        {
            //
            // Format the errors in the LINQ fragment
            //
            if (!string.IsNullOrEmpty(_linq) && _errors != null && _errors.Count > 0)
            {
                //
                // Keep track of the CAML error positions.
                //
                _camlPositions = new Dictionary<int, Position>();

                //
                // Mark all errors.
                //
                foreach (var error in _errors)
                    MarkError(error.Key, error.Value, false);

                //
                // Unselect for pretty display.
                //
                txtLinq.Select(0, 0);
                txtCaml.Select(0, 0);

                //
                // Hook up event handlers for the LINQ expression textbox.
                //
                txtLinq.MouseMove += new MouseEventHandler(txtLinq_MouseMove);
                txtLinq.MouseLeave += new EventHandler(txtLinq_MouseLeave);
                txtLinq.MouseClick += new MouseEventHandler(txtLinq_MouseClick);

                //
                // Hook up event handlers for the CAML textbox.
                //
                txtCaml.MouseMove += new MouseEventHandler(txtCaml_MouseMove);
                txtCaml.MouseLeave += new EventHandler(txtCaml_MouseLeave);
                txtCaml.MouseClick += new MouseEventHandler(txtCaml_MouseClick);

                //
                // Disable the execute button for invalid queries.
                //
                btnExecute.Enabled = false;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void txtLinq_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // Get the position inside the textbox based on the mouse cursor position.
            //
            int i = txtLinq.GetCharIndexFromPosition(e.Location);

            //
            // Find the error (if any) that corresponds to the current position.
            // We expect the number of errors to be low, so we just iterate over the collection.
            //
            foreach (var error in _errors)
            {
                //
                // Matching position?
                //
                if (error.Value.StartIndex <= i && error.Value.EndIndex >= i)
                {
                    //
                    // Compare to selected error (if any).
                    //
                    if (currentError == null || currentError != error.Value)
                    {
                        //
                        // Clear previous error display.
                        //
                        ClearSelections();

                        //
                        // Mark error.
                        //
                        MarkError(error.Key, error.Value, true);

                        //
                        // Unselect for pretty display.
                        //
                        txtLinq.Select(0, 0);
                        txtCaml.Select(0, 0);

                        //
                        // Set cursor on LINQ textbox to indicate the possibility to click it (act as a link).
                        //
                        txtLinq.Cursor = Cursors.Hand;

                        //
                        // Set the current error.
                        //
                        currentError = error.Value;
                    }

                    //
                    // Set the tooltip.
                    //
                    linqTip.SetToolTip(txtLinq, error.Value.ToString());

                    //
                    // Found and done.
                    //
                    return;
                }
            }

            //
            // No error found. Clean the error display if a previous selected error is still displayed.
            //
            if (currentError != null)
                ClearSelections();
        }

        private void txtLinq_MouseLeave(object sender, EventArgs e)
        {
            ClearSelections();
        }

        private void txtLinq_MouseClick(object sender, MouseEventArgs e)
        {
            //
            // Get the position inside the textbox based on the mouse cursor position.
            //
            int i = txtLinq.GetCharIndexFromPosition(e.Location);

            //
            // Error selected?
            //
            if (txtLinq.Cursor == Cursors.Hand && currentError != null)
            {
                // TODO: Show additional info about the error.
                MessageBox.Show(currentError.ErrorCode + ": " + currentError.Message);
            }
        }

        private void txtCaml_MouseMove(object sender, MouseEventArgs e)
        {
            //
            // Get the position inside the textbox based on the mouse cursor position.
            //
            int i = txtCaml.GetCharIndexFromPosition(e.Location);

            //
            // Find the error (if any) that corresponds to the current position.
            // We expect the number of errors to be low, so we just iterate over the collection.
            //
            foreach (var pos in _camlPositions)
            {
                ParseError error = _errors[pos.Key];

                //
                // Matching position?
                //
                if (pos.Value.Start <= i && i <= pos.Value.End)
                {
                    //
                    // Compare to selected error (if any).
                    //
                    if (currentError == null || currentError != error)
                    {
                        //
                        // Clear previous error display.
                        //
                        ClearSelections();

                        //
                        // Mark error.
                        //
                        MarkError(pos.Key, error, true);

                        //
                        // Unselect for pretty display.
                        //
                        txtLinq.Select(0, 0);
                        txtCaml.Select(0, 0);

                        //
                        // Set cursor on CAML textbox to indicate the possibility to click it (act as a link).
                        //
                        txtCaml.Cursor = Cursors.Hand;

                        //
                        // Set the current error.
                        //
                        currentError = error;
                    }

                    //
                    // Set the tooltip.
                    //
                    camlTip.SetToolTip(txtCaml, error.ToString());

                    //
                    // Found and done.
                    //
                    return;
                }
            }

            //
            // No error found. Clean the error display if a previous selected error is still displayed.
            //
            if (currentError != null)
                ClearSelections();
        }

        private void txtCaml_MouseLeave(object sender, EventArgs e)
        {
            ClearSelections();
        }

        private void txtCaml_MouseClick(object sender, MouseEventArgs e)
        {
            //
            // Get the position inside the textbox based on the mouse cursor position.
            //
            int i = txtCaml.GetCharIndexFromPosition(e.Location);

            //
            // Error selected?
            //
            if (txtCaml.Cursor == Cursors.Hand && currentError != null)
            {
                // TODO: Show additional info about the error.
                MessageBox.Show(currentError.ErrorCode + ": " + currentError.Message);
            }
        }
    }

    internal struct Position
    {
        public int Start;
        public int End;
    }
}
