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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.fields = new System.Windows.Forms.ListView();
            this.fieldProperties = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.definition = new System.Windows.Forms.WebBrowser();
            this.lnkRefresh = new System.Windows.Forms.LinkLabel();
            this.panel = new System.Windows.Forms.SplitContainer();
            this.lists = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.bgGetLists = new System.ComponentModel.BackgroundWorker();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(396, 318);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer2);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(388, 292);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Fields";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.fields);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.fieldProperties);
            this.splitContainer2.Size = new System.Drawing.Size(388, 292);
            this.splitContainer2.SplitterDistance = 139;
            this.splitContainer2.TabIndex = 0;
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
            this.fields.Location = new System.Drawing.Point(4, 3);
            this.fields.MultiSelect = false;
            this.fields.Name = "fields";
            this.fields.ShowGroups = false;
            this.fields.Size = new System.Drawing.Size(130, 286);
            this.fields.TabIndex = 3;
            this.fields.UseCompatibleStateImageBehavior = false;
            this.fields.View = System.Windows.Forms.View.Details;
            this.fields.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.fields_ItemChecked);
            this.fields.SelectedIndexChanged += new System.EventHandler(this.fields_SelectedIndexChanged);
            // 
            // fieldProperties
            // 
            this.fieldProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldProperties.Location = new System.Drawing.Point(3, 9);
            this.fieldProperties.Name = "fieldProperties";
            this.fieldProperties.Size = new System.Drawing.Size(239, 283);
            this.fieldProperties.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.definition);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(388, 292);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Definition";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // definition
            // 
            this.definition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.definition.Location = new System.Drawing.Point(3, 3);
            this.definition.MinimumSize = new System.Drawing.Size(20, 20);
            this.definition.Name = "definition";
            this.definition.Size = new System.Drawing.Size(382, 286);
            this.definition.TabIndex = 0;
            // 
            // lnkRefresh
            // 
            this.lnkRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkRefresh.AutoSize = true;
            this.lnkRefresh.Location = new System.Drawing.Point(92, 0);
            this.lnkRefresh.Name = "lnkRefresh";
            this.lnkRefresh.Size = new System.Drawing.Size(44, 13);
            this.lnkRefresh.TabIndex = 1;
            this.lnkRefresh.TabStop = true;
            this.lnkRefresh.Text = "Refresh";
            this.lnkRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefresh_LinkClicked);
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Location = new System.Drawing.Point(6, 33);
            this.panel.Name = "panel";
            // 
            // panel.Panel1
            // 
            this.panel.Panel1.Controls.Add(this.lists);
            this.panel.Panel1.Controls.Add(this.label2);
            this.panel.Panel1.Controls.Add(this.lnkRefresh);
            // 
            // panel.Panel2
            // 
            this.panel.Panel2.Controls.Add(this.tabControl1);
            this.panel.Size = new System.Drawing.Size(545, 324);
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
            this.lists.Size = new System.Drawing.Size(130, 308);
            this.lists.TabIndex = 2;
            this.lists.UseCompatibleStateImageBehavior = false;
            this.lists.View = System.Windows.Forms.View.Details;
            this.lists.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lists_ItemChecked);
            this.lists.SelectedIndexChanged += new System.EventHandler(this.lists_SelectedIndexChanged);
            // 
            // bgGetLists
            // 
            this.bgGetLists.WorkerSupportsCancellation = true;
            this.bgGetLists.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgGetLists_DoWork);
            this.bgGetLists.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgGetLists_RunWorkerCompleted);
            // 
            // ExportLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.label1);
            this.Name = "ExportLists";
            this.Size = new System.Drawing.Size(554, 360);
            this.Load += new System.EventHandler(this.ExportLists_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.WebBrowser definition;
        private System.Windows.Forms.LinkLabel lnkRefresh;
        private System.Windows.Forms.PropertyGrid fieldProperties;
        private System.Windows.Forms.SplitContainer panel;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView lists;
        private System.Windows.Forms.ListView fields;
        private System.ComponentModel.BackgroundWorker bgGetLists;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;

    }
}
