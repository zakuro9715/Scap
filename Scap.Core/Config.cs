using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Markup;

namespace Scap.Core
{
  class Config
  {
    private static IConfigurationRoot config;
    static Config()
    {
      var builder = new ConfigurationBuilder()
        //.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("config.json");
      config = builder.Build();
    }

    public static IConfigurationSection Imgur { get => config.GetSection("Imgur"); }
  }
}
