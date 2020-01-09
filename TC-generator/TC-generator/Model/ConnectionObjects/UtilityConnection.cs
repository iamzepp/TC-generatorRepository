using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TC_generator.Model.ConnectionObjects
{
    public enum FlowType
    {
        Hot,
        Cold
    }

    public class UtilityConnection
    {
        public int IdUtilityConnection { get; } = 0;
        public string Name { get; }

        public Point InstallationPoint { get; set; }
        public FlowType Type { get; set; }

        public Line[] UtilityLines
        {
            get
            {
                return CreateUtilityConnection();
            }
        }

        public UtilityConnection(Point InstallationPoint, FlowType Type)
        {
            IdUtilityConnection++;
            Name = "Utility Connection №" + IdUtilityConnection.ToString();

            this.InstallationPoint = InstallationPoint;
            this.Type = Type;
        }

        private Line[] CreateUtilityConnection()
        {
            Line[] lines = null;

            switch (Type)
            {
                case FlowType.Cold:

                    Line GeneralLineC = new Line
                    {
                        X1 = InstallationPoint.X,
                        Y1 = InstallationPoint.Y - 20,
                        X2 = InstallationPoint.X,
                        Y2 = InstallationPoint.Y + 20,

                        Stroke = Brushes.Red
                    };

                    Line ArrowLine_1_C = new Line
                    {
                        X1 = GeneralLineC.X1,
                        Y1 = GeneralLineC.Y1,
                        X2 = GeneralLineC.X1 - 5,
                        Y2 = GeneralLineC.Y1 + 5,

                        Stroke = Brushes.Red
                    };

                    Line ArrowLine_2_C = new Line
                    {
                        X1 = GeneralLineC.X1,
                        Y1 = GeneralLineC.Y1,
                        X2 = GeneralLineC.X1 + 5,
                        Y2 = GeneralLineC.Y1 + 5,

                        Stroke = Brushes.Red
                    };

                    lines = new Line[3] { GeneralLineC, ArrowLine_1_C, ArrowLine_2_C };
                    break;

                case FlowType.Hot:

                    Line GeneralLineH = new Line
                    {
                        X1 = InstallationPoint.X,
                        Y1 = InstallationPoint.Y - 20,
                        X2 = InstallationPoint.X,
                        Y2 = InstallationPoint.Y + 20,

                        Stroke = Brushes.Blue
                    };

                    Line ArrowLine_1_H = new Line
                    {
                        X1 = GeneralLineH.X2,
                        Y1 = GeneralLineH.Y2,
                        X2 = GeneralLineH.X2 - 5,
                        Y2 = GeneralLineH.Y2 - 5,

                        Stroke = Brushes.Blue
                    };

                    Line ArrowLine_2_H = new Line
                    {
                        X1 = GeneralLineH.X2,
                        Y1 = GeneralLineH.Y2,
                        X2 = GeneralLineH.X2 + 5,
                        Y2 = GeneralLineH.Y2 - 5,

                        Stroke = Brushes.Blue
                    };

                    lines = new Line[3] { GeneralLineH, ArrowLine_1_H, ArrowLine_2_H };
                    break;
            }

            return lines;
        }

    }
}
