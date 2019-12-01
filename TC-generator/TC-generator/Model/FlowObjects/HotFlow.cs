using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TC_generator.Model.ConnectionObjects;
using TC_generator.Model.FlowObjects;

namespace TC_generator.Model.Objects
{
    public class HotFlow : EnergyFlowBase
    {
        public static int IdHotFlow = 0;

        public static SolidColorBrush FlowColor { get; set; } = Brushes.Red;

        public Dictionary<int, Study> Studyes
        {
            get
            {
                return CreateStudyes(StudyCount);
            }
        }

        public override Dictionary<int, Line> Lines
        {
            get
            {
                return CreateLines();
            }
        }

        public override Dictionary<int, Point> IdTextPoint
        {
            get
            {
                return CreateIdTextPoint();
            }
        }

        public override Point TnPoint
        {
            get
            {
                return new Point(beginP.X, beginP.Y - 40);
            }
        }

        public override Point TkPoint
        {
            get
            {
                return new Point(Studyes[StudyCount - 1].EndPoint.X, Studyes[StudyCount - 1].EndPoint.Y - 40);
            }
        }

        public override Line[] ArrowLines
        {
            get
            {
                return CreateArrow();
            }
        }

        public override string Name { get; }

        public override FlowType TYPE { get => FlowType.Hot; }

        public HotFlow(int StudyCount, Point beginP, double Tn, double Tk) : base(StudyCount, beginP, Tn, Tk)
        {
            IdHotFlow++;
            Name = "H" + IdHotFlow.ToString();
        }

        public override Dictionary<int, Study> CreateStudyes(int StudyCount)
        {
            Dictionary<int, Study> studyes = new Dictionary<int, Study>();

            Point p = new Point
            {
                X = beginP.X,
                Y = beginP.Y
            };

            for (int i = 0; i < StudyCount; i++)
            {
                studyes.Add(i, new Study(p, FlowType.Hot));
                p.X += 100;
            }

            return studyes;
        }

        private Dictionary<int, Line> CreateLines()
        {
            Dictionary<int, Line> Lines = new Dictionary<int, Line>();

            for (int i = 0; i < StudyCount; i++)
            {
                Line line = new Line
                {
                    X1 = Studyes[i].BeginPoint.X,
                    Y1 = Studyes[i].BeginPoint.Y,

                    X2 = Studyes[i].EndPoint.X,
                    Y2 = Studyes[i].EndPoint.Y,

                    Stroke = FlowColor
                };

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
                    Y = ((Studyes[i].BeginPoint.Y + Studyes[i].EndPoint.Y) / 2) - 30,
                };

                IdTextPoint.Add(i, p);
            }

            return IdTextPoint;
        }

        public Line[] CreateArrow()
        {

            Line line1 = new Line
            {
                X1 = Studyes[StudyCount-1].EndPoint.X,
                Y1 = Studyes[StudyCount - 1].EndPoint.Y,
                X2 = Studyes[StudyCount - 1].EndPoint.X - 10,
                Y2 = Studyes[StudyCount - 1].EndPoint.Y + 10
            };

            Line line2 = new Line
            {
                X1 = Studyes[StudyCount - 1].EndPoint.X,
                Y1 = Studyes[StudyCount - 1].EndPoint.Y,
                X2 = Studyes[StudyCount - 1].EndPoint.X - 10,
                Y2 = Studyes[StudyCount - 1].EndPoint.Y - 10
            };

            line1.Stroke = FlowColor;
            line2.Stroke = FlowColor;

            return new Line[2] { line1, line2 };
        }

    }
}
