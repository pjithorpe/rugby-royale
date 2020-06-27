using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RugbyRoyale.Discord
{
    public sealed class Settings
    {
        public string BotToken { get; set; }
        public string CommandPrefix { get; set; }
        public string DBConnectionString { get; set; }
        public string LeagueNameLongMaxLength { get; set; }
        public string LeagueNameShortMaxLength { get; set; }
        public string MainChannel { get; set; }
        public string TransferChannel { get; set; }
        public string[] MatchChannels { get; set; }
        public string[] PollReactions { get; set; }

        private Settings()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IEnumerable<IConfigurationSection> config = configuration
                .GetSection("Config")
                .GetChildren();

            IEnumerable<IConfigurationSection> settings = configuration
                .GetSection("Settings")
                .GetChildren();

            BotToken = GetConfigItem(config, "BotToken");
            DBConnectionString = GetConfigItem(config, "DBConnectionString");
            MainChannel = GetConfigItem(config, "MainChannel");
            TransferChannel = GetConfigItem(config, "TransferChannel");
            MatchChannels = GetConfigItemList(config, "MatchChannels");

            CommandPrefix = GetConfigItem(settings, "CommandPrefix");
            LeagueNameLongMaxLength = GetConfigItem(settings, "LeagueNameLongMaxLength");
            LeagueNameShortMaxLength = GetConfigItem(settings, "LeagueNameShortMaxLength");
            PollReactions = GetConfigItemList(settings, "PollReactions");
        }

        private static string GetConfigItem(IEnumerable<IConfigurationSection> configSection, string itemID)
        {
            return configSection.Where(cs => cs.Key == itemID).First().Value;
        }

        private static string[] GetConfigItemList(IEnumerable<IConfigurationSection> configSection, string itemID)
        {
            return configSection.Where(cs => cs.Key == itemID).First().Get<string[]>();
        }

        private static readonly Lazy<Settings> lazy = new Lazy<Settings>(() => new Settings());

        public static Settings GetSettings()
        {
            return lazy.Value;
        }
    }
}