using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.EventHandling;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.LeagueTypes;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    static class Royale_MyLeague
    {
        public static async Task ExecuteAsync(CommandContext context, ILeagueRepository leagueRepo)
        {
            League league = await leagueRepo.GetAsync(context.User.Id.ToString());
            LeagueRules rules = league.LeagueType.GetRules();

            // If not yet started, advertise for people to join
            if (league.HasStarted)
            {
                var leagueAdvert = new DiscordEmbedBuilder()
                {
                    Title = "A New Competition",
                    Color = DiscordColor.Green,
                    Description = $"Introducing {context.Member.Nickname}'s new competition: **{league.Name_Long}**.",
                }
                .AddField("Format and Rules", $"{league.Name_Short} is a {rules.Name} competition. {rules.Description}")
                .AddField("Requirements", $"This competition needs at least {rules.MinSize} to start. Players will be expected to play at least 1 game every {league.DaysPerRound}.")
                .AddField("Players Joined", "-");

                DiscordMessage message = await context.Channel.SendMessageAsync(embed: leagueAdvert);
                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":white_check_mark:"));
                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":x:"));
            }
            // otherwise, show league table
            else
            {
                await context.Channel.SendMessageAsync("TODO: League Table");
            }
        }
    }
}
