using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EasyCapture
{
  public static class Debug
  {
    public static void Crash(string message, Exception exp)
    {
      MessageBox.Show(String.Format(Properties.Resources.CrashMessageFormat, message, exp.StackTrace), "Error");
      Application.Current.Shutdown();
    }
  }
}
