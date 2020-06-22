using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyCapture
{
  /// <summary>
  /// PintWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class PaintDialog : Window
  {
    public BitmapEncoder Encoder { get; private set; }
    public PaintDialog(Bitmap originalImage)
    {
      InitializeComponent();
      canvas.Height = originalImage.Height;
      canvas.Width = originalImage.Width;
      using (var ms = new MemoryStream())
      {
        originalImage.Save(ms, ImageFormat.Bmp);
        ms.Position = 0;

        var brush = new ImageBrush();
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.CreateOptions = BitmapCreateOptions.None;
        image.StreamSource = ms;
        image.EndInit();
        image.Freeze();
        brush.ImageSource = image;
        canvas.Background = brush;
      }
    }

    private void WriteImageToSTream()
    {
      var drawingVisual = new DrawingVisual();
      var drawingContext = drawingVisual.RenderOpen();

      var rect = new Rect(0, 0, canvas.ActualWidth, canvas.ActualHeight);
      drawingContext.DrawRectangle(canvas.Background, null, rect);
      canvas.Strokes.Draw(drawingContext);
      drawingContext.Close();

      var target = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Default);
      target.Render(drawingVisual);

      Encoder = new PngBitmapEncoder();
      Encoder.Frames.Add(BitmapFrame.Create(target));
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      WriteImageToSTream();
      DialogResult = true;
      Close();
    }
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Close();
    }

  }
}
