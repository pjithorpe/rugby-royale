using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class MainChannel : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            Settings settings = Settings.GetSettings();

            string channelID = ctx.Channel.Id.ToString();

            return Task.FromResult(settings.MainChannel == channelID);
        }
    }
}