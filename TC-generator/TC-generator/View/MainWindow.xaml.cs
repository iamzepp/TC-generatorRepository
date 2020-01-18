using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using TC_generator.Printer;

namespace TC_generator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool flag = true;
        Director director;

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        public void Start()
        {
            //Director director = new Director((new ManagerInput(new DefaultInput())).GetInput(), this.Canvasss);
            director = new Director((new ManagerInput(new ExcelIntut())).GetInput(), Canvasss);
            director.StartDraw();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            IPrint printer = new CanvasPrinter();
            printer.Print(Canvasss);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                (sender as MenuItem).Header = "Показать стадии";
                flag = false;

                List<Label> list = new List<Label>();

                int i = 0;
                foreach (var c in Canvasss.ItemContainerGenerator.Items)
                {
                    if (c is Label)
                        if ((c as Label).Uid == "Study_label" + i)
                        {
                            list.Add((Label)c);
                            i++;
                        }
                }

                foreach (var c in list)
                    c.Visibility = Visibility.Hidden;
            }
            else
            {
                (sender as MenuItem).Header = "Скрыть стадии";
                flag = true;

                List<Label> list = new List<Label>();

                int i = 0;
                foreach (var c in Canvasss.ItemContainerGenerator.Items)
                {
                    if (c is Label)
                        if ((c as Label).Uid == "Study_label" + i)
                        {
                            list.Add((Label)c);
                            i++;
                        }
                }

                foreach (var c in list)
                    c.Visibility = Visibility.Visible;
            }

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            IPrint printer = new ScrinPrinter();
            printer.Print(director);
            Canvasss.Items.Clear();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
