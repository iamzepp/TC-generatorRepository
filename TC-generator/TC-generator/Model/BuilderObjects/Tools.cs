using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TC_generator.Model.BuilderObjects
{
    public static class Tools
    {
        //Минимальные значения величин
        public static double MinQ = 0.01;
        public static double MinF = 0.01;

        //Еденицы измерения
        public static string UnitOfQ = "kW";
        public static string UnitOfF = "m2";

        public static int Round = 2;

        public static double RectangelBranchWidth = 20;

        //Цвета
        public static SolidColorBrush RectangelBranchColor = Brushes.YellowGreen;



        //Поиск элемента по его имени в ItemsControl
        public static T FindUidObjectFromItemsControlUI<T>(string name, string Id, ItemsControl canvas) where T : UIElement
        {
            T obj = default;

            foreach (var c in canvas.ItemContainerGenerator.Items.OfType<T>())
            {
                    if ((c as T).Uid == name + Id)
                    {
                        obj = (T)c;
                        break;
                    }
            }

            return obj;
        }

        //Отсечение велечин, неимеющих физического смысла
        public static bool CheckQandF(double F, double Q)
        {
            if (Q > MinQ && F > MinF)
                return true;
            else
                return false;
        }

    }
}
