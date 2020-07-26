using DSharpPlus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Discord.Logging
{
    public class DiscordLoggerProvider : ILoggerProvider
    {
        private readonly DiscordLoggerConfiguration config;
        private readonly ConcurrentDictionary<string, DiscordLogger> loggers = new ConcurrentDictionary<string, DiscordLogger>();

        public DiscordLoggerProvider(DiscordLoggerConfiguration configuration)
        {
            config = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, new DiscordLogger(categoryName, config));
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
                    foreach (DiscordLogger logger in loggers.Values)
                    {
                        logger.Dispose();
                    }
                    loggers.Clear();
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
