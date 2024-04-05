using System;

namespace CellNetHomework.Core
{
    class Point2D
    {
        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point2D()
        {
            X = 0;
            Y = 0;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public double DistanceTo(Point2D other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }
    }
}
