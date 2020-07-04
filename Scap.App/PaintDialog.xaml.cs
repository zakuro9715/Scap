using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Scap.App
{
  /// <summary>
  /// PintWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class PaintDialog : Window
  {
    public BitmapEncoder Encoder { get; private set; }
    private Bitmap image;

    private enum ToolKind
    {
      Select,
      Paint,
    }

    private ToolKind tool;
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
    private void EnterSelectMode()
    {
      tool = ToolKind.Select;
      canvas.Cursor = Cursors.Arrow;
      canvas.EditingMode = InkCanvasEditingMode.None;
    }

    private void EnterPaintMode()
    {
      tool = ToolKind.Paint;
      canvas.Cursor = Cursors.Pen;
      canvas.EditingMode = InkCanvasEditingMode.Ink;
    }

    private void PaintTool_Selected(object sender, RoutedEventArgs e)
    {
      EnterPaintMode();
    }

    private void SelectTool_Selected(object sender, RoutedEventArgs e)
    {
      EnterSelectMode();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      toolBox.SelectedIndex = 0;
    }

    private void SetColor(System.Windows.Media.Color color)
    {
      canvas.DefaultDrawingAttributes.Color = color;
    }

    private static System.Windows.Media.Color GetColorByOffset(GradientStopCollection collection, double offset)
    {
      GradientStop[] stops = collection.OrderBy(x => x.Offset).ToArray();
      if (offset <= 0) return stops[0].Color;
      if (offset >= 1) return stops[stops.Length - 1].Color;
      GradientStop left = stops[0], right = null;
      foreach (GradientStop stop in stops)
      {
        if (stop.Offset >= offset)
        {
          right = stop;
          break;
        }
        left = stop;
      }
      offset = Math.Round((offset - left.Offset) / (right.Offset - left.Offset), 2);
      byte a = (byte)((right.Color.A - left.Color.A) * offset + left.Color.A);
      byte r = (byte)((right.Color.R - left.Color.R) * offset + left.Color.R);
      byte g = (byte)((right.Color.G - left.Color.G) * offset + left.Color.G);
      byte b = (byte)((right.Color.B - left.Color.B) * offset + left.Color.B);
      return System.Windows.Media.Color.FromArgb(a, r, g, b);
    }

    private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
      var slider = (sender as Slider);
      var brush = (slider.Background as LinearGradientBrush);
      SetColor(GetColorByOffset(brush.GradientStops, slider.Value));    }
  }
}
