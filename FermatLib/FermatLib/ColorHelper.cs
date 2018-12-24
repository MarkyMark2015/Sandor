using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FermatLib
{
    // https://geekymonkey.com/Programming/CSharp/RGB2HSL_HSL2RGB.htm
    class ColorHelper
    {
        const int ColorCount = 1000;
        private List<ColorRGB> _rainbowColors = new List<ColorRGB>();

        public ColorHelper()
        {
            // Colors of the Rainbow
            for (double i = 0; i < 1; i += 1.0/ ColorCount)
            {
                ColorRGB c = HSL2RGB(i, 0.5, 0.5);
                _rainbowColors.Add(c);
            }
        }

        private double _min;
        private double _max;
        public ColorRGB GetColor(double result)
        {
            // Kleinsten und grössten Wert
            _min = (result < _min ? result : _min);
            _max = (result > _max ? result : _max);
            // empririsch bestimmt: ca-Wertebereich
            const double resultMin = -1.8414708881161;
            const double resultMax = 0.30116783642987;
            const double delta = resultMax - resultMin;
            double dc = delta / _rainbowColors.Count;

            // Farbindex
            int ndx = (int) ((result-resultMin) / delta * _rainbowColors.Count);
            //int ndx = (int) (Math.Log(result-resultMin) / Math.Log(delta) * _rainbowColors.Count);
            //int ndx = (int) ((result - resultMin)*(result - resultMin) / delta /delta * _rainbowColors.Count);
            if (ndx > _rainbowColors.Count-1) ndx = _rainbowColors.Count - 1;
            if (ndx < 0) ndx = 0;

            return _rainbowColors[ndx];
        }

        public void PrintMinMax()
        {
            Debug.WriteLine($"*** Min={_min}, Max={_max}");
        }

        public struct ColorRGB

        {
            public byte R;
            public byte G;
            public byte B;
            public ColorRGB(Color value)
            {
                this.R = value.R;
                this.G = value.G;
                this.B = value.B;
            }

            public static implicit operator Color(ColorRGB rgb)
            {
                Color c = Color.FromArgb(rgb.R, rgb.G, rgb.B);
                return c;
            }
            public static explicit operator ColorRGB(Color c)
            {
                return new ColorRGB(c);
            }
        }

        public static ColorRGB HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            ColorRGB rgb;
            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);
            return rgb;
        }
    }
}
