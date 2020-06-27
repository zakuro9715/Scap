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

namespace Scap.Common
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

    public static async Task Authorize(string pin, Settings settings)
    {
      var request = new HttpRequestMessage(HttpMethod.Post, Properties.Resources.ImgurOauthTokenUrl);
      request.Content = new FormUrlEncodedContent(new Dictionary<string, string>() {
        { "client_id", Properties.Resources.ImgurClientId },
        { "client_secret", Properties.Resources.ImgurClientSecret },
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
          : $"Client-ID {Properties.Resources.ImgurClientId}");
    }



    public static async Task<string> Upload(string base64Image, Settings settings)
    {
      var request = new HttpRequestMessage(HttpMethod.Post, Properties.Resources.ImgurUploadUrl);
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
