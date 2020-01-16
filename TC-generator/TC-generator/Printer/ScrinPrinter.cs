using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TC_generator.Model.BuilderObjects;

namespace TC_generator.Printer
{
    public class ScrinPrinter : IPrint
    {
        public void Print(object obj)
        {
            Director dir = (obj as Director);
            TakeScreenshot(150, 50 + 25, 300 * dir.input.StudyCount + 150, dir.Y + 50);
        }

        private void TakeScreenshot(int StartX, int StartY, int Width, int Height)
        {
            // Bitmap in right size
            Bitmap Screenshot = new Bitmap(Width, Height);
            Graphics G = Graphics.FromImage(Screenshot);
            // snip wanted area
            G.CopyFromScreen(StartX, StartY, 0, 0, new System.Drawing.Size(Width, Height), CopyPixelOperation.SourceCopy);

            // save uncompressed bitmap to disk
            string fileName = "TestBMP.bmp";
            FileStream fs = File.Open(fileName, FileMode.OpenOrCreate);
            Screenshot.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
            fs.Close();
        }

    }
}
