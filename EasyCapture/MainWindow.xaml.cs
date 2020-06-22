using EasyCapture.Common;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace EasyCapture
{
  /// <summary>
  /// MainWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class MainWindow : Window
  {
    private Settings settings;
    private CaptureDialog captureDialog;
    private PaintDialog paintDialog;
    public MainWindow()
    {
      InitializeComponent();
    }

    private void SaveImage(Bitmap image)
    {
      Directory.CreateDirectory(settings.ScreenshotDir);
      image.Save(System.IO.Path.Combine(settings.ScreenshotDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")) + ".png");
    }

    private void ShowCaptureDialog() {
      captureDialog = new CaptureDialog(settings);
      captureDialog.ShowDialog();
    }

    private void ShowPaintDialog()
    {
      var image = captureDialog.Image;
      paintDialog = new PaintDialog(image);
      paintDialog.ShowDialog();
    }

    private async Task ProcessImage(Bitmap image)
    {
      try
      {
        SaveImage(image);
        if (settings.UploadToImgur)
        {
          using (var ms = new MemoryStream())
          {
            image.Save(ms, ImageFormat.Png);
            var link = await Imgur.Upload(Convert.ToBase64String(ms.ToArray()), settings);
            Process.Start(new ProcessStartInfo("cmd", $"/c start { link.Replace("&", "^&") }"));
          }
        }

        if (settings.OpenExplorer)
        {
          Process.Start("EXPLORER.EXE", settings.ScreenshotDir);
        }
        Close();
      }
      catch (Exception e)
      {
        Debug.Crash(Properties.Resources.FaildToSaveImage, e);
      }

    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      settings = Settings.Load();
      ShowCaptureDialog();
      if (captureDialog.DialogResult == false)
      {
        Close();
        return;
      }

      ShowPaintDialog();
      if (paintDialog.DialogResult == false)
      {
        Close();
        return;
      }

      var path = System.IO.Path.Combine(settings.ScreenshotDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")) + ".png";
      using (var fs = new FileStream(path, FileMode.Create))
      {
        paintDialog.Encoder.Save(fs);
      }

      if (settings.UploadToImgur)
      {
        using (var ms = new MemoryStream())
        {
          byte[] data;
          using (var fs = File.Open(path, FileMode.Open))
          {
            data = new byte[fs.Length];
            fs.Read(data);
          }
          var link = await Imgur.Upload(Convert.ToBase64String(data), settings);
          Process.Start(new ProcessStartInfo("cmd", $"/c start { link.Replace("&", "^&") }"));
        }
      }

      if (settings.OpenExplorer)
      {
        Process.Start("EXPLORER.EXE", settings.ScreenshotDir);
      }


      Close();
    }
  }
}
