using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;


namespace EasyCapture.Settings
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private class Settings : INotifyPropertyChanged
    {
      public event PropertyChangedEventHandler PropertyChanged;
      private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
      {
        if (PropertyChanged != null)
        {
          PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
      }
      private string screenshotDir;
      public string ScreenshotDir
      {
        get { return  screenshotDir; }
        set
        {
          if (screenshotDir != value)
          {
            screenshotDir = value;
            NotifyPropertyChanged();
          }
        }
      }

      private bool openExplorer;
      public bool OpenExplorer
      {
        get { return openExplorer; }
        set
        {
          if (openExplorer != value)
          {
            openExplorer = value;
            NotifyPropertyChanged();
          }
        }
      }
    }

    private Settings settings = new Settings();
    public MainWindow()
    {
      InitializeComponent();
      DataContext = new { Settings = settings };
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      settings.ScreenshotDir = "dir"; 
    }
  }
}
