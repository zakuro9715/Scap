using Scap.Core;
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
using Image = System.Drawing.Image;

namespace Scap.App
{
  /// <summary>
  /// MainWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class MainWindow : Window
  {
    private Settings settings;
    public MainWindow()
    {
      InitializeComponent();
    }

    private Bitmap Capture(System.Drawing.Rectangle box)
    {
      var image = new Bitmap(box.Width, box.Height);
      var graphics = Graphics.FromImage(image);
      graphics.CopyFromScreen(
        new System.Drawing.Point(box.X, box.Y),
        new System.Drawing.Point(0, 0),
        new System.Drawing.Size(box.Width, box.Height)
      );
      SetImageToClipboard(image);
      return image;
    }

    private void SaveImage(Bitmap image)
    {
      Directory.CreateDirectory(settings.ScreenshotDir);
      image.Save(System.IO.Path.Combine(settings.ScreenshotDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")) + ".png");
    }

    private void SetImageToClipboard(Bitmap image)
    {
      using var ms = new MemoryStream();
      image.Save(ms, ImageFormat.Bmp);
      SetImageToClipboard(ms);
    }

    private void SetImageToClipboard(MemoryStream ms)
    {
      ms.Seek(0, System.IO.SeekOrigin.Begin);
      var source =
          BitmapFrame.Create(
              ms,
              BitmapCreateOptions.None,
              BitmapCacheOption.OnLoad
          );

      Clipboard.SetImage(source);
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      settings = Settings.Load();

      var captureDialog = new CaptureDialog(settings);
      captureDialog.ShowDialog();
      if (captureDialog.DialogResult == false)
      {
        Close();
        return;
      }

      var image = Capture(captureDialog.Rectangle);
      if (!Directory.Exists(settings.ScreenshotDir))
      {
        Directory.CreateDirectory(settings.ScreenshotDir);
      }
      var filepath = System.IO.Path.Combine(settings.ScreenshotDir, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")) + ".png";

      if (settings.UsePreview)
      {
        var paintDialog = new PaintDialog(image);
        paintDialog.ShowDialog();
        if (paintDialog.DialogResult == false)
        {
          Close();
          return;
        }

        using var ms = new MemoryStream();
        paintDialog.Encoder.Save(ms);
        SetImageToClipboard(ms);

        using var fs = new FileStream(filepath, FileMode.Create);
        ms.WriteTo(fs);
      }
      else
      {
        image.Save(filepath, ImageFormat.Png);
      }

      if (settings.UploadToImgur)
      {
        using var ms = new MemoryStream();
        byte[] data;
        using (var fs = File.Open(filepath, FileMode.Open))
        {
          data = new byte[fs.Length];
          fs.Read(data);
        }
        var link = await Imgur.Upload(Convert.ToBase64String(data), settings);
        Process.Start(new ProcessStartInfo("cmd", $"/c start { link.Replace("&", "^&") }"));
      }

      if (settings.OpenExplorer)
      {
        Process.Start("EXPLORER.EXE", settings.ScreenshotDir);
      }


      Close();
    }
  }
}
