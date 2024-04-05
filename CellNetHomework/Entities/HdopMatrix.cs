using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using CellNetHomework.Core;


namespace CellNetHomework.Entities
{
    class HdopMatrix
    {
        public HdopMatrix()
        {

        }

        public Frame2D Frame
        {
            get => frame;
            set
            {
                if (value.IsValid())
                    frame = value;
            }
        }

        public List<IPositioned> Receivers { get; set; }

        // пример, как делать не стоит
        public (Matrix<double>, double min, double max) CalculateHdopMatrix()
        {
            double min = double.NaN;
            double max = double.NaN;

            if (!IsValid())
                throw new InvalidOperationException();

            var result = Matrix<double>.Build.Dense(Frame.RowCount(), Frame.ColumnCount());

            (int offsetX, int offsetY) = Frame.Offset();

            for (int i = 0, endI = Frame.RowCount(); i < endI; ++i)
            {
                for (int j = 0, endJ = Frame.ColumnCount(); j < endJ; ++j)
                {
                    var point = new Point2D(j + offsetX, i + offsetY);
                    var distances = new List<double>();

                    // TODO: либо вынести, либо изменить проверку
                    //bool invalidPosition = false;
                    //foreach (var r in Receivers)
                    //{
                    //    if (point.Y != r.Position.Y)
                    //    {
                    //        distances.Add(point.DistanceTo(r.Position));
                    //    }
                    //    else
                    //    {
                    //        invalidPosition = true;
                    //        break;
                    //    }
                    //}


                    bool containsZeros = false;
                    foreach (var r in Receivers)
                    {
                        double distance = point.DistanceTo(r.Position);
                        if (distance == 0.0)
                        {
                            containsZeros = true;
                            break;
                        }
                        else
                        {
                            distances.Add(distance);
                        }
                    }


                    if (containsZeros)
                    {
                        result[i, j] = double.NaN;
                        continue;
                    }

                    var jacobian = Matrix<double>.Build.Dense(Receivers.Count, 2);
                    for (int ii = 0; ii < jacobian.RowCount; ++ii)
                    {
                        jacobian[ii, 0] = (Receivers[ii].Position.X - point.X) / distances[ii];
                        jacobian[ii, 1] = (Receivers[ii].Position.Y - point.Y) / distances[ii];
                    }
                    var jacobianT = jacobian.Transpose();
                    var covMatrix = jacobianT.Multiply(jacobian).Inverse();
                    var hdop = Math.Clamp(Math.Sqrt(covMatrix.Trace()), 0, 10);
                    if (double.IsFinite(hdop))
                    {
                        if (double.IsNaN(min) || double.IsNaN(max))
                        {
                            max = min = hdop;
                        }
                        else if (hdop > max)
                        {
                            max = hdop;
                        }
                        else if (hdop < min)
                        {
                            min = hdop;
                        }
                    }
                    result[i, j] = hdop;
                }
            }

            return (result, min, max);
        }

        public bool IsValid()
        {
            // TODO: add error messages
            if (!Frame.IsValid())
                return false;

            if (Receivers.Count < 1)
                return false;

            foreach (var r in Receivers)
            {
                if (!Frame.ContainsPoint(r.Position))
                    return false;
            }

            return true;
        }

        private Frame2D frame;
    }
}
