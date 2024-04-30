using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ColorChangeFilter
    {
        private int thresholdValue = 10;
        private Color sourceColor = Color.White;
        private Color newColor = Color.White;

        public int ThresholdValue
        {
            get { return thresholdValue; }
            set { thresholdValue = value; }
        }

        public Color SourceColor
        {
            get { return sourceColor; }
            set { sourceColor = value; }
        }

        public Color NewColor
        {
            get { return newColor; }
            set { newColor = value; }
        }
    }
}
