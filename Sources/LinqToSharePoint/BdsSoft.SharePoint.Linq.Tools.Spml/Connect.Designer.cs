namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    partial class Connect
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label3 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.errors = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.bgConnect = new System.ComponentModel.BackgroundWorker();
            this.panel = new System.Windows.Forms.Panel();
            this.lblError = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelCustom = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radCustom = new System.Windows.Forms.RadioButton();
            this.radNetwork = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.errors)).BeginInit();
            this.panel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelCustom.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(405, 39);
            this.label3.TabIndex = 0;
            this.label3.Text = "Enter information of the SharePoint site to export entities from and click on \'Te" +
                "st connection\'. If the connection is successful, click Next to continue.";
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(291, 210);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(100, 23);
            this.btnTest.TabIndex = 11;
            this.btnTest.Text = "&Test connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // errors
            // 
            this.errors.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errors.ContainerControl = this;
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.errors.SetIconPadding(this.txtUrl, 5);
            this.txtUrl.Location = new System.Drawing.Point(59, 0);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(302, 20);
            this.txtUrl.TabIndex = 2;
            this.txtUrl.TextChanged += new System.EventHandler(this.txtUrl_TextChanged);
            this.txtUrl.Enter += new System.EventHandler(this.txtUrl_Enter);
            this.txtUrl.Validating += new System.ComponentModel.CancelEventHandler(this.txtUrl_Validating);
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.errors.SetIconPadding(this.txtUser, 5);
            this.txtUser.Location = new System.Drawing.Point(67, 1);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(259, 20);
            this.txtUser.TabIndex = 1;
            this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
            this.txtUser.Enter += new System.EventHandler(this.txtUser_Enter);
            this.txtUser.Validating += new System.ComponentModel.CancelEventHandler(this.txtUser_Validating);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.errors.SetIconPadding(this.txtPassword, 5);
            this.txtPassword.Location = new System.Drawing.Point(67, 29);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(259, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            this.txtPassword.Validating += new System.ComponentModel.CancelEventHandler(this.txtPassword_Validating);
            // 
            // txtDomain
            // 
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.errors.SetIconPadding(this.txtDomain, 5);
            this.txtDomain.Location = new System.Drawing.Point(67, 57);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(259, 20);
            this.txtDomain.TabIndex = 5;
            this.txtDomain.TextChanged += new System.EventHandler(this.txtDomain_TextChanged);
            this.txtDomain.Enter += new System.EventHandler(this.txtDomain_Enter);
            this.txtDomain.Validating += new System.ComponentModel.CancelEventHandler(this.txtDomain_Validating);
            // 
            // bgConnect
            // 
            this.bgConnect.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgConnect_DoWork);
            this.bgConnect.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgConnect_RunWorkerCompleted);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.lblError);
            this.panel.Controls.Add(this.label1);
            this.panel.Controls.Add(this.txtUrl);
            this.panel.Controls.Add(this.btnTest);
            this.panel.Controls.Add(this.label2);
            this.panel.Controls.Add(this.groupBox1);
            this.panel.Location = new System.Drawing.Point(9, 42);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(391, 237);
            this.panel.TabIndex = 12;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(0, 215);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(196, 13);
            this.lblError.TabIndex = 12;
            this.lblError.Text = "Failed to connect to the SharePoint site.";
            this.lblError.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Site URL:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "E.g. http://wss3demo";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panelCustom);
            this.groupBox1.Controls.Add(this.radCustom);
            this.groupBox1.Controls.Add(this.radNetwork);
            this.groupBox1.Location = new System.Drawing.Point(0, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 156);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log on to the server";
            // 
            // panelCustom
            // 
            this.panelCustom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCustom.Controls.Add(this.txtDomain);
            this.panelCustom.Controls.Add(this.txtPassword);
            this.panelCustom.Controls.Add(this.txtUser);
            this.panelCustom.Controls.Add(this.label6);
            this.panelCustom.Controls.Add(this.label5);
            this.panelCustom.Controls.Add(this.label4);
            this.panelCustom.Enabled = false;
            this.panelCustom.Location = new System.Drawing.Point(35, 65);
            this.panelCustom.Name = "panelCustom";
            this.panelCustom.Size = new System.Drawing.Size(350, 86);
            this.panelCustom.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "&Domain:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "&Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "&User name:";
            // 
            // radCustom
            // 
            this.radCustom.AutoSize = true;
            this.radCustom.Location = new System.Drawing.Point(15, 41);
            this.radCustom.Name = "radCustom";
            this.radCustom.Size = new System.Drawing.Size(114, 17);
            this.radCustom.TabIndex = 1;
            this.radCustom.TabStop = true;
            this.radCustom.Text = "&Custom credentials";
            this.radCustom.UseVisualStyleBackColor = true;
            this.radCustom.CheckedChanged += new System.EventHandler(this.radCustom_CheckedChanged);
            // 
            // radNetwork
            // 
            this.radNetwork.AutoSize = true;
            this.radNetwork.Checked = true;
            this.radNetwork.Location = new System.Drawing.Point(15, 18);
            this.radNetwork.Name = "radNetwork";
            this.radNetwork.Size = new System.Drawing.Size(154, 17);
            this.radNetwork.TabIndex = 0;
            this.radNetwork.TabStop = true;
            this.radNetwork.Text = "Default &network credentials";
            this.radNetwork.UseVisualStyleBackColor = true;
            // 
            // Connect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.label3);
            this.Name = "Connect";
            this.Size = new System.Drawing.Size(411, 282);
            this.Load += new System.EventHandler(this.Connect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errors)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelCustom.ResumeLayout(false);
            this.panelCustom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.ErrorProvider errors;
        private System.ComponentModel.BackgroundWorker bgConnect;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panelCustom;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radCustom;
        private System.Windows.Forms.RadioButton radNetwork;
        private System.Windows.Forms.Label lblError;
    }
}
