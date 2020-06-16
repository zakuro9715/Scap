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
    private EasyCapture.Common.Settings settings = null;
    public MainWindow()
    {
      InitializeComponent();
      settings = EasyCapture.Common.Settings.Load();
      DataContext = new { Settings = settings };
    }
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      settings.Save();
      Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void ImgurLogin_Click(object sender, RoutedEventArgs e)
    {
      var dialog = new ImgurAuthorizeDialog();
      dialog.ShowDialog();
      if (dialog.DialogResult == true)
      {
        settings.ImgurToken = dialog.Result.Token;
        settings.ImgurUsername = dialog.Result.Username;
      }
    }
    private void ImgurLogout_Click(object sender, RoutedEventArgs e)
    {
      settings.ImgurToken = "";
      settings.ImgurUsername = "";
    }

  }
}
