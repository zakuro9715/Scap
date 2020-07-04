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
using Scap.Core;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.Windows.Interop;

namespace Scap.App
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

    public System.Drawing.Rectangle Rectangle { get; set; }

    private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      this.Visibility = Visibility.Hidden;
      selectingRectangle.Fill= new SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
      selectionBox.Visibility = Visibility.Hidden;
      if (selectionBox.Width == 0 || selectionBox.Height == 0)
      {
        // Capture full screen when just click not drag and drop.
        var bounds = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle).Bounds;
        Rectangle = new System.Drawing.Rectangle(0, 0, bounds.Width, bounds.Height);
      } else {
        Rectangle = new System.Drawing.Rectangle(selectionBox.Left, selectionBox.Top, selectionBox.Width, selectionBox.Height);
      }

      DialogResult = true;
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
