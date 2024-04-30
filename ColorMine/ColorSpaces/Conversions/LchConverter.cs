using ColorMine.ColorSpaces.Utility;
using System;

namespace ColorMine.ColorSpaces.Conversions
{
    internal static class LchConverter
    {
        internal static void ToColorSpace(IRgb color, ILch item)
        {
            var lab = color.To<Lab>();
            var h = MathUtils.RadToDeg(Math.Atan2(lab.B, lab.A));

            if (h < 0)
            {
                h += 360.0;
            }
            else if (h >= 360)
            {
                h -= 360.0;
            }

            item.L = lab.L;
            item.C = Math.Sqrt(lab.A * lab.A + lab.B * lab.B);
            item.H = h;
        }

        internal static IRgb ToColor(ILch item)
        {
            var hRadians = MathUtils.DegToRad(item.H);
            var lab = new Lab
                {
                    L = item.L,
                    A = Math.Cos(hRadians) * item.C,
                    B = Math.Sin(hRadians) * item.C
                };
            return lab.To<Rgb>();
        }
    }
}