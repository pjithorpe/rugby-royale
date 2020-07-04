using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.LeagueTypes;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    static class Royale_NewLeague
    {
        public static async Task ExecuteAsync(CommandContext context, Settings settings, ILeagueRepository leagueRepo)
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
            await dmChannel.SendMessageAsync($"Please respond with the **full name** of the new league (max. {longNameMax} characters, e.g. \"The Gallagher Premiership\"):");
            result = await dmChannel.GetNextMessageAsync(context.Member);

            if (!await result.CheckValid(dmChannel, 5, longNameMax)) return;
            string longName = result.Result.Content.Trim();

            // Short name
            if (!int.TryParse(settings.LeagueNameShortMaxLength, out int shortNameMax))
            {
                // ERROR
                return;
            }
            await dmChannel.SendMessageAsync($"Thanks! Now respond with a shortened version of the league's name (max. {shortNameMax} characters, e.g. \"The Prem\"):");
            result = await dmChannel.GetNextMessageAsync(context.Member);

            if (!await result.CheckValid(dmChannel, 5, shortNameMax)) return;
            string shortName = result.Result.Content.Trim();

            // League Type
            DiscordEmoji[] pollEmojis = settings.PollReactions
                .Select(s => DiscordEmoji.FromName(context.Client, s))
                .ToArray();
            LeagueType[] leagueTypes = Enum.GetValues(typeof(LeagueType))
                .Cast<LeagueType>()
                .ToArray();
            LeagueRules[] leagueTypeRules = leagueTypes
                .Select(lt => lt.GetRules())
                .ToArray();

            // build embed
            var leaguesEmbed = new DiscordEmbedBuilder()
            {
                Title = "Available League Types",
                Description = "Thanks! Now choose a league type by reacting to this message."
            };
            for (int i = 0; i < leagueTypeRules.Length; i++)
            {
                string fieldValue = $"\n   {leagueTypeRules[i].Description}";
                if (leagueTypeRules[i].Conferences > 1) fieldValue += $"\n   Conferences: {leagueTypeRules[i].Conferences}";
                fieldValue += $"\n   Teams: {leagueTypeRules[i].MinSize} - {leagueTypeRules[i].MaxSize}";

                // add league type as field in embed
                leaguesEmbed.AddField($"{pollEmojis[i]} - {leagueTypeRules[i].Name}", fieldValue);
            }

            DiscordMessage pollMessage = await dmChannel.SendMessageAsync(embed: leaguesEmbed);

            // add reactions
            var emojiLeagueMappings = new Dictionary<DiscordEmoji, LeagueType>();
            for (int i = 0; i < leagueTypes.Length; i++)
            {
                emojiLeagueMappings.Add(pollEmojis[i], leagueTypes[i]);
            }

            foreach (DiscordEmoji emoji in emojiLeagueMappings.Keys)
            {
                await pollMessage.CreateReactionAsync(emoji);
            }

            // collect and check result
            InteractivityResult<MessageReactionAddEventArgs> pollResult = await pollMessage.WaitForReactionAsync(context.Member);

            if (!await pollResult.CheckValid(dmChannel, pollEmojis)) return;
            DiscordEmoji emojiChosen = pollResult.Result.Emoji;

            LeagueType leagueType = emojiLeagueMappings[emojiChosen];

            var newLeague = new League()
            {
                Name_Long = longName,
                Name_Short = shortName,
                LeagueType = leagueType,
                DaysPerRound = 3,
                HasStarted = false,
                UserID = context.User.Id.ToString()
            };

            // Save to DB
            if (!await leagueRepo.SaveAsync(newLeague))
            {
                await dmChannel.SendMessageAsync("Failed to save new league. Cancelling.");
                return;
            }

            await dmChannel.SendMessageAsync("League created successfully.");
        }
    }
}
