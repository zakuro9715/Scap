using System;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Drawing.Imaging;
using EasyCapture.Common;
using System.Threading.Tasks;
using System.Security.Permissions;

namespace EasyCapture
{
  using Point = System.Drawing.Point;

  /// <summary>
  /// Overlay.xaml の相互作用ロジック
  /// </summary>
  public partial class CaptureDialog : Window
  {
    private class SelectionBox : BindingData
    {
      public Point corner;

      private int left;
      public int Left
      {
        get => left;
        private set => SetField(ref left, value);
      }

      private int top;
      public int Top
      {
        get => top;
        private set => SetField(ref top, value);
      }

      private int width;
      public int Width
      {
        get => width;
        private set => SetField(ref width, value);
      }

      private int height;
      public int Height
      {
        get => height;
        private set => SetField(ref height, value);
      }

      private Visibility visibility = Visibility.Hidden;
      public Visibility Visibility
      {
        get => visibility;
        set => SetField(ref visibility, value);
      }

      public void UpdateBox(Point otherCorner)
      {
        Left = Math.Min(corner.X, otherCorner.X);
        Top = Math.Min(corner.Y, otherCorner.Y);
        Width = Math.Abs(corner.X - otherCorner.X);
        Height = Math.Abs(corner.Y - otherCorner.Y);
      }

      public void UpdateBox(System.Windows.Point otherCorner)
        => UpdateBox(new Point((int)otherCorner.X, (int)otherCorner.Y));
    }

    private bool selecting {
      get {
        return selectionBox.Visibility == Visibility.Hidden;
      }
    }

    private readonly SelectionBox selectionBox = new SelectionBox();
    private readonly Settings settings;
    public CaptureDialog(Settings settings)
    {
      InitializeComponent();
      this.DataContext = new { SelectionBox = this.selectionBox };

      this.settings = settings;
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        Cancel();
      }
    }

    private void Cancel()
    {
      selectionBox.Visibility = Visibility.Hidden;
      DialogResult = false;
      Close();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      var pos = e.GetPosition(this);
      selectionBox.corner = new Point((int)pos.X, (int)pos.Y);
      selectionBox.Visibility = Visibility.Visible;
    }

    private void SaveImage(Bitmap image, Settings settings)
    {
      Directory.CreateDirectory(settings.ScreenshotDir);
      image.Save(System.IO.Path.Combine(settings.ScreenshotDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")) + ".png");
    }

    public Bitmap Image { get; private set; }
    private void Capture()
    {
        Image = new Bitmap(selectionBox.Width, selectionBox.Height);
        var graphics = Graphics.FromImage(Image);
        graphics.CopyFromScreen(
          new System.Drawing.Point(selectionBox.Left, selectionBox.Top),
          new System.Drawing.Point(0, 0),
          new System.Drawing.Size(selectionBox.Width, selectionBox.Height)
        );
        DialogResult = true;
        Close();
    }

    private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      selectionBox.Visibility = Visibility.Hidden;
      Capture();
    }

    private void Window_MouseMove(object sender, MouseEventArgs e)
    {
      var pos = e.GetPosition(this);
      selectionBox.UpdateBox(pos);
    }

    private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      Cancel();
    }
  }
}
