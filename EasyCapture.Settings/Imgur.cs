using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace EasyCapture.Settings
{
  public static class Imgur
  {
    public static void OpenAuthWindow()
    {
      var url = Properties.Resources.ImgurAuthorizeUrl.Replace("&", "^&");
      Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
    }
  }
}
