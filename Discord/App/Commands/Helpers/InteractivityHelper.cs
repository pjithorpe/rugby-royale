using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using System.Linq;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    internal static class InteractivityHelper
    {
        public async static Task<bool> CheckValid(this InteractivityResult<DiscordMessage> result, DiscordChannel channel, int maxLength)
        {
            if (result.TimedOut)
            {
                await channel.SendMessageAsync("No response. Cancelling.");
                return false;
            }

            DiscordMessage message = result.Result;
            if (string.IsNullOrWhiteSpace(message.Content))
            {
                await channel.SendMessageAsync("Cannot be blank. Cancelling.");
                return false;
            }

            if (message.Content.Trim().Length > maxLength)
            {
                await channel.SendMessageAsync($"Cannot be over {maxLength} characters. Cancelling.");
                return false;
            }

            return true;
        }

        public async static Task<bool> CheckValid(this InteractivityResult<MessageReactionAddEventArgs> result, DiscordChannel channel, DiscordEmoji[] validEmojis)
        {
            if (result.TimedOut || result.Result == null)
            {
                await channel.SendMessageAsync("No response. Cancelling.");
                return false;
            }

            DiscordEmoji emoji = result.Result.Emoji;
            if (emoji == null)
            {
                await channel.SendMessageAsync("No reaction. Cancelling.");
                return false;
            }

            if (!validEmojis.Any(e => e == emoji))
            {
                await channel.SendMessageAsync($"{emoji.GetDiscordName()} is not a valid response. Cancelling.");
                return false;
            }

            return true;
        }
    }
}