using DSharpPlus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Discord.Logging
{
    public class DiscordLoggerFactory : ILoggerFactory
    {
        private DiscordLoggerConfiguration config;

        public DiscordLoggerFactory(DiscordLoggerConfiguration configuration)
        {
            config = configuration;
            provider = new DiscordLoggerProvider(config);
        }

        private ILoggerProvider provider { get; }

        public void AddProvider(ILoggerProvider provider) { }

        public ILogger CreateLogger(string categoryName)
        {
            if (this.isDisposed) throw new InvalidOperationException($"This {this.GetType().Name} is already disposed.");

            return provider.CreateLogger(categoryName);
        }

        #region IDisposable Support
        private bool isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    provider.Dispose();
                }

                isDisposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
