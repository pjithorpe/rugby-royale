using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Helpers;
using RugbyRoyale.Data.Repository;
using RugbyRoyale.Discord.Helpers;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.LeagueTypes;
using RugbyRoyale.Entities.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    static class Royale_MyLeague
    {
        public static async Task ExecuteAsync(CommandContext context, Settings settings, MessageTracker messageTracker, IRepositoryCollection repos)
        {
            League league = await repos.Leagues.GetWithUsersAsync(context.User.Id.ToString());
            LeagueRules rules = league.LeagueType.GetRules();

            // If not yet started, advertise for people to join
            if (!league.HasStarted)
            {
                // Get joined users
                string joinedString;
                if (league.Users != null && league.Users.Count > 0)
                {
                    var joinedMembers = new List<DiscordMember>();
                    foreach (User user in league.Users)
                    {
                        joinedMembers.Add(await context.Guild.GetMemberAsync(ulong.Parse(user.UserID)));
                    }
                    joinedString = string.Join(", ", joinedMembers.Select(dm => dm.DisplayName));
                }
                else
                {
                    joinedString = "-";
                }

                // Build advert embed
                var leagueAdvertBuilder = new DiscordEmbedBuilder().ConstructSafely("A New Competition", $"Introducing {context.Member.DisplayName}'s new competition: **{league.Name_Long}**.");
                leagueAdvertBuilder.AddFieldSafely("Format and Rules", $"{league.Name_Short} is a {rules.Name} competition. {rules.Description}");
                leagueAdvertBuilder.AddFieldSafely("Requirements", $"This competition needs at least {rules.MinSize} players to start. Players will be expected to play at least 1 game every {league.DaysPerRound} days.");
                leagueAdvertBuilder.AddFieldSafely(Constants.FIELD_NAME_LeagueMemberNames, joinedString);
                leagueAdvertBuilder.AddFieldSafely(context.Client.AcceptEmoji().GetDiscordName(), "Join competition", true);
                leagueAdvertBuilder.AddFieldSafely(context.Client.RejectEmoji().GetDiscordName(), "Leave competition", true);
                leagueAdvertBuilder.Color = DiscordColor.Green;

                DiscordChannel mainChannel = context.Guild.GetChannel(ulong.Parse(settings.MainChannel));
                DiscordMessage message = await mainChannel.SendMessageAsync(embed: leagueAdvertBuilder);
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
