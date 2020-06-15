using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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
  public partial class Overlay : Window
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
      private double top;
      public double Top {
        get { return top; }
        set {
          if (top != value) {
            top = value;
            NotifyPropertyChanged();
          }
        }
      }
      private double left;
      public double Left
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
      private double width;
      public double Width
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
      private double height;
      public double Height
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

    public Overlay()
    {
      InitializeComponent();
      this.DataContext = new { SelectionBox = this.selectionBox };
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
      selectionBox.Left = pos.X;
      selectionBox.Top = pos.Y;

    }

    private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      selectionBox.Visibility = Visibility.Hidden;
    }

    private void Window_MouseMove(object sender, MouseEventArgs e)
    {
      var pos =e.GetPosition(this);
      selectionBox.Width = pos.X - selectionBox.Left;
      selectionBox.Height = pos.Y - selectionBox.Top;
    }
  }
}
