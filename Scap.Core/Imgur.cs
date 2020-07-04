using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.Cache;
using System.Net.Http;
using System.Text.Json;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace Scap.Core
{
  public static class Imgur
  {
    public static void OpenAuthWindow()
    {
      var url = string.Format(
        Config.Imgur["authorize_url_formt"],
        Config.Imgur["authorize_url_format"],
        Config.Imgur["clientId"]
      ).Replace("&", "^&");
      Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));
    }

    public static async Task Authorize(string pin, Settings settings)
    {
      var request = new HttpRequestMessage(HttpMethod.Post, Config.Imgur["oauth_token_url"]);
      request.Content = new FormUrlEncodedContent(new Dictionary<string, string>() {
        { "client_id", Config.Imgur["client_id"] },
        { "client_secret" ,Config.Imgur["imgur.client_secret"] },
        { "pin", pin },
        { "grant_type", "pin" }
      });
      using (var client = new HttpClient())
      {
        var response = await client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
        dynamic obj = JsonSerializer.Deserialize<ExpandoObject>(body);
        settings.ImgurToken = obj.access_token.GetString();
        settings.ImgurUsername = obj.account_username.GetString();
        settings.ImgurRefreshToken = obj.refresh_token.GetString();
        settings.ImgurExpiresIn = obj.expires_in.GetInt32();
      }
    }


    public static void SetAuthorizationHeader(HttpRequestMessage request, Settings settings)
    {
      request.Headers.Add(
        "Authorization",
        settings.ImgurLoggedIn
          ? $"Bearer {settings.ImgurToken}"
          : $"Client-ID {Config.Imgur["client_id"]}");
    }



    public static async Task<string> Upload(string base64Image, Settings settings)
    {
      var request = new HttpRequestMessage(HttpMethod.Post, Config.Imgur["upload_url"]);
      request.Content = new FormUrlEncodedContent(new Dictionary<string, string>() {
        { "image", base64Image },
        { "type", "base64" },
      });
      SetAuthorizationHeader(request, settings);

      using (var client = new HttpClient())
      {
        var response = await client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
        dynamic obj = JsonSerializer.Deserialize<ExpandoObject>(body);
        return obj.data.GetProperty("link").GetString();
      }
    }
  }
}
