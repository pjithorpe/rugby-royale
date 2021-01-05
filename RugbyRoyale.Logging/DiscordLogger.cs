using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.Logging
{
    public class DiscordLogger : ILogger
    {
        private readonly string name;
        private readonly DiscordLoggerConfiguration config;

        private DiscordClient discord;
        private DiscordChannel logChannel;

        public DiscordLogger(string categoryName, DiscordLoggerConfiguration configuration)
        {
            name = categoryName != "" ? categoryName : "Unknown";
            config = configuration;
            
            DiscordConfiguration discordConfig = new DiscordConfiguration
            {
                Token = config.BotToken,
                TokenType = TokenType.Bot
            };
            discord = new DiscordClient(discordConfig);
            if (!discord.ConnectAsync().Wait(TimeSpan.FromSeconds(30))) // Blocking action
            {
                throw new TimeoutException("Timed out while connecting to Discord.");
            }

            Task<DiscordChannel> channelTask = discord.GetChannelAsync(config.LogChannelID);
            if (channelTask.Wait(TimeSpan.FromSeconds(30))) // Blocking action
            {
                logChannel = channelTask.Result;
            }
            else
            {
                throw new TimeoutException($"Timed out while getting log channel. ID: {config.LogChannelID}");
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            // Scope not implemented
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return config.LogLevel <= logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (!config.LogLevelColours.TryGetValue(logLevel, out DiscordColor color))
            {
                color = DiscordColor.White;
            }

            string title = $"{logLevel}: {formatter(state, exception)}";

            string description = null;
            string stackTrace = null;
            if (exception != null)
            {
                Exception baseException = exception.GetBaseException();
                if (baseException != null && !string.IsNullOrWhiteSpace(baseException.Message))
                {
                    description = $"{baseException.GetType().FullName}: {baseException.Message}";
                }
                
                if (!string.IsNullOrWhiteSpace(exception.StackTrace))
                {
                    stackTrace = exception.StackTrace;
                }
            }

            var errorEmbed = new DiscordEmbedBuilder()
                .ConstructSafely(title, description ?? "{No exception}", footer: $"Source: {name}")
                .WithColor(color)
                .WithTimestamp(DateTime.UtcNow);

            if (stackTrace != null)
            {
                errorEmbed.AddFieldSafely("Stack Trace", stackTrace);
            }

            Task.Run(() => logChannel.SendMessageAsync(embed: errorEmbed));
        }

        #region IDisposable Support
        private bool isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // dispose managed objects
                    discord.DisconnectAsync().ContinueWith(t => discord.Dispose());
                }

                isDisposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
