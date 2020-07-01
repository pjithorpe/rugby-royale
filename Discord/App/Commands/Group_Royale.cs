using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using RugbyRoyale.Discord.App.Attributes;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.LeagueTypes;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    [Group("royale")]
    [MainChannel]
    public class Group_Royale : BaseCommandModule
    {
        private IClient client;
        private Settings settings;
        private ILeagueRepository leagueRepo;

        public Group_Royale(IClient gameClient, Settings appSettings, ILeagueRepository leagueRepository)
        {
            client = gameClient;
            settings = appSettings;
            leagueRepo = leagueRepository;
        }

        [Command("newteam"), Aliases("nt")]
        public async Task New(CommandContext context)
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

        [Command("newleague"), Aliases("nl")]
        public async Task NewLeague(CommandContext context)
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

        [Command("myleague"), Aliases("ml")]
        public async Task MyLeague(CommandContext context)
        {
            League league = await leagueRepo.GetAsync(context.User.Id.ToString());

            // If not yet started, advertise for people to join
            if (league.HasStarted)
            {
                //TODO
                var leagueAdvert = new DiscordEmbedBuilder()
                {
                    Title = "A New League",
                    Color = new DiscordColor(league.Colour),
                    Description = "",

                };
                await context.Channel.SendMessageAsync();
            }
            // otherwise, show league table
            else
            {
                await context.Channel.SendMessageAsync("TODO: League Table");
            }
        }

        /* For dev purposes */
        [Command("generate-team")]
        public async Task Test(CommandContext context)
        {
            await context.RespondAsync($"Generating random team...");

            var nationalities = new Nationality[]
            {
                Nationality.Argentine, Nationality.English, Nationality.French, Nationality.SouthAfrican, Nationality.Georgian,
                Nationality.Irish, Nationality.Scottish, Nationality.Japanese, Nationality.Italian, Nationality.NewZealander,
                Nationality.Russian, Nationality.Welsh, Nationality.Romanian
            };

            var testGen = new PlayerGenerator(50,  nationalities[new Random().Next(0, nationalities.Length)]);
            var testTeam = new Teamsheet()
            {
                LooseheadProp = await testGen.GeneratePlayer(Position.Prop),
                Hooker = await testGen.GeneratePlayer(Position.Hooker),
                TightheadProp = await testGen.GeneratePlayer(Position.Prop),
                Number4Lock = await testGen.GeneratePlayer(Position.Lock),
                Number5Lock = await testGen.GeneratePlayer(Position.Lock),
                BlindsideFlanker = await testGen.GeneratePlayer(Position.Flanker),
                OpensideFlanker = await testGen.GeneratePlayer(Position.Flanker),
                Number8 = await testGen.GeneratePlayer(Position.Number8),
                ScrumHalf = await testGen.GeneratePlayer(Position.ScrumHalf),
                FlyHalf = await testGen.GeneratePlayer(Position.FlyHalf),
                InsideCentre = await testGen.GeneratePlayer(Position.Centre),
                OutsideCentre = await testGen.GeneratePlayer(Position.Centre),
                LeftWing = await testGen.GeneratePlayer(Position.Wing),
                RightWing = await testGen.GeneratePlayer(Position.Wing),
                FullBack = await testGen.GeneratePlayer(Position.FullBack),
            };

            string output = "";
            int number = 1;
            foreach(Player player in testTeam.GetPlayers())
            {
                output += $"{number}. {player.FirstName} {player.LastName}";
                output += $"\n   Primary: {string.Join(' ', player.Positions_Primary.Select(p => p.ToString()))}";
                output += $"\n   Secondary: {string.Join(' ', player.Positions_Secondary.Select(p => p.ToString()))}";
                output += $"\n   Focus: {player.Focus}";
                output += $"\n   Stats: ATT {player.Attack}   DEF {player.Defence}   STA {player.Stamina}   PHY {player.Physicality}   HAN {player.Handling}   KIC {player.Kicking}\n";
                number++;
            }

            await context.RespondAsync(output);
        }
        /**/
    }
}