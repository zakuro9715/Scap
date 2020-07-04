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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;


namespace Scap.Settings
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private Scap.Core.Settings settings = null;
    public MainWindow()
    {
      InitializeComponent();
      settings = Scap.Core.Settings.Load();
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
      var dialog = new ImgurAuthorizeDialog(settings);
      dialog.ShowDialog();
    }
    private void ImgurLogout_Click(object sender, RoutedEventArgs e)
    {
      settings.ImgurToken = "";
      settings.ImgurRefreshToken = "";
      settings.ImgurUsername = "";
    }
    private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
    {
      var dialog = new FolderBrowserDialog();
      dialog.SelectedPath = settings.ScreenshotDir;
      var result = dialog.ShowDialog();
      if (result == System.Windows.Forms.DialogResult.OK)
      {
        settings.ScreenshotDir = dialog.SelectedPath;
      }
    }
  }
}
