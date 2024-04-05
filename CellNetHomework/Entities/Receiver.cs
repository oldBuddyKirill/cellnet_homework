using CellNetHomework.Core;

namespace CellNetHomework.Entities
{
    class Receiver : IPositioned
    {
        public Receiver(Point2D position)
        {
            Position = position;
        }

        public Point2D Position { get; set; }
    }
}
