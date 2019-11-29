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

            string json = File.ReadAllText("document.json");
            InputInfo input = JsonConvert.DeserializeObject<InputInfo>(json);

            //InputInfo input = new InputInfo();

            Director director = new Director(input, this.Canvasss);
            director.StartDraw();


            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(Canvasss, "Визитная карточка");
            }
        }
    }
}
