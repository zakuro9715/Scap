using System;
using System.Collections.Generic;
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
  /// Overlay.xaml の相互作用ロジック
  /// </summary>
  public partial class Overlay : Window
  {
    public Overlay()
    {
      InitializeComponent();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if(e.Key == Key.Escape)
      {
        this.Close();
      }
    }
  }
}
