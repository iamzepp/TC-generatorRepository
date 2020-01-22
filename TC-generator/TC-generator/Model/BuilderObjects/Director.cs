using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TC_generator.Model.ConnectionObjects;
using TC_generator.Model.InputObjects;
using TC_generator.Model.Objects;

namespace TC_generator.Model.BuilderObjects
{


    public class Director
    {
        public IChartBilder chartBilder;

        public Director(IChartBilder chartBilder)
        {
            this.chartBilder = chartBilder;
        }

        public void Draw()
        {
            chartBilder.Bild();
        }

    }
}
