using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    partial class Connect : UserControl, IWizardStep
    {
        private Context context;
        private bool _next;

        public Connect(Context context)
        {
            this.context = context;
            _next = false;

            InitializeComponent();
        }

        private void Connect_Load(object sender, EventArgs e)
        {

        }

        public string Title
        {
            get { return "Connect to the SharePoint site"; }
        }

        public bool CanNext
        {
            get { return _next; }
        }

        public event EventHandler StateChanged;

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren(ValidationConstraints.Enabled | ValidationConstraints.Selectable))
            {
                context.WssUrl = txtUrl.Text;
                if (radCustom.Checked)
                {
                    context.WssUser = txtUser.Text;
                    context.WssPassword = txtPassword.Text;
                    context.WssDomain = txtDomain.Text;
                }

                _next = true;

                if (StateChanged != null)
                    StateChanged(this, new EventArgs());
            }
        }

        private void radCustom_CheckedChanged(object sender, EventArgs e)
        {
            panelCustom.Enabled = radCustom.Checked;

            if (!panelCustom.Enabled)
            {
                errors.SetError(txtUser, "");
                errors.SetError(txtPassword, "");
                errors.SetError(txtDomain, "");
            }
        }

        private void txtUrl_Validating(object sender, CancelEventArgs e)
        {
            if (!Uri.IsWellFormedUriString(txtUrl.Text, UriKind.Absolute))
            {
                e.Cancel = true;
                errors.SetError(txtUrl, "Invalid URL");
            }
            else
                errors.SetError(txtUrl, "");
        }

        private void txtUser_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUser.Text))
            {
                e.Cancel = true;
                errors.SetError(txtUser, "No user name specified");
            }
            else
                errors.SetError(txtUser, "");
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                errors.SetError(txtPassword, "No password specified");
            }
            else
                errors.SetError(txtPassword, "");
        }

        private void txtDomain_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDomain.Text))
            {
                e.Cancel = true;
                errors.SetError(txtDomain, "No domain specified");
            }
            else
                errors.SetError(txtDomain, "");
        }
    }
}
