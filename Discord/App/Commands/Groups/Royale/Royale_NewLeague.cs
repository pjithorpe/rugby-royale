﻿using DSharpPlus;
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
        public static async Task ExecuteAsync(CommandContext context, Settings settings, ILeagueRepository leagueRepo, ILeagueUserRepository leagueUserRepo)
        {
            DiscordClient discordClient = context.Client;
            InteractivityExtension interactivity = discordClient.GetInteractivity();
            DiscordDmChannel dmChannel = await context.Member.CreateDmChannelAsync();

            if (!int.TryParse(settings.LeagueNameLongMaxLength, out int longNameMax)) throw new Exception("Failed to read setting: LeagueNameLongMaxLength");
            if (!int.TryParse(settings.LeagueNameShortMaxLength, out int shortNameMax)) throw new Exception("Failed to read setting: LeagueNameShortMaxLength");

            // Long name
            string longName = await dmChannel.GetInputString(context.Member, $"Please respond with the **full name** of the new league (max. {longNameMax} characters, e.g. \"The Gallagher Premiership\"):", 5, longNameMax);
            if (longName == null) return;

            // Short name
            string shortName = await dmChannel.GetInputString(context.Member, $"Thanks! Now respond with a shortened version of the league's name (max. {shortNameMax} characters, e.g. \"The Prem\"):", 5, shortNameMax);
            if (shortName == null) return;

            // League Type
            DiscordEmoji[] pollEmojis = settings.PollReactions
                .Select(s => DiscordEmoji.FromName(discordClient, s))
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

            // League size
            LeagueRules leagueRules = leagueType.GetRules();
            int defaultMinSize = leagueRules.MinSize;
            int defaultMaxSize = leagueRules.MaxSize;
            DiscordEmoji skipEmoji = discordClient.SkipEmoji();

            // minimum size
            DiscordMessage sizeMessage = await dmChannel.SendMessageAsync($"Please respond with the **minimum size** (no. of teams) of the new league (default: {leagueRules.MinSize}). Cannot be less than the default value. {skipEmoji} to skip):");
            await sizeMessage.CreateReactionAsync(skipEmoji);

            ReactionOrMessageTask reactionOrMessageResult = await dmChannel.WaitForReactionOrMessage(sizeMessage, skipEmoji, context.Member);

            int minSize;
            if (reactionOrMessageResult.ReactionTask != null)
            {
                InteractivityResult<MessageReactionAddEventArgs> skipResult = await reactionOrMessageResult.ReactionTask;
                if (!await skipResult.CheckValid(dmChannel)) return;

                // skipped - use default
                minSize = defaultMinSize;
            }
            else if (reactionOrMessageResult.MessageTask != null)
            {
                InteractivityResult<DiscordMessage> messageResult = await reactionOrMessageResult.MessageTask;
                if (!await messageResult.CheckValidInt(dmChannel, minValue: defaultMinSize)) return;
                minSize = int.Parse(messageResult.Result.Content.Trim());
            }
            else
            {
                await dmChannel.SendMessageAsync("No response. Cancelling");
                return;
            }


            // maximum size
            sizeMessage = await dmChannel.SendMessageAsync($"Thanks! Now respond with the **maximum size** of the new league (default: {leagueRules.MaxSize}). Cannot be more than the default value. {skipEmoji} to skip):");
            await sizeMessage.CreateReactionAsync(skipEmoji);

            reactionOrMessageResult = await dmChannel.WaitForReactionOrMessage(sizeMessage, skipEmoji, context.Member);

            int maxSize;
            if (reactionOrMessageResult.ReactionTask != null)
            {
                InteractivityResult<MessageReactionAddEventArgs> skipResult = await reactionOrMessageResult.ReactionTask;
                if (!await skipResult.CheckValid(dmChannel)) return;

                // skipped - use default
                maxSize = defaultMaxSize;
            }
            else if (reactionOrMessageResult.MessageTask != null)
            {
                InteractivityResult<DiscordMessage> messageResult = await reactionOrMessageResult.MessageTask;
                if (!await messageResult.CheckValidInt(dmChannel, maxValue: defaultMaxSize)) return;
                maxSize = int.Parse(messageResult.Result.Content.Trim());
            }
            else
            {
                await dmChannel.SendMessageAsync("No response. Cancelling");
                return;
            }

            var newLeague = new League()
            {
                Name_Long = longName,
                Name_Short = shortName,
                LeagueType = leagueType,
                DaysPerRound = 3,
                HasStarted = false,
                Size_Min = minSize,
                Size_Max = maxSize,
                UserID = context.User.Id.ToString()
            };

            // Save to DB
            if (!await leagueRepo.SaveAsync(newLeague))
            {
                await dmChannel.SendMessageAsync("Failed to save new league. Cancelling.");
                return;
            }

            await dmChannel.SendMessageAsync("League created successfully.");

            // Send an advert for the league to the main channel
            await Royale_MyLeague.ExecuteAsync(context, settings, leagueRepo, leagueUserRepo);
        }
    }
}
