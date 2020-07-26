using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace RugbyRoyale.Discord.Logging
{
    public class DiscordLoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public Dictionary<LogLevel, DiscordColor> LogLevelColours { get; set; }
        public string BotToken { get; set; }
        public ulong LogChannelID { get; set; }
    }
}
