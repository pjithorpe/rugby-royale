using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Helpers;
using DSharpPlus.Interactivity;
using RugbyRoyale.Data.Repository;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    static class Royale_NewTeam
    {
        public static async Task ExecuteAsync(CommandContext context, Settings settings, IRepositoryCollection repos)
        {
            DiscordClient discordClient = context.Client;
            DiscordDmChannel dmChannel = await context.Member.CreateDmChannelAsync();

            // Check if user already has a team
            string userID = context.User.Id.ToString();
            if (await repos.Teams.ExistsAsync(userID))
            {
                await dmChannel.SendMessageAsync("You already have a team. Cancelling.");
                return;
            }

            // Get settings
            if (!int.TryParse(settings.TeamNameLongMaxLength, out int longNameMax)) throw new Exception("Failed to read setting: TeamNameLongMaxLength");
            if (!int.TryParse(settings.TeamNameShortMaxLength, out int shortNameMax)) throw new Exception("Failed to read setting: TeamNameShortMaxLength");

            // Get all teams in db
            List<Team> allTeams = await teamRepo.ListAllAsync();

            // Long name
            string longName = await dmChannel.GetInputString(context.Member, $"Please respond with the **full name** of the new team (max. {longNameMax} characters, e.g. \"Leicester Tigers\"):", 5, longNameMax);
            if (longName == null) return;

            if (allTeams.Exists(x => x.Name_Long.Replace(" ", "").ToLower() == longName.Replace(" ", "").ToLower()))
            {
                await dmChannel.SendMessageAsync("Name is too similar to an already existing name. Cancelling.");
                return;
            }

            // Short name
            string shortName = await dmChannel.GetInputString(context.Member, $"Thanks! Now respond with a **shortened version** of the team's name (max. {shortNameMax} characters e.g. \"Tigers\"):", 5, shortNameMax);
            if (shortName == null) return;

            if (allTeams.Exists(x => x.Name_Short.Replace(" ", "").ToLower() == shortName.Replace(" ", "").ToLower()))
            {
                await dmChannel.SendMessageAsync("Name is too similar to an already existing name. Cancelling.");
                return;
            }

            // Abbreviation
            string abbreviatedName = shortName.Substring(0, 3).ToUpper();
            InputPrompt_BinaryChoice prompt = new InputPrompt_BinaryChoice()
            {
                Title = "Abbreviated Team Name",
                PromptText = $"Thanks! An abbreviated version of your team's name will sometimes be used.\n\nDefault: **{abbreviatedName}**.",
                AcceptPrompt = "Accept default.",
                RejectPrompt = "Reject and enter different abbreviation."
            };
            DiscordEmoji emojiChosen = await dmChannel.GetInputBinaryChoice(context, prompt);
            if (emojiChosen == null) return;

            if (emojiChosen == discordClient.RejectEmoji())
            {
                // rejected, ask for different abbreviation
                abbreviatedName = await dmChannel.GetInputString(context.Member, $"Respond with your preferred abbreviation (must be exactly 3 uppercase characters and numbers e.g. \"LEI\" or \"R92\"):", regexExp: "[A-Z0-9]{3}");
                if (abbreviatedName == null) return;
            }

            if (allTeams.Exists(x => x.Name_Abbreviated == abbreviatedName))
            {
                await dmChannel.SendMessageAsync("That abbreviation is already in use. Cancelling.");
                return;
            }

            // Colour
            var skipEmoji = discordClient.SkipEmoji();

            var colourEmbed = new DiscordEmbedBuilder()
            {
                Title = "Team Colours",
                Description = $"Thanks! Now we will set your teams colours. This will be done using HEX colour codes (e.g. #ff0090 for pink)."
            }
            .AddField("Colour picker", "You can use a colour picker such as [this one](https://www.google.com/search?q=color+picker) to get HEX codes.")
            .AddField("Responses", $"Optional responses can be skipped by reacting with {skipEmoji}.");

            await dmChannel.SendMessageAsync(embed: colourEmbed);

            // primary
            string regex = "#*[a-zA-Z0-9]{6}";
            string primaryColour = await dmChannel.GetInputString(context.Member, $"Respond with your **primary** colour:", regexExp: regex);
            if (primaryColour == null) return;

            // secondary
            string secondaryColour;

            DiscordMessage colourMessage = await dmChannel.SendMessageAsync($"Respond with your **secondary** colour (optional, {skipEmoji} to skip):");
            await colourMessage.CreateReactionAsync(skipEmoji);

            ReactionOrMessageTask reactionOrMessageResult = await dmChannel.WaitForReactionOrMessage(colourMessage, skipEmoji, context.Member);
            if (reactionOrMessageResult.ReactionTask != null)
            {
                InteractivityResult<MessageReactionAddEventArgs> skipResult = await reactionOrMessageResult.ReactionTask;
                if (!await skipResult.CheckValid(dmChannel)) return;

                // optional colour skipped - use primary colour
                secondaryColour = primaryColour;
            }
            else if (reactionOrMessageResult.MessageTask != null)
            {
                InteractivityResult<DiscordMessage> messageResult = await reactionOrMessageResult.MessageTask;
                if (!await messageResult.CheckValidString(dmChannel, regexExp: regex)) return;
                secondaryColour = messageResult.Result.Content.Trim();
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

            reactionOrMessageResult = await dmChannel.WaitForReactionOrMessage(colourMessage, skipEmoji, context.Member);
            if (reactionOrMessageResult.ReactionTask != null)
            {
                InteractivityResult<MessageReactionAddEventArgs> skipResult = await reactionOrMessageResult.ReactionTask;
                if (!await skipResult.CheckValid(dmChannel)) return;

                // optional colour skipped - use primary colour
                tertiaryColour = primaryColour;
            }
            else if (reactionOrMessageResult.MessageTask != null)
            {
                InteractivityResult<DiscordMessage> messageResult = await reactionOrMessageResult.MessageTask;
                if (!await messageResult.CheckValidString(dmChannel, regexExp: regex)) return;
                tertiaryColour = messageResult.Result.Content.Trim();
            }
            else
            {
                await dmChannel.SendMessageAsync("No response. Cancelling");
                return;
            }

            // Create team
            var team = new Team()
            {
                Name_Long = longName,
                Name_Short = shortName,
                Name_Abbreviated = abbreviatedName,
                Colour_Primary = primaryColour,
                Colour_Secondary = secondaryColour,
                Colour_Tertiary = tertiaryColour,
            };

            // Register user if new
            if (!await repos.Users.ExistsAsync(userID))
            {
                repos.Users.Add(new User() {
                    UserID = userID,
                    Team = team,
                });
            }
            else
            {
                // Otherwise just add team to user
                team.UserID = userID;
                repos.Teams.Add(team);
            }

            // Save to DB
            if (!await repos.Commit())
            {
                await dmChannel.SendMessageAsync("Failed to save new team. Cancelling.");
                return;
            }

            await dmChannel.SendMessageAsync("Team created successfully.");
        }
    }
}
