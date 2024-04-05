using System;
using System.Collections.Generic;
using CellNetHomework.Core;
using CellNetHomework.Entities;

namespace CellNetHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            var receiverPositions = new List<Point2D>() {
                new Point2D(100, 113),
                new Point2D(600, 0),
                new Point2D(-500, -216),
            };

            var receivers = new List<IPositioned>();
            foreach (var position in receiverPositions)
            {
                receivers.Add(new Receiver(position));
            }

            var frame = new Frame2D(new Point2D(-1000, 1000), new Point2D(1000, -1000));

            var matrix = new HdopMatrix
            {
                Frame = frame,
                Receivers = receivers
            };

            string filename = $"HDOP_{receiverPositions.Count}_receivers.bmp";
            (var heatMap, double min, double max) = matrix.CalculateHdopMatrix();
            HeatMapDrawer.Draw(heatMap, min, max, filename);
            Console.WriteLine($"File \"{filename}\" was successfully saved!");
        }
    }
}
