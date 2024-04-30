using System;

namespace ColorMine.ColorSpaces.Utility
{
    internal static class MathUtils
    {
        internal static double DegToRad(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        internal static double RadToDeg(double radians)
        {
            return radians / Math.PI * 180;
        }
    }
}
