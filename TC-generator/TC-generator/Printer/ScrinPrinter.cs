using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using TC_generator.Model.BuilderObjects;

namespace TC_generator.Printer
{
    public class ScrinPrinter : IPrint
    {
        ItemsControl itemsControl;
        bool IsMove;

        System.Windows.Shapes.Rectangle rc;
        System.Windows.Shapes.Rectangle rcw;

        public void Print(object obj)
        {
            itemsControl = (obj as ItemsControl);
            TakeScreenshot(150, 50 + 25, 2700, 1700);
            Show();
        }

        public void Show()
        {
            //Область выделения линии
            rc = new System.Windows.Shapes.Rectangle();
            rc.Width = 20;
            rc.Height = 20;
            rc.Fill = System.Windows.Media.Brushes.DeepPink;
            rc.Opacity = 0.15;
            rc.Name = "UU";
            rc.Margin = new Thickness(150, 100, 0, 0);
            itemsControl.Items.Add(rc);


            //Область выделения линии
            rcw = new System.Windows.Shapes.Rectangle();
            rcw.Width = 5;
            rcw.Height = 5;
            rcw.Fill = System.Windows.Media.Brushes.Black;
            rcw.Opacity = 1;
            rcw.Name = "II";
            rcw.Margin = new Thickness(150 + 15, 100 +15, 0, 0);
            itemsControl.Items.Add(rcw);

            rcw.MouseDown += new MouseButtonEventHandler(Rectangel_MouseButtonDown);
            rcw.MouseMove += new MouseEventHandler(Rectangel_MouseButtonMove);
            rcw.MouseUp += new MouseButtonEventHandler(Rectangel_MouseButtonUp);

            //rc.MouseEnter += new MouseEventHandler(RectangelBranch_MouseButtonEnter);
            //rc.MouseLeave += new MouseEventHandler(RectangelBranch_MouseButtonLeave);
        }

        private void TakeScreenshot(int StartX, int StartY, int Width, int Height)
        {
            Bitmap Screenshot = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(Screenshot);
            G.CopyFromScreen(StartX, StartY, 0, 0, new System.Drawing.Size(Width, Height), CopyPixelOperation.SourceCopy);

            string fileName = "Temperature sheme.bmp";
            FileStream fs = File.Open(fileName, FileMode.OpenOrCreate);
            Screenshot.Save(fs, ImageFormat.Bmp);
            fs.Close();
        }

        private void Rectangel_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                IsMove = true;
            }
        }

        private void Rectangel_MouseButtonMove(object sender, MouseEventArgs e)
        {
            if (IsMove & e.LeftButton == MouseButtonState.Pressed)
            {

                double X = e.GetPosition(itemsControl).X;
                double Y = e.GetPosition(itemsControl).Y;


                rcw.Margin = new Thickness(X - 5, Y -5, X, Y);
                rc.Margin = new Thickness(rc.Margin.Left, rc.Margin.Top, rcw.Margin.Right, rcw.Margin.Bottom);

            }
        }

        private void Rectangel_MouseButtonUp(object sender, MouseButtonEventArgs e)
        {    
           IsMove = false;
        }

    }
}
