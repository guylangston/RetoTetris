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
using SkiaSharp;

namespace Tetris.WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WriteableBitmap buffer;
        int cc = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        // http://lostindetails.com/blog/post/SkiaSharp-with-Wpf
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.buffer = CreateImage((int)this.Width, (int)this.Height);

            fullImage.Source = buffer;

            CompositionTarget.Rendering +=  (o, ee) => this.UpdateImage(this.buffer);
        }

        public WriteableBitmap CreateImage(int width, int height)
        {
            return new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, BitmapPalettes.Halftone256Transparent);
        }

        private bool busy;

        public void UpdateImage(WriteableBitmap writeableBitmap)
        {
            busy = true;

            int width  = (int)writeableBitmap.Width,
                height = (int)writeableBitmap.Height;
            writeableBitmap.Lock();
            using (var surface = SKSurface.Create(
                width: width,
                height: height,
                colorType: SKColorType.Bgra8888,
                alphaType: SKAlphaType.Premul,
                pixels: writeableBitmap.BackBuffer,
                rowBytes: width * 4))
            {
                SKCanvas canvas = surface.Canvas;
                canvas.Clear(new SKColor(130, 130, 130));
                canvas.DrawText($"SkiaSharp on Wpf! {cc++}", 50, 200, new SKPaint() { Color = new SKColor(0, 0, 0), TextSize = 20 });
            }
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            writeableBitmap.Unlock();

            busy = false;

        }
    }
}
