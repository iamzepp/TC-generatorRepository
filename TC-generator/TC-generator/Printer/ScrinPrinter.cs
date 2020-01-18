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
            TakeScreenshot(150, 50 + 25, 3800, 1000);

            //RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(1800, 1000, 96, 96, PixelFormats.Pbgra32);
            //renderTargetBitmap.Render(dir.Canvasss);
            //PngBitmapEncoder pngImage = new PngBitmapEncoder();
            //pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            //FileStream fs = File.Open("www", FileMode.OpenOrCreate);
            //pngImage.Save(fs);
            //fs.Close();

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
            Screenshot.Save(fs, ImageFormat.Bmp);
            fs.Close();
        }

    }
}
