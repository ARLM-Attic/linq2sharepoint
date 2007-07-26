/*
 * LINQ to SharePoint
 * http://www.codeplex.com/LINQtoSharePoint
 * 
 * Copyright Bart De Smet (C) 2007
 * info@bartdesmet.net - http://blogs.bartdesmet.net/bart
 * 
 * This project is subject to licensing restrictions. Visit http://www.codeplex.com/LINQtoSharePoint/Project/License.aspx for more information.
 */

/*
 * Version history:
 * 
 * 0.2.2 - Introduction of entity wizard
 */

#region Namespace imports

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BdsSoft.SharePoint.Linq.Tools.EntityGenerator;
using System.Diagnostics;

#endregion

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    public partial class ExportLists : UserControl, IWizardStep
    {
        private WizardContext context;
        private bool _next;

        public ExportLists(WizardContext context)
        {
            this.context = context;
            _next = true;

            InitializeComponent();
        }

        private void ExportLists_Load(object sender, EventArgs e)
        {
            this.panel.Panel1MinSize = 100;
            this.panel.Panel2MinSize = 300;
            this.splitContainer2.Panel1MinSize = 100;
            this.splitContainer2.Panel2MinSize = 200;

            if (context.Selection.Lists == null)
            {
                context.Selection.Lists = new List<List>();
                PopulateLists();
            }
        }

        private void PopulateLists()
        {
            lists.Items.Clear();
            foreach (List list in context.Lists)
                lists.Items.Add(new ListListViewItem(list));
            lists.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public string Title
        {
            get { return "Define list entities"; }
        }

        public bool CanNext
        {
            get { return _next; }
        }

        public event EventHandler StateChanged;

        public event EventHandler Working;

        public event EventHandler WorkCompleted;

        public void Cancel()
        {
        }

        private void lnkRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel.Enabled = false;
            if (Working != null)
                Working(this, new EventArgs());

            bgGetLists.RunWorkerAsync();
        }

        private void bgGetLists_DoWork(object sender, DoWorkEventArgs e)
        {
            Helpers.GetLists(context);
        }

        private void bgGetLists_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                PopulateLists();
                if (StateChanged != null)
                    StateChanged(this, new EventArgs());
            }
            else
                MessageBox.Show("Failed to connect to the server.", this.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);

            panel.Enabled = true;
            if (WorkCompleted != null)
                WorkCompleted(this, new EventArgs());
        }

        private void lists_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lists.SelectedItems.Count == 1)
            {
                ListListViewItem item = lists.SelectedItems[0] as ListListViewItem;
                Debug.Assert(item != null);

                if (!item.Loaded)
                    LoadList(item);

                // TODO: display additional data about the list
                fields.Items.Clear();
                foreach (Field f in item.List.Fields)
                {
                    if (!f.IsHidden || f.IsPrimaryKey) //This filtering logic should be part of the lower layer
                    {
                        FieldListViewItem fi = new FieldListViewItem(f);
                        fi.Checked = true;
                        fields.Items.Add(fi);
                    }
                }
                fields.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private void LoadList(ListListViewItem item)
        {
            //
            // TODO: put in background
            //
            item.List = Helpers.GetList(context, item.List.Id.ToString());
            item.Loaded = true;
        }

        private void lists_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListListViewItem item = e.Item as ListListViewItem;
            Debug.Assert(item != null);
            if (item.Checked)
            {
                if (!item.Loaded)
                    LoadList(item);

                context.Selection.Lists.Add(item.List);
            }
            else
                context.Selection.Lists.Remove(item.List);
        }

        private void fields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fields.SelectedItems.Count == 1)
            {
                FieldListViewItem field = fields.SelectedItems[0] as FieldListViewItem;
                Debug.Assert(field != null);

                fieldProperties.SelectedObject = field.Field;
            }
        }

        private void fields_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //MessageBox.Show("Not implemented yet.");
        }
    }

    class ListListViewItem : ListViewItem
    {
        public ListListViewItem(List list)
            : base(list.Name)
        {
            this.List = list;
            this.Loaded = false;
        }

        public List List { get; set; }
        public bool Loaded { get; set; }
    }

    class FieldListViewItem : ListViewItem
    {
        public FieldListViewItem(Field field)
            : base(field.DisplayName)
        {
            this.Field = field;
        }

        public Field Field { get; set; }
    }
}
