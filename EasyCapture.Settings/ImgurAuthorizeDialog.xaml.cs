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
    public AuthResult Result { get; set; }

    public ImgurAuthorizeDialog()
    {
      InitializeComponent();
      var url = Properties.Resources.ImgurAuthorizeUrl.Replace("&", "^&");
      Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
      Result = new AuthResult("dummytoken", "dummmyuser");
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
