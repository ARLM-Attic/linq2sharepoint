using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    public partial class Wizard : Form
    {
        private List<Control> steps;
        private Context context;
        private int step;

        public Wizard()
        {
            context = new Context();
            steps = new List<Control>() {
                        //new Welcome(context),
                        //new Finish(context)
                    };

            InitializeComponent();

            step = 0;
            Step();
        }

        private void Step()
        {
            if (step == 0)
            {
                btnPrev.Enabled = false;
                btnNext.Enabled = true;
                btnFinish.Enabled = false;
                btnCancel.Enabled = true;
            }
            else if (step == steps.Count - 1)
            {
                btnPrev.Enabled = false;
                btnNext.Enabled = false;
                btnFinish.Enabled = true;
                btnCancel.Enabled = false;

                this.AcceptButton = btnFinish;
            }
            else
            {
                btnPrev.Enabled = true;
                btnNext.Enabled = true;
                btnFinish.Enabled = false;
                btnCancel.Enabled = true;
            }

            Control c = steps[step];
            c.Dock = DockStyle.Fill;

            IWizardStep s = c as IWizardStep;
            if (s != null)
                lblTitle.Text = s.Title;

            contents.Controls.Clear();
            contents.Controls.Add(c);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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
    }
}
