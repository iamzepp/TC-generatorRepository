using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TC_generator.Model.BuilderObjects;
using TC_generator.Model.FlowObjects;

namespace TC_generator.Model.Objects
{
    public class ColdFlow : EnergyFlowBase
    {
        public int IdColdFlow { get; } = 0;

        public static SolidColorBrush FlowColor { get; set; } = Brushes.Blue;

        public Dictionary<int, Study> Studyes
        {
            get
            {
                return CreateStudyes(StudyCount);
            }
        }

        public Dictionary<int, Line> Lines
        {
            get
            {
                return CreateLines();
            }
        }

        public Dictionary<int, Point> IdTextPoint
        {
            get
            {
                return CreateIdTextPoint();
            }
        }

        public Point TnPoint
        {
            get
            {
                return new Point(beginP.X, beginP.Y - 10);
            }
        }

        public Point TkPoint
        {
            get
            {
                return new Point(Studyes[StudyCount - 1].EndPoint.X, Studyes[StudyCount - 1].EndPoint.Y - 10);
            }
        }

        public Line[] ArrowLines
        {
            get
            {
                return CreateArrow();
            }
        }


        public ColdFlow(int StudyCount, Point beginP, double Tn, double Tk) : base(StudyCount, beginP, Tn, Tk)
        {
            IdColdFlow++;
        }

        public override Dictionary<int, Study> CreateStudyes(int StudyCount)
        {
            Dictionary<int, Study> studyes = new Dictionary<int, Study>();

            Point p = new Point();
            p.X = beginP.X;
            p.Y = beginP.Y;

            for (int i = 0; i < StudyCount; i++)
            {
                studyes.Add(i, new Study(p));
                p.X = p.X + 100;
            }

            return studyes;
        }

        private Dictionary<int, Line> CreateLines()
        {
            Dictionary<int, Line> Lines = new Dictionary<int, Line>();

            for (int i = 0; i < StudyCount; i++)
            {
                Line line = new Line()
                {
                    X1 = Studyes[i].BeginPoint.X,
                    Y1 = Studyes[i].BeginPoint.Y,
                    X2 = Studyes[i].EndPoint.X,
                    Y2 = Studyes[i].EndPoint.Y,
                };

                line.Stroke = FlowColor;

                Lines.Add(i, line);
            }

            return Lines;
        }

        public Dictionary<int, Point> CreateIdTextPoint()
        {
            Dictionary<int, Point> IdTextPoint = new Dictionary<int, Point>();

            for (int i = 0; i < StudyCount; i++)
            {
                Point p = new Point()
                {
                    X = (Studyes[i].BeginPoint.X + Studyes[i].EndPoint.X) / 2,
                    Y = ((Studyes[i].BeginPoint.Y + Studyes[i].EndPoint.Y) / 2) - 10,
                };

                IdTextPoint.Add(i, p);
            }

            return IdTextPoint;
        }

        public Line[] CreateArrow()
        {

            Line line1 = new Line
            {
                X1 = Studyes[StudyCount - 1].EndPoint.X,
                Y1 = Studyes[StudyCount - 1].EndPoint.Y,
                X2 = Studyes[StudyCount - 1].EndPoint.X + 10,
                Y2 = Studyes[StudyCount - 1].EndPoint.Y - 10
            };

            Line line2 = new Line
            {
                X1 = Studyes[StudyCount - 1].EndPoint.X,
                Y1 = Studyes[StudyCount - 1].EndPoint.Y,
                X2 = Studyes[StudyCount - 1].EndPoint.X + 10,
                Y2 = Studyes[StudyCount - 1].EndPoint.Y + 10
            };

            line1.Stroke = FlowColor;
            line2.Stroke = FlowColor;

            return new Line[2] { line1, line2 };
        }

    }
}
