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
    public partial class Wizard : Form
    {
        private List<Control> steps;
        private Context context;
        private int step;
        private IWizardStep current;
        private bool noCancel = false;

        public Wizard()
        {
            context = new Context();
            steps = new List<Control>() {
                        new Welcome(context),
                        new Connect(context),
                        new Finish(context)
                    };

            InitializeComponent();

            step = 0;
            Step();
        }

        public Context Context
        {
            get
            {
                return context;
            }
        }

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
            // Event handler.
            //
            current.StateChanged +=
                delegate(object sender, EventArgs e)
                {
                    btnNext.Enabled = ((IWizardStep)sender).CanNext;
                };

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
                btnPrev.Enabled = false;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.Cancel;
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Wizard_Load(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Debug.Assert(step < steps.Count - 1);
            step++;
            Step();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Debug.Assert(step > 0);
            step--;
            Step();
        }

        private void Wizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !ConfirmClose();
        }

        private bool ConfirmClose()
        {
            if (noCancel)
                return true;
            else
                return MessageBox.Show("Are you sure you want to cancel the wizard?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes;
        }
    }
}
