using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.Cache;
using System.Net.Http;
using System.Text.Json;
using System.Dynamic;

namespace EasyCapture.Settings
{
  public static class Imgur
  {
    public static void OpenAuthWindow()
    {
      var url = string.Format(
        Properties.Resources.ImgurAuthorizeUrlFormat,
        Properties.Resources.ImgurClientId
      ).Replace("&", "^&");
      Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
    }

    public static string Authorize(string pin, EasyCapture.Common.Settings settings)
    {
      var content = new FormUrlEncodedContent(new Dictionary<string, string>() {
        { "client_id", Properties.Resources.ImgurClientId },
        { "client_secret", Properties.Resources.ImgurClientSecret },
        { "pin", pin },
        { "grant_type", "pin" }
      });
      new HttpClient().PostAsync(Properties.Resources.ImgurOauthTokenUrl, content)
        .ContinueWith((task) => task.Result.Content.ReadAsStringAsync())
        .ContinueWith((task) =>
        {
          var body = task.Result.Result;
          dynamic obj = JsonSerializer.Deserialize<ExpandoObject>(body);
          settings.ImgurToken = obj.access_token.GetString();
          settings.ImgurUsername = obj.account_username.GetString();
          settings.ImgurRefreshToken = obj.refresh_token.GetString();
          settings.ImgurExpiresIn = obj.expires_in.GetInt32();

          Console.WriteLine();
        });
      return "";
    }
  }
}
