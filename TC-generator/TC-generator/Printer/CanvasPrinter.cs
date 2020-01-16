using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TC_generator.Printer
{
    public class CanvasPrinter : IPrint
    {
        public void Print(object obj)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(obj as Canvas, "Температурная схемы");
            }
        }
    }
}
