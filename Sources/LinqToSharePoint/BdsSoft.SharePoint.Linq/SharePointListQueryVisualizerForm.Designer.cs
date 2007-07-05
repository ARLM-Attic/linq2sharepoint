namespace BdsSoft.SharePoint.Linq
{
    partial class SharePointListQueryVisualizerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SharePointListQueryVisualizerForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEntity = new System.Windows.Forms.TextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLinq = new System.Windows.Forms.RichTextBox();
            this.txtCaml = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtEntity
            // 
            resources.ApplyResources(this.txtEntity, "txtEntity");
            this.txtEntity.Name = "txtEntity";
            this.txtEntity.ReadOnly = true;
            // 
            // btnExecute
            // 
            resources.ApplyResources(this.btnExecute, "btnExecute");
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtLinq
            // 
            resources.ApplyResources(this.txtLinq, "txtLinq");
            this.txtLinq.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtLinq.DetectUrls = false;
            this.txtLinq.Name = "txtLinq";
            this.txtLinq.ReadOnly = true;
            this.txtLinq.ShortcutsEnabled = false;
            this.txtLinq.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtLinq_MouseClick);
            // 
            // txtCaml
            // 
            resources.ApplyResources(this.txtCaml, "txtCaml");
            this.txtCaml.BackColor = System.Drawing.SystemColors.Window;
            this.txtCaml.DetectUrls = false;
            this.txtCaml.Name = "txtCaml";
            this.txtCaml.ReadOnly = true;
            this.txtCaml.ShortcutsEnabled = false;
            // 
            // SharePointListQueryVisualizerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.txtCaml);
            this.Controls.Add(this.txtLinq);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtEntity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.MinimizeBox = false;
            this.Name = "SharePointListQueryVisualizerForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.SharePointListQueryVisualizerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEntity;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox txtLinq;
        private System.Windows.Forms.RichTextBox txtCaml;
    }
}