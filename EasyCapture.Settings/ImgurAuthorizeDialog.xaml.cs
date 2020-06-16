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
using EasyCapture.Common;


namespace EasyCapture.Settings
{
  /// <summary>
  /// ImgurAuthorizeDialog.xaml の相互作用ロジック
  /// </summary>
  public partial class ImgurAuthorizeDialog : Window
  {
    public struct AuthResult
    {
      public readonly string Token;
      public readonly string Username;

      public AuthResult(string token, string username)
      {
        this.Token = token;
        this.Username = username;
      }
    }
    private EasyCapture.Common.Settings settings;
      public ImgurAuthorizeDialog(EasyCapture.Common.Settings settings)
    {
      InitializeComponent();
      this.settings = settings;
      Imgur.OpenAuthWindow();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
      Imgur.Authorize(PinCodeInput.Text, settings);
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
