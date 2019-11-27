using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TC_generator.Model.Objects;

namespace TC_generator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ColdFlow flow1 = new ColdFlow(20, new Point(100, 100));

            for (int i = 0; i < flow1.Lines.Count; i++)
            {
                Canvasss.Children.Add(flow1.Lines[i]);
            }


            ColdFlow flow2 = new ColdFlow(22, new Point(100, 150));

            for (int i = 0; i < flow2.Lines.Count; i++)
            {
                Canvasss.Children.Add(flow2.Lines[i]);
            }


            ColdFlow flow3 = new ColdFlow(4, new Point(100, 200));

            for (int i = 0; i < flow3.Lines.Count; i++)
            {
                Canvasss.Children.Add(flow3.Lines[i]);
            }


            ColdFlow flow4 = new ColdFlow(9, new Point(100, 250));

            for (int i = 0; i < flow4.Lines.Count; i++)
            {
                Canvasss.Children.Add(flow4.Lines[i]);
            }

            HotFlow flow5 = new HotFlow(15, new Point(100, 300));

            for (int i = 0; i < flow5.Lines.Count; i++)
            {
                Canvasss.Children.Add(flow5.Lines[i]);
            }

            HotFlow flow6 = new HotFlow(9, new Point(100, 350));

            for (int i = 0; i < flow6.Lines.Count; i++)
            {
                Canvasss.Children.Add(flow6.Lines[i]);
            }


            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(Canvasss, "Визитная карточка");
            }
        }
    }
}
