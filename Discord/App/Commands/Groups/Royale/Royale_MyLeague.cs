using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Helpers;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.LeagueTypes;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    static class Royale_MyLeague
    {
        public static async Task ExecuteAsync(CommandContext context, Settings settings, MessageTracker messageTracker, ILeagueRepository leagueRepo, ILeagueUserRepository leagueUserRepo)
        {
            League league = await leagueRepo.GetAsync(context.User.Id.ToString());
            LeagueRules rules = league.LeagueType.GetRules();

            // If not yet started, advertise for people to join
            if (!league.HasStarted)
            {
                // Get joined users
                string joinedString;
                List<LeagueUser> leagueUsers = await leagueUserRepo.ListAsync(league.LeagueID);
                if (leagueUsers != null)
                {
                    var joinedMembers = new List<DiscordMember>();
                    foreach (LeagueUser leagueUser in leagueUsers)
                    {
                        joinedMembers.Add(await context.Guild.GetMemberAsync(ulong.Parse(leagueUser.UserID)));
                    }
                    joinedString = string.Join(", ", joinedMembers.Select(dm => dm.Nickname));
                }
                else
                {
                    joinedString = "-";
                }

                // Build advert embed
                var leagueAdvert = new DiscordEmbedBuilder()
                {
                    Title = "A New Competition",
                    Color = DiscordColor.Green,
                    Description = $"Introducing {context.Member.Nickname}'s new competition: **{league.Name_Long}**.",
                }
                .AddField("Format and Rules", $"{league.Name_Short} is a {rules.Name} competition. {rules.Description}")
                .AddField("Requirements", $"This competition needs at least {rules.MinSize} players to start. Players will be expected to play at least 1 game every {league.DaysPerRound} days.")
                .AddField("Players Joined", joinedString)
                .AddField(context.Client.AcceptEmoji().GetDiscordName(), "Join competition", true)
                .AddField(context.Client.RejectEmoji().GetDiscordName(), "Leave competition", true);

                DiscordChannel mainChannel = context.Guild.GetChannel(ulong.Parse(settings.MainChannel));
                DiscordMessage message = await mainChannel.SendMessageAsync(embed: leagueAdvert);
                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":white_check_mark:"));
                await message.CreateReactionAsync(DiscordEmoji.FromName(context.Client, ":x:"));

                // Add to advert tracker
                if (!messageTracker.TryAddLeagueAdvert(message.Id, league.LeagueID))
                {
                    DiscordMessage errorMessage = await context.Channel.SendMessageAsync("Failed to create competition advert. Deleting.");
                    await message.DeleteAsync();
                }
            }
            // otherwise, show league table
            else
            {
                await context.Channel.SendMessageAsync("TODO: League Table");
            }
        }
    }
}
