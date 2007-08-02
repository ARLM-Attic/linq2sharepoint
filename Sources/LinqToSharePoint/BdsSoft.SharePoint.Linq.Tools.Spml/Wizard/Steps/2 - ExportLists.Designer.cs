namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    partial class ExportLists
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.fields = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.label4 = new System.Windows.Forms.Label();
            this.properties = new System.Windows.Forms.PropertyGrid();
            this.panel = new System.Windows.Forms.SplitContainer();
            this.lists = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.txtContext = new System.Windows.Forms.TextBox();
            this.chkContext = new System.Windows.Forms.CheckBox();
            this.bgList = new System.ComponentModel.BackgroundWorker();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel.Panel1.SuspendLayout();
            this.panel.Panel2.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(359, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the lists you want to generate entities for and click Next to continue.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Lists:";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.fields);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.label4);
            this.splitContainer2.Panel2.Controls.Add(this.properties);
            this.splitContainer2.Size = new System.Drawing.Size(402, 305);
            this.splitContainer2.SplitterDistance = 144;
            this.splitContainer2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Fields:";
            // 
            // fields
            // 
            this.fields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fields.CheckBoxes = true;
            this.fields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.fields.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.fields.Location = new System.Drawing.Point(4, 16);
            this.fields.MultiSelect = false;
            this.fields.Name = "fields";
            this.fields.ShowGroups = false;
            this.fields.Size = new System.Drawing.Size(135, 286);
            this.fields.TabIndex = 0;
            this.fields.UseCompatibleStateImageBehavior = false;
            this.fields.View = System.Windows.Forms.View.Details;
            this.fields.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.fields_ItemChecked);
            this.fields.SelectedIndexChanged += new System.EventHandler(this.fields_SelectedIndexChanged);
            this.fields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.fields_ItemCheck);
            this.fields.Enter += new System.EventHandler(this.fields_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Properties:";
            // 
            // properties
            // 
            this.properties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.properties.Location = new System.Drawing.Point(3, 16);
            this.properties.Name = "properties";
            this.properties.Size = new System.Drawing.Size(248, 289);
            this.properties.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Location = new System.Drawing.Point(6, 52);
            this.panel.Name = "panel";
            // 
            // panel.Panel1
            // 
            this.panel.Panel1.Controls.Add(this.lists);
            this.panel.Panel1.Controls.Add(this.label2);
            // 
            // panel.Panel2
            // 
            this.panel.Panel2.Controls.Add(this.splitContainer2);
            this.panel.Size = new System.Drawing.Size(545, 305);
            this.panel.SplitterDistance = 139;
            this.panel.TabIndex = 1;
            // 
            // lists
            // 
            this.lists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lists.CheckBoxes = true;
            this.lists.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lists.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lists.Location = new System.Drawing.Point(6, 16);
            this.lists.MultiSelect = false;
            this.lists.Name = "lists";
            this.lists.ShowGroups = false;
            this.lists.Size = new System.Drawing.Size(130, 289);
            this.lists.TabIndex = 1;
            this.lists.UseCompatibleStateImageBehavior = false;
            this.lists.View = System.Windows.Forms.View.Details;
            this.lists.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lists_ItemChecked);
            this.lists.SelectedIndexChanged += new System.EventHandler(this.lists_SelectedIndexChanged);
            this.lists.Enter += new System.EventHandler(this.lists_Enter);
            // 
            // txtContext
            // 
            this.txtContext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContext.Location = new System.Drawing.Point(253, 26);
            this.txtContext.Name = "txtContext";
            this.txtContext.Size = new System.Drawing.Size(295, 20);
            this.txtContext.TabIndex = 2;
            this.txtContext.TextChanged += new System.EventHandler(this.txtContext_TextChanged);
            // 
            // chkContext
            // 
            this.chkContext.AutoSize = true;
            this.chkContext.Checked = true;
            this.chkContext.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkContext.Location = new System.Drawing.Point(6, 28);
            this.chkContext.Name = "chkContext";
            this.chkContext.Size = new System.Drawing.Size(241, 17);
            this.chkContext.TabIndex = 1;
            this.chkContext.Text = "Auto-generate data &context object with name:";
            this.chkContext.UseVisualStyleBackColor = true;
            this.chkContext.CheckedChanged += new System.EventHandler(this.chkContext_CheckedChanged);
            // 
            // bgList
            // 
            this.bgList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgList_DoWork);
            this.bgList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgList_RunWorkerCompleted);
            // 
            // ExportLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkContext);
            this.Controls.Add(this.txtContext);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.label1);
            this.Name = "ExportLists";
            this.Size = new System.Drawing.Size(554, 360);
            this.Load += new System.EventHandler(this.ExportLists_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.panel.Panel1.ResumeLayout(false);
            this.panel.Panel1.PerformLayout();
            this.panel.Panel2.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PropertyGrid properties;
        private System.Windows.Forms.SplitContainer panel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lists;
        private System.Windows.Forms.ListView fields;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtContext;
        private System.Windows.Forms.CheckBox chkContext;
        private System.Windows.Forms.Label label4;
        private System.ComponentModel.BackgroundWorker bgList;

    }
}
