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
using TC_generator.Model.InputObjects;
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


            InputInfo input = new InputInfo();

            ColdFlow flow1 = new ColdFlow(input.StudyCount, new Point(600, 100),input.CF_Tn[0], input.CF_Tk[0]);

            for (int i = 0; i < flow1.Lines.Count; i++)
            {
                Canvasss.Items.Add(flow1.Lines[i]);
            }

            for (int i = 0; i < flow1.ArrowLines.Length; i++)
            {
                Canvasss.Items.Add(flow1.ArrowLines[i]);
            }

            Label Tn_L1 = new Label();
            Tn_L1.Margin = new Thickness(flow1.TnPoint.X, flow1.TnPoint.Y, 0, 0);
            Tn_L1.Content = flow1.Tn.ToString();
            Canvasss.Items.Add(Tn_L1);

            Label Tk_L1 = new Label();
            Tk_L1.Margin = new Thickness(flow1.TkPoint.X, flow1.TkPoint.Y, 0, 0);
            Tk_L1.Content = flow1.Tk.ToString();
            Canvasss.Items.Add(Tk_L1);





            //PrintDialog dialog = new PrintDialog();
            //if (dialog.ShowDialog() == true)
            //{
            //    dialog.PrintVisual(Canvasss, "Визитная карточка");
            //}
        }
    }
}
