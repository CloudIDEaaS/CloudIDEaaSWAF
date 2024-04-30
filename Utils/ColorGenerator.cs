using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using System.Drawing;

namespace Utils
{
    public class ColorGenerator
    {
        private static List<HSLColor> colors;
        private static int index = 0;
        private int Luminosity { get; set; } 

        public ColorGenerator(int luminosity = 250)
        {
            var step = 240 / 8;

            colors = new List<HSLColor>();

            for (var i = 0; i < 240; i += step)
            {
                var color = new HSLColor((double)i, 240, luminosity);

                colors.Add(color);
            }

            for (var i = 30; i < 255; i += step)
            {
                var color = new HSLColor((double)i, 220, luminosity - 20);

                colors.Add(color);
            }

            colors = colors.Randomize((c1, c2) => Math.Abs(c1.Hue - c2.Hue) > 61).ToList();
        }

        public Color Next
        {
            get
            {
                Color color;

                index = NumberExtensions.ScopeRange(index, colors.Count - 1);

                color = colors[index++];

                return color;
            }
        }
    }
}
