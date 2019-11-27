using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
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

        public ColdFlow(int StudyCount, Point beginP) : base(StudyCount, beginP)
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
                Line line = new Line();

                line.X1 = Studyes[i].BeginPoint.X;
                line.Y1 = Studyes[i].BeginPoint.Y;

                line.X2 = Studyes[i].EndPoint.X;
                line.Y2 = Studyes[i].EndPoint.Y;

                line.Stroke = FlowColor;

                Lines.Add(i, line);
            }

            return Lines;
        }

    }
}
