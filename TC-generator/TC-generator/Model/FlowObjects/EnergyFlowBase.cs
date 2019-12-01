using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using TC_generator.Model.ConnectionObjects;
using TC_generator.Model.FlowObjects;

namespace TC_generator.Model.Objects
{
    public abstract class EnergyFlowBase
    {
        public int IdEnergyFlow { get; } = 0;

        public abstract string Name { get; }

        public int StudyCount { get; set; }

        public Point beginP { get; set; }

        public double Tn { get; set; }

        public double Tk { get; set; }

        public abstract FlowType TYPE { get; }

        public abstract Dictionary<int, Line> Lines { get; }

        public abstract Dictionary<int, Point> IdTextPoint { get; }

        public abstract Point TnPoint { get; }

        public abstract Point TkPoint { get; }

        public abstract Line[] ArrowLines { get; }


        public EnergyFlowBase(int StudyCount, Point beginP, double Tn, double Tk)
        {
            IdEnergyFlow++;

            this.StudyCount = StudyCount;
            this.beginP = beginP;

            this.Tn = Tn;
            this.Tk = Tk;
        }

        public abstract Dictionary<int, Study> CreateStudyes(int StudyCount);
        
    }
}
