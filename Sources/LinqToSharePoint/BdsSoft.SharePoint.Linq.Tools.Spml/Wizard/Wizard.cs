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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    /// <summary>
    /// Entity generator wizard.
    /// </summary>
    public partial class Wizard : Form
    {
        #region Private members

        /// <summary>
        /// Wizard steps.
        /// </summary>
        private List<Control> steps;

        /// <summary>
        /// Wizard context.
        /// </summary>
        private WizardContext context;

        /// <summary>
        /// Current step number.
        /// </summary>
        private int step;

        /// <summary>
        /// Current step.
        /// </summary>
        private IWizardStep current;

        /// <summary>
        /// Indicates whether a form close operation should be considered a wizard cancellation.
        /// </summary>
        private bool noCancel = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the entity generator wizard.
        /// </summary>
        public Wizard()
        {
            //
            // Create new wizard context.
            //
            context = new WizardContext();

            //
            // Initialize wizard steps.
            //
            steps = new List<Control>() {
                        new Welcome(context),
                        new Connect(context),
                        new ExportLists(context),
                        new Finish(context)
                    };

            //
            // Hook up event handlers for all of the steps.
            //
            SetupEvents();

            //
            // Form initialization.
            //
            InitializeComponent();

            //
            // Go to the first step.
            //
            step = 0;
            Step();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the wizard context.
        /// </summary>
        public WizardContext WizardContext
        {
            get
            {
                return context;
            }
        }

        #endregion

        #region Event handlers

        private void Wizard_Load(object sender, EventArgs e)
        {
            btnNext.Focus();
        }

        private void Wizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !ConfirmClose();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Debug.Assert(step > 0);
            step--;
            Step();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Debug.Assert(step < steps.Count - 1);
            step++;
            Step();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (progress.Visible)
                current.Cancel();
            else
                this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Hooks up event handlers for the wizard steps.
        /// </summary>
        private void SetupEvents()
        {
            foreach (IWizardStep step in steps)
            {
                step.StateChanged +=
                    delegate(object sender, EventArgs e)
                    {
                        bool next = ((IWizardStep)sender).CanNext;
                        btnNext.Enabled = next;
                        if (next)
                        {
                            this.AcceptButton.NotifyDefault(false);
                            this.AcceptButton = btnNext;
                            this.AcceptButton.NotifyDefault(true);
                            btnNext.Focus();
                        }
                    };
                step.Working +=
                    delegate(object sender, EventArgs e)
                    {
                        progress.Visible = true;
                        buttons.Enabled = false;
                    };
                step.WorkCompleted +=
                    delegate(object sender, EventArgs e)
                    {
                        progress.Visible = false;
                        buttons.Enabled = true;
                    };
            }
        }

        /// <summary>
        /// Moves the wizard to the step indicated in member variable step.
        /// </summary>
        private void Step()
        {
            //
            // Get rid of old step.
            //
            contents.Controls.Clear();

            //
            // Get next step.
            //
            Control c = steps[step];
            c.Dock = DockStyle.Fill;
            contents.Controls.Add(c);

            IWizardStep s = c as IWizardStep;
            Debug.Assert(s != null);
            current = s;

            lblTitle.Text = s.Title;

            //
            // Enable/disable buttons.
            //
            btnNext.Enabled = current.CanNext;
            if (step == 0)
            {
                btnPrev.Enabled = false;
                btnFinish.Enabled = false;
                btnCancel.Enabled = true;
            }
            else if (step == steps.Count - 1)
            {
                btnPrev.Enabled = true;
                btnFinish.Enabled = true;
                btnCancel.Enabled = false;

                this.AcceptButton = btnFinish;
                noCancel = true;
            }
            else
            {
                btnPrev.Enabled = true;
                btnFinish.Enabled = false;
                btnCancel.Enabled = true;
            }
        }

        /// <summary>
        /// Asks the user for close confirmation.
        /// </summary>
        /// <returns>true if close is permitted; otherwise, false.</returns>
        private bool ConfirmClose()
        {
            if (noCancel)
                return true;
            else
                return MessageBox.Show("Are you sure you want to cancel the wizard?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes;
        }

        #endregion
    }
}
