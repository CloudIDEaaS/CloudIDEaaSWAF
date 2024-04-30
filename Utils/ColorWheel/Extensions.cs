using ColorMine.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Utils.ColorWheel
{
    public static class Extensions
    {
        static Extensions()
        {
            var g = 0xbf;
            var hex = "#febf00";
            var color = ColorTranslator.FromHtml(hex);
            var colorEx = color.ToColorEx();

            colorEx.H += 180;
            
            var color2 = colorEx.FromColorEx();
            var hex2 = ColorTranslator.ToHtml(color2);
        }

        public static IEnumerable<KeyValuePair<string, Color>> GetBrightColors(this Dictionary<string, Color?> colors)
        {
            return colors.Where(c => c.Value != null && c.Value.Value.GetBrightness() > .5).Select(p => new KeyValuePair<string, Color>(p.Key, p.Value.Value));
        }

        public static IEnumerable<Color> GetGrays()
        {
            var step = 15;

            for (var i = 50; i <= 150; i += step)
            {
                var color = Color.FromArgb(i, i, i);

                yield return color;
            }
        }

        public static IEnumerable<Color> GetLightGrays()
        {
            var step = 5;

            for (var i = 220; i <= 245; i += step)
            {
                var color = Color.FromArgb(i, i, i);

                yield return color;
            }
        }

        public static IEnumerable<Color> GetPastels()
        {
            var step = 240 / 16;

            for (var i = 0; i < 240; i += step)
            {
                var color = new HSLColor((double)i, 240, 160);

                yield return color;
            }
        }

        public static IEnumerable<Color> GetCalmColors()
        {
            var calmColors = new[]
            {
                "#eae7e2",
                "#d7e2e8",
                "#b9cbd9",
                "#fcc9c5",
                "#feddd8",
                "#cfe3e2",
                "#dfb9a5",
                "#b6aa8d",
                "#b3c18d"
            };

            return calmColors.Select(c => ColorTranslator.FromHtml(c));
        }

        public static IEnumerable<Color> GetColorwheelColors()
        {
            var colorEx = ColorTranslator.FromHtml("#bb00fe").ToColorEx();

            for (var x = 0; x < 24; x++)
            {
                var wheelColor = colorEx.Clone();
                var degrees = x * 15;

                wheelColor.H += (short) degrees;

                yield return wheelColor.FromColorEx();
            }
        }

        public static IEnumerable<Color> GetWarmColors()
        {
            var colorWheelColors = GetColorwheelColors().ToList();

            return colorWheelColors.Take((int)(colorWheelColors.Count * .75));
        }

        public static IEnumerable<Color> GetCoolColors()
        {
            var colorWheelColors = GetColorwheelColors().ToList();

            return colorWheelColors.Skip((int)(colorWheelColors.Count * .25));
        }

        public static Color AssureDark(this Color color, float percentAmount)
        {
            if (color.GetBrightness() > percentAmount)
            {
                return color.Lighten(-percentAmount * 2);
            }

            return color;
        }

        public static IEnumerable<Color> GetConservativeColors()
        {
            var conservativeColors = new[] 
            {
                "#151c2e",
                "#154b94",
                "#264271",
                "#2a4463",
                "#91aecc",
                "#a2b5df",
                "#191718",
                "#4d2d20",
                "#917368",
                "#836d70",
                "#6d3d3b",
                "#541424",
                "#524f3c",
                "#193d39",
                "#11211e",
                "#0b1013",
                "#451008",
                "#5f402e"
            };

            return conservativeColors.Select(c => ColorTranslator.FromHtml(c));
        }

        public static IEnumerable<Color> GetRoyalColors()
        {
            var color = ColorTranslator.FromHtml("#febf00");
            var monochromatics = color.GetMonochromatic();

            foreach (var royalColor in monochromatics.GetTupleValues().Cast<Color>().Concat(new [] { color }))
            {
                yield return royalColor;
            }

            color = ColorTranslator.FromHtml("#bb00fe");
            monochromatics = color.GetMonochromatic();

            foreach (var royalColor in monochromatics.GetTupleValues().Cast<Color>().Concat(new[] { color }))
            {
                yield return royalColor;
            }
        }

        public static Color Royalize(this Color color)
        {
            var royalColors = GetRoyalColors();
            var closestColor = royalColors.OrderBy(c => c.Compare(color, ColorCompareOption.CIE2000)).First();
            Color returnColor;

            returnColor = closestColor.Blend(color, .65).Lighten(.3);

            return returnColor;
        }

        public static Color Pastelize(this Color color)
        {
            var pastelColors = GetPastels();
            var closestColors = pastelColors.OrderBy(c => c.Compare(color, ColorCompareOption.CIE2000)).Skip(1).Take(5);
            var closestColor = closestColors.Randomize().First();

            return closestColor;
        }

        public static Color Darkerize(this Color color, int amount = 25)
        {
            return color.SetLight(amount);
        }

        public static Color Bolderize(this Color color, int amount = 40)
        {
            color = color.GetAnalogousRight().GetTupleValues().Cast<Color>().Concat(color.GetAnalogousLeft().GetTupleValues().Cast<Color>()).Randomize().First();

            return color.SetLight(amount);
        }

        public static Color Neutralize(this Color color, int amount = 100)
        {
            color = color.GetAnalogousRight().GetTupleValues().Cast<Color>().Concat(color.GetAnalogousLeft().GetTupleValues().Cast<Color>()).Randomize().First();
            color = color.GetMonochromatic().GetTupleValues().Skip(1).Cast<Color>().Randomize().First();

            return color.SetLight(amount);
        }

        public static Color Warmerize(this Color color)
        {
            var warmColors = GetWarmColors();
            var closestColors = warmColors.OrderBy(c => c.Compare(color, ColorCompareOption.CIE2000)).Skip(1).Take(5);
            var closestColor = closestColors.Randomize().First();

            return closestColor;
        }

        public static Color Coolerize(this Color color)
        {
            var coolColors = GetCoolColors();
            var closestColors = coolColors.OrderBy(c => c.Compare(color, ColorCompareOption.CIE2000)).Skip(1).Take(5);
            var closestColor = closestColors.Randomize().First();

            return closestColor;
        }

        public static Color Feminize(this Color color)
        {
            return color.Saturate(.2);
        }

        public static Color Masculinize(this Color color)
        {
            return color.Saturate(.8);
        }

        public static bool AdjustForecolor(this Label label)
        {
            var backColor = label.BackColor;
            var brightness = backColor.GetBrightness();

            if (brightness <= .50)
            {
                label.ForeColor = Color.White;
                return false;
            }
            else
            {
                label.ForeColor = Color.Black;
                return true;
            }
        }

        public static void SetColor(this ColorPicker colorPicker, Color color, string colorName = null)
        {
            KeyValuePair<string, Color?> pair;

            pair = colorPicker.Colors.Where(c => !c.Key.IsNullOrEmpty()).FirstOrDefault(k => k.Value.Value.A == color.A && k.Value.Value.R == color.R && k.Value.Value.G == color.G && k.Value.Value.B == color.B);

            if (pair.Key != null && colorName == null)
            {
                int index;

                colorName = pair.Key;
                index = colorPicker.Colors.IndexOf(k => k.Key == colorName);

                colorPicker.SelectedIndex = index;
            }
            else
            {
                colorPicker.AddCustomColor(colorName, color, true);
            }
        }

        public static ColorEx ToColorEx(this Color color)
        {
            return new ColorEx { R = color.R, G = color.G, B = color.B };
        }

        public static Color FromColorEx(this ColorEx color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }

        public static Color GetComplement(this Color color)
        {
            var colorEx = color.ToColorEx();
            var colorComplement = colorEx.Clone();

            colorComplement.H += 180;

            return colorComplement.FromColorEx();
        }

        public static Color Shade(this Color color)
        {
            color = color.Lighten(-.3);
            color = color.Saturate(.3);

            return color;
        }

        public static Color Tint(this Color color)
        {
            color = color.Lighten(.5);
            color = color.Saturate(.3);

            return color;
        }

        public static Color GetLeft(this Color color, short degrees = 20)
        {
            var colorEx = color.ToColorEx();
            var colorComplement = colorEx.Clone();

            colorComplement.H -= degrees;

            return colorComplement.FromColorEx();
        }

        public static Color GetRight(this Color color, short degrees = 20)
        {
            var colorEx = color.ToColorEx();
            var colorComplement = colorEx.Clone();

            colorComplement.H += degrees;

            return colorComplement.FromColorEx();
        }

        public static (Color, Color) GetSplitComplements(this Color color, short split = 20)
        {
            var colorEx = color.ToColorEx();
            var colorComplement1 = colorEx.Clone();
            var colorComplement2 = colorEx.Clone();

            colorComplement1.H += (short)(180 + split);
            colorComplement2.H += (short)(180 - split);

            return (colorComplement1.FromColorEx(), colorComplement2.FromColorEx());
        }

        public static (Color, Color) GetTriads(this Color color)
        {
            var colorEx = color.ToColorEx();
            var colorTriad1 = colorEx.Clone();
            var colorTriad2 = colorEx.Clone();

            colorTriad1.H += 120;
            colorTriad2.H -= 120;

            return (colorTriad1.FromColorEx(), colorTriad2.FromColorEx());
        }

        public static (Color, Color, Color) GetTetrads(this Color color)
        {
            var colorEx = color.ToColorEx();
            var colorTetrad1 = colorEx.Clone();
            var colorTetrad2 = colorEx.Clone();
            var colorTetrad3 = colorEx.Clone();

            colorTetrad1.H += 180;
            colorTetrad2.H += 90;
            colorTetrad3.H -= 90;

            return (colorTetrad1.FromColorEx(), colorTetrad2.FromColorEx(), colorTetrad3.FromColorEx());
        }

        public static (Color, Color, Color, Color) GetQuintads(this Color color)
        {
            var colorEx = color.ToColorEx();
            var colorQuintad1 = colorEx.Clone();
            var colorQuintad2 = colorEx.Clone();
            var colorQuintad3 = colorEx.Clone();
            var colorQuintad4 = colorEx.Clone();

            colorQuintad1.H += 144;
            colorQuintad2.H -= 144;
            colorQuintad3.H += 72;
            colorQuintad4.H -= 72;

            return (colorQuintad1.FromColorEx(), colorQuintad2.FromColorEx(), colorQuintad3.FromColorEx(), colorQuintad4.FromColorEx());
        }

        public static (Color, Color) GetAnalogousLeft(this Color color, int degrees = 20)
        {
            var colorEx = color.ToColorEx();
            var colorAnalogous1 = colorEx.Clone();
            var colorAnalogous2 = colorEx.Clone();

            colorAnalogous1.H -= (short) degrees;
            colorAnalogous2.H -= (short)(degrees * 2);

            return (colorAnalogous1.FromColorEx(), colorAnalogous2.FromColorEx());
        }

        public static (Color, Color) GetAnalogousRight(this Color color, int degrees = 20)
        {
            var colorEx = color.ToColorEx();
            var colorAnalogous1 = colorEx.Clone();
            var colorAnalogous2 = colorEx.Clone();

            colorAnalogous1.H += (short) degrees;
            colorAnalogous2.H += (short)(degrees * 2);

            return (colorAnalogous1.FromColorEx(), colorAnalogous2.FromColorEx());
        }

        public static (Color, Color, Color, Color) GetMonochromatic(this Color color, double gap = .20)
        {
            var colorEx = color.ToColorEx();
            byte nS = (byte)(colorEx.S % gap);
            var colorMonochromatic1 = new ColorEx() { H = colorEx.H, S = (double)(nS + gap), V = colorEx.V };
            var colorMonochromatic2 = new ColorEx() { H = colorEx.H, S = (double)(nS + (2 * gap)), V = colorEx.V };
            var colorMonochromatic3 = new ColorEx() { H = colorEx.H, S = (double)(nS + (3 * gap)), V = colorEx.V };
            var colorMonochromatic4 = new ColorEx() { H = colorEx.H, S = (double)(nS + (4 * gap)), V = colorEx.V };

            return (colorMonochromatic1.FromColorEx(), colorMonochromatic2.FromColorEx(), colorMonochromatic3.FromColorEx(), colorMonochromatic4.FromColorEx());
        }

        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            var rgb = new Rgb(color.R, color.G, color.B);
            var hsv = new Hsv(rgb);

            hue = hsv.H;
            saturation = hsv.S;
            value = hsv.V;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            var hsv = new Hsv(hue, saturation, value);
            var rgb = new Rgb(hsv);

            return Color.FromArgb((int) rgb.R, (int)rgb.G, (int)rgb.B);
        }

        public static Color Blend(this Color color, Color backColor, double amount)
        {
            byte a = (byte)(color.A * amount + backColor.A * (1 - amount));
            byte r = (byte)(color.R * amount + backColor.R * (1 - amount));
            byte g = (byte)(color.G * amount + backColor.G * (1 - amount));
            byte b = (byte)(color.B * amount + backColor.B * (1 - amount));

            return Color.FromArgb(a, r, g, b);
        }
    }
}
