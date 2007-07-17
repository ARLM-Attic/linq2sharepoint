using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BdsSoft.SharePoint.Linq.Tools.Spml
{
    public partial class Line : Control
    {
        public Line()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.DrawLine(Pens.White, 0, 0, this.Width, 0);
            base.OnPaint(pe);
        }
    }
}
