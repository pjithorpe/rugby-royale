using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.EventHandlers
{
    public static class Handler_Message_ReactionAdd
    {
        public static async Task ExecuteAsync(MessageTracker messageTracker, MessageReactionAddEventArgs e, ITeamRepository teamRepo, ILeagueRepository leagueRepo, ILeagueUserRepository leagueUserRepo)
        {
            if (messageTracker.CheckMessageIsCurrentLeagueAdvert(e.Message))
            {
                Guid leagueID = messageTracker.GetAdvertisedLeagueID(e.Message);

                League league = await leagueRepo.GetAsync(leagueID);

                // Check if league is full
                if (await leagueUserRepo.CountAsync(leagueID) == league.Size)
                {
                    await e.Channel.SendMessageAsync($"Sorry {e.User.Mention}, that competition is full. 😥");
                    return;
                }

                // Check if user has a team
                Team team = await teamRepo.GetAsync(e.User.Id.ToString());
                if (team == null)
                {
                    await e.Channel.SendMessageAsync($"{e.User.Mention}, you need to create a team before you can join a competition.");
                    return;
                }

                // Add team to league
                team.LeagueID = leagueID;
                if (!await teamRepo.EditAsync(team))
                {
                    await e.Channel.SendMessageAsync($"Failed to add team to competition.");
                    return;
                }

                // Add user to league
                var leagueUser = new LeagueUser()
                {
                    LeagueID = leagueID,
                    UserID = e.User.Id.ToString()
                };
                if (!await leagueUserRepo.SaveAsync(leagueUser))
                {
                    await e.Channel.SendMessageAsync($"Failed to join competition.");
                    return;
                }

                // Update message
                DiscordEmbed embed = e.Message.Embeds.FirstOrDefault();
                if (embed == null)
                {
                    // Something went wrong. As a backup, send a message confirming the user has been added to the league
                    await e.Channel.SendMessageAsync($"{e.User.Mention} has joined {league.Name_Long}.");
                    return;
                }

                List<LeagueUser> leagueUsers = await leagueUserRepo.ListAsync(leagueID);
                if (leagueUsers == null || leagueUsers.Count < 1)
                {
                    await e.Channel.SendMessageAsync($"Failed to get participants.");
                    return;
                }

                var leagueMembers = new List<DiscordMember>();
                foreach (LeagueUser lgUser in leagueUsers)
                {
                    if (!ulong.TryParse(lgUser.UserID, out ulong discordID))
                    {
                        await e.Channel.SendMessageAsync($"Failed to get participants.");
                    }
                    leagueMembers.Add(await e.Guild.GetMemberAsync(discordID));
                }

                string userList = string.Join(", ", leagueMembers.Select(m => m.Nickname));

                int usersFieldPos = embed.Fields.Count - 3;
                var editedAdvert = new DiscordEmbedBuilder(embed)
                    .RemoveFieldAt(usersFieldPos)
                    .AddField(embed.Fields[usersFieldPos].Name, userList);
                Optional<DiscordEmbed> optionalEmbed = new Optional<DiscordEmbed>(editedAdvert);

                await e.Message.ModifyAsync(embed: optionalEmbed);
            }
        }
    }
}
