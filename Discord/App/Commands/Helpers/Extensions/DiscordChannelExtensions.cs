using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    internal static class DiscordChannelExtensions
    {
        public async static Task<string> GetInputString(this DiscordChannel channel, DiscordMember member, string inputPrompt, int? minLength = null, int? maxLength = null, string regexExp = null)
        {
            await channel.SendMessageAsync(inputPrompt);
            InteractivityResult<DiscordMessage> result = await channel.GetNextMessageAsync(member);

            if (!await result.CheckValidString(channel, minLength, maxLength)) return null;

            return result.Result.Content.Trim();
        }

        public async static Task<DiscordEmoji> GetInputBinaryChoice(this DiscordChannel channel, CommandContext context, InputPrompt_BinaryChoice prompt)
        {
            var acceptEmoji = context.Client.AcceptEmoji();
            var rejectEmoji = context.Client.RejectEmoji();

            var abbreviationEmbed = new DiscordEmbedBuilder()
            {
                Title = prompt.Title,
                Description = prompt.PromptText
            }
            .AddField(acceptEmoji.GetDiscordName(), prompt.AcceptPrompt)
            .AddField(rejectEmoji.GetDiscordName(), prompt.RejectPrompt);

            DiscordMessage pollMessage = await channel.SendMessageAsync(embed: abbreviationEmbed);
            await pollMessage.CreateReactionAsync(acceptEmoji);
            await pollMessage.CreateReactionAsync(rejectEmoji);

            // collect and check result
            InteractivityResult<MessageReactionAddEventArgs> pollResult = await pollMessage.WaitForReactionAsync(context.Member);

            if (!await pollResult.CheckValid(channel, new DiscordEmoji[] { acceptEmoji, rejectEmoji })) return null;

            return pollResult.Result.Emoji;
        }

        public async static Task<ReactionOrMessageTask> WaitForReactionOrMessage(this DiscordChannel discordChannel, DiscordMessage message, DiscordEmoji emoji, DiscordMember member = null)
        {
            Task<InteractivityResult<DiscordMessage>> messageTask = discordChannel.GetNextMessageAsync(member);
            Task<InteractivityResult<MessageReactionAddEventArgs>> reactionTask = message.WaitForReactionAsync(member, emoji);

            return new ReactionOrMessageTask(await Task.WhenAny(messageTask, reactionTask));
        }
    }
}
