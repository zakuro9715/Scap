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
    private Bitmap image;

    public PaintDialog(Bitmap image)
    {
      InitializeComponent();

      this.image = image;
      canvas.Height = image.Height;
      canvas.Width = image.Width;
      using (var ms = new MemoryStream())
      {
        image.Save(ms, ImageFormat.Bmp);
        ms.Position = 0;

        var brush = new ImageBrush();
        var bg = new BitmapImage();
        bg.BeginInit();
        bg.CacheOption = BitmapCacheOption.OnLoad;
        bg.StreamSource = ms;
        bg.EndInit();
        bg.Freeze();
        brush.ImageSource = bg;
        canvas.Background = brush;
      }
    }

    private void WriteImageToSTream()
    {
      var target = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Default);
      target.Render(canvas);

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
