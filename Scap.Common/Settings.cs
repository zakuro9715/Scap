using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scap.Common
{
  public class Settings : INotifyPropertyChanged
  {
    public Settings() { }

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
      get { return screenshotDir; }
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

    private bool usePreview;
    public bool UsePreview
    {
      get { return usePreview; }
      set
      {
        if (usePreview != value)
        {
          usePreview = value;
          NotifyPropertyChanged();
        }
      }
    }

    private bool uploadToImgur;
    public bool UploadToImgur
    {
      get { return uploadToImgur; }
      set
      {
        if (uploadToImgur != value)
        {
          uploadToImgur = value;
          NotifyPropertyChanged();
        }
      }
    }
    private string imgurToken;
    public string ImgurToken
    {
      get { return imgurToken; }
      set
      {
        if (imgurToken != value)
        {
          imgurToken = value;
          NotifyPropertyChanged();
        }
      }
    }


    private string imgurUsername;
    public string ImgurUsername
    {
      get { return imgurUsername; }
      set
      {
        if (imgurUsername != value)
        {
          imgurUsername = value;
          NotifyPropertyChanged();
        }
      }
    }

    private string imgurRefreshToken;
    public string ImgurRefreshToken
    {
      get { return imgurRefreshToken; }
      set
      {
        if (imgurRefreshToken != value)
        {
          imgurRefreshToken = value;
          NotifyPropertyChanged();
        }
      }
    }


    private int imgurExpiresIn;
    public int ImgurExpiresIn
    {
      get { return imgurExpiresIn; }
      set
      {
        if (imgurExpiresIn != value)
        {
          imgurExpiresIn = value;
          NotifyPropertyChanged();
        }
      }
    }

    public bool ImgurLoggedIn
    {
      get { return imgurToken.Length > 0; }
    }



    private readonly static string settingsDir = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
      AppInfo.AppName
    );

    public static string SettingsJsonPath {
       get { return Path.Combine(settingsDir, "Settings.json"); }
    }

    private readonly static string defaultScreenshotDir = Path.Combine(
      Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
      AppInfo.AppName
    );

    public static Settings Default {
      get {
        var settings = new Settings();
        settings.screenshotDir = defaultScreenshotDir;
        settings.openExplorer = true;
        settings.usePreview = true;
        return settings;
      }
    }


    public void Save()
    {
      Directory.CreateDirectory(settingsDir);
      var jsonStr= JsonSerializer.Serialize<Settings>(this);
      File.WriteAllText(SettingsJsonPath, jsonStr);
    }


    public static Settings Load()
    {
      if (!File.Exists(SettingsJsonPath))
      {
        return Default;
      }
      var jsonStr = File.ReadAllText(SettingsJsonPath);
      return JsonSerializer.Deserialize<Settings>(jsonStr);
    }
  }

}
