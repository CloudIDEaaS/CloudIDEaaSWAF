using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unvell.D2DLib;

namespace Utils.HighGraphics
{
    public delegate void RenderEventHandler(object sender, RenderEventArgs e);

    public class RenderEventArgs
    {
        public D2DGraphics Graphics { get; }

        public RenderEventArgs(D2DGraphics graphics)
        {
            this.Graphics = graphics;
        }
    }
}
