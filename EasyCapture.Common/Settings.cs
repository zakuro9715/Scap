using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyCapture.Common
{
  public class Settings : INotifyPropertyChanged
  {

    public Settings() { }
    public Settings(string screenshotDir, bool openExplorer)
    {
      this.screenshotDir = screenshotDir;
      this.openExplorer = openExplorer;
    }

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
      get { return new Settings(defaultScreenshotDir, true); }
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
