using CXuesong.Uel.Serilog.Sinks.Discord;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RugbyRoyale.Discord.App.Attributes;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.GameEngine;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    public class AdminControls : BaseCommandModule
    {
        private DiscordWebhookMessenger logMessenger;

        public AdminControls(DiscordWebhookMessenger logWebhookMessenger)
        {
            logMessenger = logWebhookMessenger;
        }

        [Command("stop"), Aliases("exit", "kill")]
        [MainChannel]
        public async Task Start(CommandContext context)
        {
            await logMessenger.ShutdownAsync();
            logMessenger.Dispose();
        }
    }
}