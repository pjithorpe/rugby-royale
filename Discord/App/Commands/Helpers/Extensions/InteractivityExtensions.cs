using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    internal static class InteractivityExtensions
    {
        public async static Task<bool> CheckValid(this InteractivityResult<DiscordMessage> result, DiscordChannel channel)
        {
            if (result.TimedOut || result.Result == null)
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

            return true;
        }

        public async static Task<bool> CheckValid(this InteractivityResult<MessageReactionAddEventArgs> result, DiscordChannel channel, DiscordEmoji[] validEmojis = null)
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

            if (validEmojis != null && !validEmojis.Any(e => e == emoji))
            {
                await channel.SendMessageAsync($"{emoji.GetDiscordName()} is not a valid response. Cancelling.");
                return false;
            }

            return true;
        }

        public async static Task<bool> CheckValidInt(this InteractivityResult<DiscordMessage> result, DiscordChannel channel, int? minValue = null, int? maxValue = null)
        {
            if (!await result.CheckValid(channel)) return false;

            string content = result.Result.Content.Trim();
            if (!int.TryParse(content, out int value))
            {
                await channel.SendMessageAsync($"Must be an integer value. Cancelling.");
                return false;
            }

            if (minValue != null && value < minValue)
            {
                await channel.SendMessageAsync($"Cannot be under {minValue}. Cancelling.");
                return false;
            }

            if (maxValue != null && value > maxValue)
            {
                await channel.SendMessageAsync($"Cannot be over {maxValue}. Cancelling.");
                return false;
            }

            return true;
        }

        public async static Task<bool> CheckValidString(this InteractivityResult<DiscordMessage> result, DiscordChannel channel, int? minLength = null, int? maxLength = null, string regexExp = null)
        {
            if (!await result.CheckValid(channel)) return false;

            string content = result.Result.Content.Trim();
            if (minLength != null && content.Length < minLength)
            {
                await channel.SendMessageAsync($"Cannot be under {minLength} characters. Cancelling.");
                return false;
            }

            if (maxLength != null && content.Length > maxLength)
            {
                await channel.SendMessageAsync($"Cannot be over {maxLength} characters. Cancelling.");
                return false;
            }

            if (regexExp != null && !Regex.IsMatch(content, regexExp))
            {
                await channel.SendMessageAsync($"Response not valid. Cancelling.");
                return false;
            }

            return true;
        }

        public async static Task<ReactionOrMessageTask> WaitForReactionOrMessage(this DiscordChannel discordChannel, DiscordMessage message, DiscordEmoji emoji, DiscordMember member = null)
        {
            Task<InteractivityResult<DiscordMessage>> messageTask = discordChannel.GetNextMessageAsync(member);
            Task<InteractivityResult<MessageReactionAddEventArgs>> reactionTask = message.WaitForReactionAsync(member, emoji);

            return new ReactionOrMessageTask(await Task.WhenAny(messageTask, reactionTask));
        }
    }
}