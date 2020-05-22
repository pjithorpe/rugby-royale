using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    internal static class InteractivityHelper
    {
        public async static Task<bool> CheckValid(this InteractivityResult<DiscordMessage> result, DiscordChannel channel, int maxLength, bool optional = false)
        {
            if (result.TimedOut)
            {
                await channel.SendMessageAsync("No response. Cancelling.");
                return false;
            }

            DiscordMessage message = result.Result;
            if (optional || string.IsNullOrWhiteSpace(message.Content))
            {
                await channel.SendMessageAsync("Cannot be blank. Cancelling.");
                return false;
            }

            if (message.Content.Trim().Length <= 40)
            {
                await channel.SendMessageAsync($"Cannot be over {maxLength} characters. Cancelling.");
                return false;
            }

            return true;
        }
    }
}