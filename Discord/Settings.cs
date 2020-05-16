using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace RugbyRoyale.Client
{
    public sealed class Settings
    {
        private Settings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            foreach (PropertyInfo p in typeof(Settings).GetProperties())
            {
                if (p.Name != "Instance")
                {
                    object value = configuration.GetSection("Settings")
                    .GetChildren().Where(s => s.Key == p.Name)
                    .FirstOrDefault().Value;

                    if (value == null)
                    {
                        Console.Error.WriteLine($"Failed to load setting {p.Name} from appsettings.json.");
                    }

                    p.SetValue(this, value);
                }
            }
        }

        public string BotToken { get; set; }
        public string CommandPrefix { get; set; }
        public string DBConnectionString { get; set; }

        private static readonly Lazy<Settings> lazy = new Lazy<Settings>(() => new Settings());

        public static Settings GetSettings()
        {
            return lazy.Value;
        }
    }
}