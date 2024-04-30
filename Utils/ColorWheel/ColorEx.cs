using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Utils.ColorWheel
{
	public class ColorEx
	{
		public static short MaxHue = 360;
		public static short MaxSaturation = 100;

		private bool _IsRGBDirty = false;
		private bool _IsHSVDirty = false;

		public ColorEx()
		{
			RGB2HSV();
		}

		public ColorEx Clone()
		{
			return (new ColorEx()
			{
				R = R,
				G = G,
				B = B
			});
		}

		#region Properties

		private Color _Color = Color.FromArgb(255, 128, 128, 128);

		public Color Color
		{
			get
			{
				if (_IsRGBDirty)
				{
					HSV2RGB();
				}

				return (_Color);
			}
		}

		public byte R
		{
			get
			{
				if (_IsRGBDirty)
				{
					HSV2RGB();
				}

				return (_Color.R);
			}
			set
			{
				_Color = _Color.SetR(value);

				_IsHSVDirty = true;
			}
		}

		public byte G
		{
			get
			{
				if (_IsRGBDirty)
				{
					HSV2RGB();
				}

				return (_Color.G);
			}
			set
			{
				_Color = _Color.SetG(value);

				_IsHSVDirty = true;
			}
		}

		public byte B
		{
			get
			{
				if (_IsRGBDirty)
				{
					HSV2RGB();
				}

				return (_Color.B);
			}
			set
			{
				_Color = _Color.SetB(value);

				_IsHSVDirty = true;
			}
		}

		private double _H = 0;

		public double H
		{
			get
			{
				if (_IsHSVDirty)
				{
					RGB2HSV();
				}

				return (_H);
			}
			set
			{
				// Hue is circular (degree)
				_H = (short)((value < 0 ? 360 : 0) + (value % 360));

				_IsRGBDirty = true;
			}
		}

		private double _S = 0;

		public double S
		{
			get
			{
				if (_IsHSVDirty)
				{
					RGB2HSV();
				}

				return (_S);
			}
			set
			{
				if (value >= 0 && value <= 100)
				{
					_S = value;

					_IsRGBDirty = true;
				}
			}
		}

		private double _V = 0;

		public double V
		{
			get
			{
				if (_IsHSVDirty)
				{
					RGB2HSV();
				}

				return (_V);
			}
			set
			{
				if (value >= 0 && value <= 100)
				{
					_V = value;

					_IsRGBDirty = true;
				}
			}
		}

		#endregion

		#region Helpers

		private void RGB2HSV()
		{
			double h;
			double s;
			double v;

			Extensions.ColorToHSV(_Color, out h, out s, out v);

			_H = h;
			_S = s;
			_V = v;

			_IsHSVDirty = false;
		}

		private void HSV2RGB()
		{
			_Color = Extensions.ColorFromHSV(_H, _S, _V);

			_IsRGBDirty = false;
		}

		#endregion
	}
}
