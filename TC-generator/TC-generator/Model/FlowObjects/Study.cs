using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TC_generator.Model.ConnectionObjects;

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
        public FlowType type { get; }

        public Study(Point BeginPoint, FlowType type)
        {
            this.BeginPoint = BeginPoint;
            this.type = type;

          
        }

        private Point CalculateEndPoint()
        {
            double X = default;
            double Y = default;

            switch(type)
            {
                case FlowType.Hot:
                    X = BeginPoint.X + Lenght;
                    Y = BeginPoint.Y;
                        break;

                case FlowType.Cold:
                    X = BeginPoint.X - Lenght;
                    Y = BeginPoint.Y;
                    break;
            }

            return new Point(X, Y);
        }

    }
}
