using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using TC_generator.Model.BuilderObjects;
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

            Start();
        }

        Line CurLine;
        bool Flag = true;
        Director director;

        public void Start()
        {
            //Director director = new Director((new ManagerInput(new DefaultInput())).GetInput(), this.Canvasss);
            director = new Director((new ManagerInput(new ExcelIntut())).GetInput(), this.Canvasss);
            director.StartDraw();


            //PrintDialog dialog = new PrintDialog();
            //if (dialog.ShowDialog() == true)
            //{
            //    dialog.PrintVisual(Canvasss, "Визитная карточка");
            //}
        }

        List<Point> p = new List<Point>();
        int n = 0;

        private void Canvasss_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (n == 0)
            //{   
            //        List<Line> lines = Canvasss.Items.OfType<Line>().ToList();

            //        foreach (Line l in lines)
            //        {
            //            if (Math.Abs(l.X1 - e.GetPosition(this).X) < 20 && Math.Abs(l.Y1 - e.GetPosition(this).Y) < 20)
            //            {
            //                CurLine = l;
            //                Cursor = Cursors.Hand;
            //                p.Add(new Point(l.X1, l.Y1));
            //                n += 1;
            //            }
            //        }
            //}
            //else if(CurLine!=null)
            //{
            //    p.Add(new Point(e.GetPosition(this).X, e.GetPosition(this).Y));
            //    CurLine.X1 = p[1].X;
            //    CurLine.X2 = p[1].X;
            //    n = 0;
            //    Cursor = Cursors.Arrow;
            //}
        }

        private void Canvasss_MouseMove(object sender, MouseEventArgs e)
        {

            //if (e.LeftButton == MouseButtonState.Pressed &  CurLine != null & director.IsMoved)
            //{
            //    director.CurLine.X1 = e.GetPosition(this).X;
            //    director.CurLine.X2 = e.GetPosition(this).X;
            //}
        }

        private void Canvasss_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //CurLine = null;
            //Flag = true;
            //Cursor = Cursors.Arrow;
            //p = null;
            //n = 0;
        }
    }
}
