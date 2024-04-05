using System;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;

namespace CellNetHomework.Entities
{
    static class HeatMapDrawer
    {
        public static void Draw(Matrix<double> data, double min, double max, string filename)
        {
            int height = data.RowCount;
            int width = data.ColumnCount;

            if (height == 0 || width == 0)
                return;

            var heatmap = new Bitmap(width, height);

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    double value = data[y, x];
                    // Нормированное значение
                    double valN;
                    if (double.IsFinite(value))
                    {
                        valN = (value - min) / (max - min);
                    }
                    else
                    {
                        valN = 1;
                    }

                    (int r, int g, int b) = HslToRgb(Convert.ToInt32(240 * (1 - valN)), 100, 50);
                    Color color = Color.FromArgb(255, r, g, b);
                    
                    heatmap.SetPixel(x, height - y - 1, color);
                }
            }

            heatmap.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp);
        }


        public static (int r, int g, int b) HslToRgb(double hIn, double sIn, double lIn)
        {
            double red, green, blue;

            var h = hIn / 360.0;
            var s = sIn / 100.0;
            var l = lIn / 100.0;

            if (Math.Abs(s - 0.0) < double.Epsilon)
            {
                red = l;
                green = l;
                blue = l;
            }
            else
            {
                double var2;

                if (l < 0.5)
                {
                    var2 = l * (1.0 + s);
                }
                else
                {
                    var2 = l + s - s * l;
                }

                var var1 = 2.0 * l - var2;

                red = HueToRgb(var1, var2, h + 1.0 / 3.0);
                green = HueToRgb(var1, var2, h);
                blue = HueToRgb(var1, var2, h - 1.0 / 3.0);
            }

            var nRed = Convert.ToInt32(red * 255.0);
            var nGreen = Convert.ToInt32(green * 255.0);
            var nBlue = Convert.ToInt32(blue * 255.0);

            return (nRed, nGreen, nBlue);
        }

        private static double HueToRgb(
            double v1,
            double v2,
            double vH)
        {
            if (vH < 0.0)
            {
                vH += 1.0;
            }
            if (vH > 1.0)
            {
                vH -= 1.0;
            }
            if (6.0 * vH < 1.0)
            {
                return v1 + (v2 - v1) * 6.0 * vH;
            }
            if (2.0 * vH < 1.0)
            {
                return v2;
            }
            if (3.0 * vH < 2.0)
            {
                return v1 + (v2 - v1) * (2.0 / 3.0 - vH) * 6.0;
            }

            return v1;
        }
    }
}
