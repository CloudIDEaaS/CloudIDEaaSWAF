using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using unvell.D2DLib;
using unvell.D2DLib.WinForm;

namespace Utils.HighGraphics
{
    public partial class RenderControl : D2DControl
    {
        public event RenderEventHandler Render;

        public RenderControl()
        {
            this.InitializeComponent();
        }

        protected override void OnRender(D2DGraphics g)
        {
            base.OnRender(g);

            if (Render != null)
            {
                Render(this, new RenderEventArgs(g));
            }
        }
    }
}
