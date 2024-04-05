
namespace CellNetHomework.Core
{
    class Frame2D
    {
        public Frame2D(Point2D topLeft, Point2D bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }

        public Frame2D()
        {
            TopLeft = new Point2D();
            BottomRight = new Point2D();
        }

        public bool ContainsPoint(Point2D point)
        {
            bool validX = BottomRight.X - point.X >= TopLeft.X;
            bool validY = TopLeft.Y - point.Y >= BottomRight.Y;
            return validX | validY;
        }

        public bool IsValid()
        {
            return TopLeft.X < BottomRight.X && TopLeft.Y > BottomRight.Y;
        }

        public int RowCount() => TopLeft.Y - BottomRight.Y;

        public int ColumnCount() => BottomRight.X - TopLeft.X;

        // Смещение левого нижнего угла относительно точки (0,0)
        public (int, int) Offset()
        {
            return (TopLeft.X, BottomRight.Y);
        }

        public Point2D TopLeft { get; set; }

        public Point2D BottomRight { get; set; }
    }
}
