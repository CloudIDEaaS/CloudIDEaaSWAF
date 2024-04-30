using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace unvell.D2DLib
{
  public class D2DEffect
  {
    public D2DEffect(IntPtr handle)
    {
      Handle = handle;
    }

    public IntPtr Handle { get; }

    public void SetInput(int index, D2DBitmap bitmap, bool invalidate)
    {
      D2D.SetInput(this.Handle, index, bitmap.Handle, invalidate);
    }
  }
}
