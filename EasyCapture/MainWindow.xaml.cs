using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace EasyCapture
{

  /// <summary>
  /// Overlay.xaml の相互作用ロジック
  /// </summary>
  public partial class MainWindow : Window
  {
    private class SelectionBox : INotifyPropertyChanged
    {
      public event PropertyChangedEventHandler PropertyChanged;
      private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
      {
        if (PropertyChanged != null)
        {
          PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
      }
      private int top;
      public int Top {
        get { return top; }
        set {
          if (top != value) {
            top = value;
            NotifyPropertyChanged();
          }
        }
      }
      private int left;
      public int Left
      {
        get { return left; }
        set
        {
          if (left != value)
          {
            left = value;
            NotifyPropertyChanged();
          }
        }
      }
      private int width;
      public int Width
      {
        get { return width; }
        set
        {
          if (width != value)
          {
            width = value;
            NotifyPropertyChanged();
          }
        }
      }
      private int height;
      public int Height
      {
        get { return height; }
        set
        {
          if (height != value)
          {
            height = value;
            NotifyPropertyChanged();
          }
        }
      }

      private Visibility visibility = Visibility.Hidden;
      public Visibility Visibility
      {
        get { return visibility; }
        set
        {
          if (visibility != value)
          {
            visibility = value;
            NotifyPropertyChanged();
          }
        }
      }
    }

    private bool selecting {
      get {
        return selectionBox.Visibility == Visibility.Hidden;
      }
    }

    private readonly SelectionBox selectionBox = new SelectionBox();
    public MainWindow()
    {
      InitializeComponent();
      this.DataContext = new { SelectionBox = this.selectionBox };
      Mouse.OverrideCursor = Cursors.Cross;
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
      {
        this.Close();
      }
    }

    private void CancelSelect()
    {
      selectionBox.Visibility = Visibility.Hidden;
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      selectionBox.Visibility = Visibility.Visible;
      var pos = e.GetPosition(this);
      selectionBox.Left =(int)pos.X;
      selectionBox.Top = (int)pos.Y;

    }

    private void SaveAndExit()
    {
      try
      {
        var settings = EasyCapture.Common.Settings.Load();

        using (var image = new Bitmap(selectionBox.Width, selectionBox.Height))
        {
          var graphics = Graphics.FromImage(image);
          graphics.CopyFromScreen(
            new System.Drawing.Point(0, 0),
            new System.Drawing.Point(0, 0),
            new System.Drawing.Size(selectionBox.Width, selectionBox.Height)
          );

          Directory.CreateDirectory(settings.ScreenshotDir);
          image.Save(System.IO.Path.Combine(settings.ScreenshotDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")) + ".png");
        }

        if (settings.OpenExplorer)
        {
          Process.Start("EXPLORER.EXE", settings.ScreenshotDir);
        }
        Close();
      }
      catch (Exception e)
      {
        Debug.Crash(Properties.Resources.FaildToSaveImage, e);
      }
    }

    private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      selectionBox.Visibility = Visibility.Hidden;
      SaveAndExit();
    }

    private void Window_MouseMove(object sender, MouseEventArgs e)
    {
      var pos =e.GetPosition(this);
      selectionBox.Width = (int)pos.X - selectionBox.Left;
      selectionBox.Height = (int)pos.Y - selectionBox.Top;
    }

    private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      Close();
    }
  }
}
