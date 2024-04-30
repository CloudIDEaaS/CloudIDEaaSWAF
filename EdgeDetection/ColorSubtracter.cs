using System;
using System.Drawing;

namespace BitmapedTextures
{
    public abstract class ColorSubtracter
    {
        protected string name = "NoName";
        public abstract double GetColorDiff(Color c1, Color c2);

        public override string ToString()
        {
            return name;
        }
    }

    class RedSubtractor : ColorSubtracter
    {
        public RedSubtractor()
        {
            name = "Red";
        }

        public override double GetColorDiff(Color c1, Color c2)
        {
            return Math.Abs(c1.R - c2.R);
        }
    }

    class GreenSubtractor : ColorSubtracter
    {
        public GreenSubtractor()
        {
            name = "Green";
        }

        public override double GetColorDiff(Color c1, Color c2)
        {
            return Math.Abs(c1.G - c2.G);
        }
    }

    class BlueSubtractor : ColorSubtracter
    {
        public BlueSubtractor()
        {
            name = "Blue";
        }

        public override double GetColorDiff(Color c1, Color c2)
        {
            return Math.Abs(c1.B - c2.B);
        }
    }

    class HueSubtractor : ColorSubtracter
    {
        public HueSubtractor()
        {
            name = "Hue";
        }

        public override double GetColorDiff(Color c1, Color c2)
        {
            if ((c1.GetHue() < 0.01f || c2.GetHue() < 0.01f))
                return Math.Abs(c1.GetHue() - c2.GetHue());
            else
                return 0;
        }
    }

    public class BrightnessSubtractor : ColorSubtracter
    {
        public BrightnessSubtractor()
        {
            name = "Lightness";
        }

        public override double GetColorDiff(Color c1, Color c2)
        {
            double diffR = Math.Abs(c1.R - c2.R);
            double diffG = Math.Abs(c1.G - c2.G);
            double diffB = Math.Abs(c1.B - c2.B);
            double diffW = Math.Abs(c1.GetBrightness() - c2.GetBrightness());
            
            return Math.Sqrt(diffR * diffR + diffG * diffG + diffB * diffB + diffW * diffW);
        }
    }

    class EuclidSubtractor : ColorSubtracter
    {
        public EuclidSubtractor()
        {
            name = "Euclidian";
        }

        public override double GetColorDiff(Color c1, Color c2)
        {
            double diffR = Math.Abs(c1.R - c2.R);
            double diffG = Math.Abs(c1.G - c2.G);
            double diffB = Math.Abs(c1.B - c2.B);
            
            return Math.Sqrt(diffR * diffR + diffG * diffG + diffB * diffB);
        }
    }

    class LuminoSubtractor : ColorSubtracter
    {
        public LuminoSubtractor()
        {
            name = "Lumino"; // Luma, for historical reasons referred to as intensity is computed according to the following formula
        }

        public override double GetColorDiff(Color c1, Color c2)
        {
            double color1 = 0.3 * c1.R + 0.59 * c1.G + 0.11 * c1.B;
            double color2 = 0.3 * c2.R + 0.59 * c2.G + 0.11 * c2.B;

            return color1 - color2;
        }
    }

    class AvgSubtractor : ColorSubtracter
    {
        public AvgSubtractor()
        {
            name = "Average";
        }

        public override double GetColorDiff(Color c1, Color c2)
        {
            double color1 = (c1.R + c1.G + c1.B) / 3;
            double color2 = (c2.R + c2.G + c2.B) / 3;

            return Math.Abs(color1 - color2);
        }
    }
}
