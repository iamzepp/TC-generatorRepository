using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TC_generator.Model.FlowObjects
{
    public class Study
    {
        public Point BeginPoint { get; set; }
        public Point EndPoint
        {
            get
            {
                return CalculateEndPoint();
            }
        }

        public static double Lenght { get; set; } = 100;

        public Study(Point BeginPoint)
        {
            this.BeginPoint = BeginPoint;
        }

        private Point CalculateEndPoint()
        {
            Point p = new Point
            {
                X = BeginPoint.X + Lenght,
                Y = BeginPoint.Y 
            };

            return p;
        }

    }
}
