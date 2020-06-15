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
      MessageBox.Show(message + exp.StackTrace);
      Application.Current.Shutdown();
    }
  }
}
