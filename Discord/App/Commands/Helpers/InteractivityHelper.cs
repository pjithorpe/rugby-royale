using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    internal static class InteractivityHelper
    {
        public async static Task<bool> CheckValid(this InteractivityResult<DiscordMessage> result, DiscordChannel channel, int? minLength = null, int? maxLength = null, string regexExp = null)
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

            string content = message.Content.Trim();
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

        public async static Task<Tuple<Task<InteractivityResult<MessageReactionAddEventArgs>>, Task<InteractivityResult<DiscordMessage>>>> WaitForReactionOrMessage(this DiscordChannel discordChannel, DiscordMessage message, DiscordEmoji emoji, DiscordMember member = null)
        {
            // wait for reaction or message response
            Task<InteractivityResult<DiscordMessage>> messageTask = discordChannel.GetNextMessageAsync(member);
            Task<InteractivityResult<MessageReactionAddEventArgs>> reactionTask = message.WaitForReactionAsync(member, emoji);

            Task firstResponse = await Task.WhenAny(messageTask, reactionTask);

            if (firstResponse is Task<InteractivityResult<MessageReactionAddEventArgs>> reactionResponse)
            {
                return Tuple.Create<Task<InteractivityResult<MessageReactionAddEventArgs>>, Task<InteractivityResult<DiscordMessage>>>(reactionResponse, null);
            }
            else if (firstResponse is Task<InteractivityResult<DiscordMessage>> messageResponse)
            {
                return Tuple.Create<Task<InteractivityResult<MessageReactionAddEventArgs>>, Task<InteractivityResult<DiscordMessage>>>(null, messageResponse);
            }

            return null;
        }
    }
}