using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    partial class Finish : UserControl, IWizardStep
    {
        private Context context;

        public Finish(Context context)
        {
            this.context = context;

            InitializeComponent();
        }

        private void Finish_Load(object sender, EventArgs e)
        {

        }

        public string Title
        {
            get { return "Entity mapping generation completed."; }
        }
    }
}
