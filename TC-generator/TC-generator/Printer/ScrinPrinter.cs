using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TC_generator.Model.BuilderObjects;

namespace TC_generator.Printer
{
    public class ScrinPrinter : IPrint
    {
        public void Print(object obj)
        {
            Director dir = (obj as Director);
            TakeScreenshot(150, 50 + 25, 2700, 1700);
        }

        private void TakeScreenshot(int StartX, int StartY, int Width, int Height)
        {
            Bitmap Screenshot = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(Screenshot);
            G.CopyFromScreen(StartX, StartY, 0, 0, new Size(Width, Height), CopyPixelOperation.SourceCopy);

            string fileName = "Temperature sheme.bmp";
            FileStream fs = File.Open(fileName, FileMode.OpenOrCreate);
            Screenshot.Save(fs, ImageFormat.Bmp);
            fs.Close();
        }

    }
}
