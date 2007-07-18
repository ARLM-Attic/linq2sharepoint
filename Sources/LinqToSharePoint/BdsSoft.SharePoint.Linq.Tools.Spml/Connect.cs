using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml;
using System.Web.Services.Protocols;
using System.Diagnostics;

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
            txtUrl.Focus();
            txtUrl.Select(txtUrl.Text.Length, 0);
            Form f = this.ParentForm;
            f.AcceptButton.NotifyDefault(false);
            f.AcceptButton = btnTest;
            f.AcceptButton.NotifyDefault(true);
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
        public event EventHandler Working;
        public event EventHandler WorkCompleted;

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren(ValidationConstraints.Enabled | ValidationConstraints.Selectable))
            {
                panel.Enabled = false;
                if (Working != null)
                    Working(this, new EventArgs());

                string url = txtUrl.Text;
                string user = txtUser.Text;
                string password = txtPassword.Text;
                string domain = txtDomain.Text;

                ConnectionParameters conn = new ConnectionParameters() { Url = url, User = user, Password = password, Domain = domain, CustomAuthentication = radCustom.Checked };
                bgConnect.RunWorkerAsync(conn);
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

            Reset();
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
            /*
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                errors.SetError(txtPassword, "No password specified");
            }
            else
                errors.SetError(txtPassword, "");
             */
        }

        private void txtDomain_Validating(object sender, CancelEventArgs e)
        {
            /*
            if (string.IsNullOrEmpty(txtDomain.Text))
            {
                e.Cancel = true;
                errors.SetError(txtDomain, "No domain specified");
            }
            else
                errors.SetError(txtDomain, "");
             */
        }

        private void bgConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                _next = true;
                if (StateChanged != null)
                    StateChanged(this, new EventArgs());
            }
            else
            {
                lblError.Visible = false;
            }

            panel.Enabled = true;
            if (WorkCompleted != null)
                WorkCompleted(this, new EventArgs());
        }

        private void bgConnect_DoWork(object sender, DoWorkEventArgs e)
        {
            ConnectionParameters parameters = (ConnectionParameters)e.Argument;

            context.ConnectionParameters = parameters;
            Helpers.GetLists(context);
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void txtDomain_TextChanged(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            _next = false;
            if (StateChanged != null)
                StateChanged(this, new EventArgs());
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }

        private void txtUser_Enter(object sender, EventArgs e)
        {
            txtUser.SelectAll();
        }

        private void txtDomain_Enter(object sender, EventArgs e)
        {
            txtDomain.SelectAll();
        }

        private void txtUrl_Enter(object sender, EventArgs e)
        {
            txtUrl.SelectAll();
        }

        public void Cancel()
        {
            if (bgConnect.IsBusy && !bgConnect.CancellationPending)
                bgConnect.CancelAsync();
        }

        private void txtUrl_Validated(object sender, EventArgs e)
        {
            //errors.SetError(txtUrl, "");
        }

        private void txtUser_Validated(object sender, EventArgs e)
        {
            //errors.SetError(txtUser, "");
        }
    }

    public class ConnectionParameters
    {
        public string Url { get; set; }
        public bool CustomAuthentication { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        public string ToSpml()
        {
            XmlDataDocument doc = new XmlDataDocument();
            XmlElement conn = doc.CreateElement("Connection");

            XmlAttribute url = doc.CreateAttribute("Url");
            url.Value = Url;
            conn.Attributes.Append(url);

            if (CustomAuthentication)
            {
                XmlAttribute user = doc.CreateAttribute("User");
                user.Value = User ?? "";
                conn.Attributes.Append(user);

                XmlAttribute password = doc.CreateAttribute("Password");
                password.Value = Password ?? "";
                conn.Attributes.Append(password);

                XmlAttribute domain = doc.CreateAttribute("Domain");
                domain.Value = Domain ?? "";
                conn.Attributes.Append(domain);
            }

            return conn.OuterXml;
        }
    }

    public class Connection
    {
        public WebServices.Lists ListsProxy { get; set; }
        public ConnectionParameters Parameters { get; set; }
    }
}
