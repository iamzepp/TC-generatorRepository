using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TC_generator.Model.FlowObjects;

namespace TC_generator.Model.Objects
{
    public abstract class EnergyFlowBase
    {
        public int IdEnergyFlow { get; } = 0;

        public string Name { get; }

        public int StudyCount { get; set; }

        public Point beginP { get; set; }

        public EnergyFlowBase(int StudyCount, Point beginP)
        {
            IdEnergyFlow++;
            Name = "FLOW №" + IdEnergyFlow.ToString();

            this.StudyCount = StudyCount;
            this.beginP = beginP;
        }

        public abstract Dictionary<int, Study> CreateStudyes(int StudyCount);
        
    }
}
