using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    static class Royale_NewTeam
    {
        public static async Task ExecuteAsync(CommandContext context, Settings settings)
        {
            InteractivityExtension interactivity = context.Client.GetInteractivity();
            DiscordDmChannel dmChannel = await context.Member.CreateDmChannelAsync();
            InteractivityResult<DiscordMessage> result;

            // Long name
            if (!int.TryParse(settings.LeagueNameLongMaxLength, out int longNameMax))
            {
                // ERROR
                return;
            }
            await dmChannel.SendMessageAsync($"Please respond with the **full name** of the new team (max. {longNameMax} characters, e.g. \"Leicester Tigers\"):");
            result = await dmChannel.GetNextMessageAsync(context.Member);

            if (!await result.CheckValid(dmChannel, 5, longNameMax)) return;
            string longName = result.Result.Content.Trim();

            // Short name
            if (!int.TryParse(settings.LeagueNameShortMaxLength, out int shortNameMax))
            {
                // ERROR
                return;
            }
            await dmChannel.SendMessageAsync($"Thanks! Now respond with a **shortened version** of the team's name (max. {shortNameMax} characters e.g. \"Tigers\"):");
            result = await dmChannel.GetNextMessageAsync(context.Member);

            if (!await result.CheckValid(dmChannel, 5, shortNameMax)) return;
            string shortName = result.Result.Content.Trim();

            // Abbreviation
            string acceptEmojiName = ":white_check_mark:";
            string rejectEmojiName = ":x:";
            var acceptEmoji = DiscordEmoji.FromName(context.Client, acceptEmojiName);
            var rejectEmoji = DiscordEmoji.FromName(context.Client, rejectEmojiName);

            string abbreviatedName = shortName.Substring(0, 3).ToUpper();

            var abbreviationEmbed = new DiscordEmbedBuilder()
            {
                Title = "Abbreviated Team Name",
                Description = $"Thanks! An abbreviated version of your team's name will sometimes be used.\n\nDefault: **{abbreviatedName}**."
            }
            .AddField(acceptEmojiName, "Accept default.")
            .AddField(rejectEmojiName, "Reject and enter different abbreviation.");

            DiscordMessage pollMessage = await dmChannel.SendMessageAsync(embed: abbreviationEmbed);
            await pollMessage.CreateReactionAsync(acceptEmoji);
            await pollMessage.CreateReactionAsync(rejectEmoji);

            // collect and check result
            InteractivityResult<MessageReactionAddEventArgs> pollResult = await pollMessage.WaitForReactionAsync(context.Member);

            if (!await pollResult.CheckValid(dmChannel, new DiscordEmoji[] { acceptEmoji, rejectEmoji })) return;
            DiscordEmoji emojiChosen = pollResult.Result.Emoji;

            if (emojiChosen == rejectEmoji)
            {
                // rejected, ask for different abbreviation
                await dmChannel.SendMessageAsync($"Respond with your preferred abbreviation (max. 3 characters e.g. \"LEI\"):");
                result = await dmChannel.GetNextMessageAsync(context.Member);

                if (!await result.CheckValid(dmChannel, 3, 3)) return;
                abbreviatedName = result.Result.Content.Trim();
            }

            // Colour
            string skipEmojiName = ":leftwards_arrow_with_hook:";
            var skipEmoji = DiscordEmoji.FromName(context.Client, skipEmojiName);

            var colourEmbed = new DiscordEmbedBuilder()
            {
                Title = "Team Colours",
                Description = $"Thanks! Now we will set your teams colours. This will be done using HEX colour codes (e.g. #ff0090 for pink)."
            }
            .AddField("Colour picker", "You can use a colour picker such as [this one](https://www.google.com/search?q=color+picker) to get HEX codes.")
            .AddField("Responses", $"Optional responses can be skipped by reacting with {skipEmoji}.");

            await dmChannel.SendMessageAsync(embed: colourEmbed);

            // primary
            await dmChannel.SendMessageAsync($"Respond with your **primary** colour:");
            result = await dmChannel.GetNextMessageAsync(context.Member);

            string regex = "#*[a-zA-Z0-9]{6}";
            if (!await result.CheckValid(dmChannel, regexExp: regex)) return;
            string primaryColour = result.Result.Content.Trim();

            // secondary
            string secondaryColour;

            DiscordMessage colourMessage = await dmChannel.SendMessageAsync($"Respond with your **secondary** colour (optional, {skipEmoji} to skip):");
            await colourMessage.CreateReactionAsync(skipEmoji);

            ReactionOrMessageTask reactionOrMessageResult = await dmChannel.WaitForReactionOrMessage(colourMessage, rejectEmoji, context.Member);
            if (reactionOrMessageResult.ReactionTask != null)
            {
                InteractivityResult<MessageReactionAddEventArgs> rejectResult = await reactionOrMessageResult.ReactionTask;
                if (!await rejectResult.CheckValid(dmChannel)) return;

                // optional colour skipped - use primary colour
                secondaryColour = primaryColour;
            }
            else if (reactionOrMessageResult.MessageTask != null)
            {
                InteractivityResult<DiscordMessage> messageResult = await reactionOrMessageResult.MessageTask;
                if (!await messageResult.CheckValid(dmChannel, regexExp: regex)) return;
                secondaryColour = result.Result.Content.Trim();
            }
            else
            {
                await dmChannel.SendMessageAsync("No response. Cancelling");
                return;
            }

            // tertiary
            string tertiaryColour;

            colourMessage = await dmChannel.SendMessageAsync($"Respond with your **tertiary** colour (optional, {skipEmoji} to skip):");
            await colourMessage.CreateReactionAsync(skipEmoji);

            reactionOrMessageResult = await dmChannel.WaitForReactionOrMessage(colourMessage, rejectEmoji, context.Member);
            if (reactionOrMessageResult.ReactionTask != null)
            {
                InteractivityResult<MessageReactionAddEventArgs> rejectResult = await reactionOrMessageResult.ReactionTask;
                if (!await rejectResult.CheckValid(dmChannel)) return;

                // optional colour skipped - use primary colour
                tertiaryColour = primaryColour;
            }
            else if (reactionOrMessageResult.MessageTask != null)
            {
                InteractivityResult<DiscordMessage> messageResult = await reactionOrMessageResult.MessageTask;
                if (!await messageResult.CheckValid(dmChannel, regexExp: regex)) return;
                tertiaryColour = result.Result.Content.Trim();
            }
            else
            {
                await dmChannel.SendMessageAsync("No response. Cancelling");
                return;
            }

            var newTeam = new Team()
            {
                Name_Long = longName,
                Name_Short = shortName,
                Name_Abbreviated = abbreviatedName,
                Colour_Primary = primaryColour,
                Colour_Secondary = secondaryColour,
                Colour_Tertiary = tertiaryColour,
                UserID = context.User.Id.ToString()
            };

            await dmChannel.SendMessageAsync("Team created successfully.");
        }
    }
}
