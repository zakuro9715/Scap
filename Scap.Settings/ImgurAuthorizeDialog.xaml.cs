using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Scap.Core;


namespace Scap.Settings
{
  /// <summary>
  /// ImgurAuthorizeDialog.xaml の相互作用ロジック
  /// </summary>
  public partial class ImgurAuthorizeDialog : Window
  {
    private Scap.Core.Settings settings;
      public ImgurAuthorizeDialog(Scap.Core.Settings settings)
    {
      InitializeComponent();
      this.settings = settings;
      Imgur.OpenAuthWindow();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
      await Imgur.Authorize(PinCodeInput.Text, settings);
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
