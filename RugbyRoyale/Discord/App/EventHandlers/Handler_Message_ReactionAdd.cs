using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Helpers;
using RugbyRoyale.Data.Repository;
using RugbyRoyale.Discord.Helpers;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.EventHandlers
{
    public static class Handler_Message_ReactionAdd
    {
        public static async Task ExecuteAsync(MessageTracker messageTracker, DiscordClient discordClient, MessageReactionAddEventArgs e, IRepositoryCollection repo)
        {
            if (!messageTracker.CheckMessageIsCurrentLeagueAdvert(e.Message)) return;

            if (!messageTracker.TryGetAdvertisedLeagueID(e.Message, out Guid leagueID))
            {
                Log.Error("Failed to get leagueID from MessageTracker for league advert message: {MessageID}", e.Message.Id);
                return;
            }

            User user = await repo.Users.GetAsync(e.User.Id.ToString());
            if (user == null)
            {
                await e.Channel.SendMessageAsync($"{e.User.Mention}, you need to create a team before you can join a competition.");
                return;
            }

            // Load league and its users
            League league = await repo.Leagues.GetWithUsersAsync(leagueID);
            if (league == null)
            {
                Log.Error("Failed to get League with LeagueID {LeagueID} for league advert message: {MessageID}", leagueID, e.Message.Id);
                return;
            }

            // Join react chosen
            if (e.Emoji == discordClient.AcceptEmoji())
            {
                if (!await TryAddToLeagueAsync(user, league, e, repo)) return;
            }
            // Leave react
            else if (e.Emoji == discordClient.RejectEmoji())
            {
                if (!await TryRemoveFromLeagueAsync(user, league, e, repo)) return;
            }

            // Get updated league members
            if (league.Users == null || league.Users.Count < 1)
            {
                await e.Channel.SendMessageAsync($"Failed to get participants.");
                return;
            }

            await EditLeagueAdvert(league.Users, e);
        }

        private static async Task<bool> TryAddToLeagueAsync(User user, League league, MessageReactionAddEventArgs e, IRepositoryCollection repo)
        {
            // Check if league is full
            if (league.IsFull())
            {
                await e.Channel.SendMessageAsync($"Sorry {e.User.Mention}, that competition is full. 😥");
                return false;
            }

            // Check if user is already in this league
            if (league.Users.Any(u => u.UserID == user.UserID))
            {
                await e.Channel.SendMessageAsync($"{e.User.Mention}, you're already in a league.");
                return false;
            }

            // Get user's team
            string userID = e.User.Id.ToString();
            Team team = await repo.Teams.GetAsync(userID);
            if (team == null)
            {
                await e.Channel.SendMessageAsync($"{e.User.Mention}, you need to create a team before you can join a competition.");
                return false;
            }

            // Add user to league
            league.Users.Add(user);

            // Commit changes
            if (!await repo.Commit())
            {
                await e.Channel.SendMessageAsync($"Failed to join competition.");
                return false;
            }

            return true;
        }

        private static async Task<bool> TryRemoveFromLeagueAsync(User user, League league, MessageReactionAddEventArgs e, IRepositoryCollection repo)
        {
            // If user not in league, don't need to remove them
            if (!league.Users.Any(u => u.UserID == user.UserID)) return true;

            // Check if user has a team
            Team team = await repo.Teams.GetAsync(user.UserID);
            if (team == null)
            {
                Log.Warning("User with userID {UserID} being removed from league with leagueID {LeagueID} had no team.", user.UserID, league.LeagueID);
                return false;
            }

            // Add user to league
            league.Users.Remove(user);

            // Commit changes
            if (!await repo.Commit())
            {
                await e.Channel.SendMessageAsync($"Failed to leave competition.");
                return false;
            }

            return true;
        }

        private static async Task EditLeagueAdvert(IEnumerable<User> leagueUsers, MessageReactionAddEventArgs e)
        {
            // Build list of league member name strings
            var leagueMemberNames = new List<string>();
            foreach (User user in leagueUsers)
            {
                if (ulong.TryParse(user.UserID, out ulong discordID))
                {
                    DiscordMember member = await e.Guild.GetMemberAsync(discordID);
                    if (member != null)
                    {
                        leagueMemberNames.Add(member.DisplayName);
                    }
                    else
                    {
                        // Didn't find guild member with this ID, log a warning and just add their ID string
                        Log.Warning("Failed to get guild member with userID {UserID} while editing league advert.", discordID);
                        leagueMemberNames.Add(user.UserID);
                    }
                }
                else
                {
                    Log.Error("Failed to parse userID {UserID} while editing league advert.", user.UserID);
                    leagueMemberNames.Add(user.UserID ?? "ERROR");
                }
            }

            // Update message
            DiscordEmbed embed = e.Message.Embeds.FirstOrDefault();
            if (embed == null)
            {
                Log.Warning("{Username} has joined a league, but the reaction event handler failed to find an embed attached to the advert message with messageID {MessageID}.", e.User.Username, e.Message.Id);
                return;
            }

            string userList = string.Join(", ", leagueMemberNames);

            List<string> fieldNames = embed.Fields.Select(f => f.Name).ToList();
            int usersFieldPos = fieldNames.IndexOf(Constants.FIELD_NAME_LeagueMemberNames);
            
            var editedAdvert = new DiscordEmbedBuilder(embed).RemoveFieldAt(usersFieldPos);
            editedAdvert.AddFieldSafely(embed.Fields[usersFieldPos].Name, userList);
            
            var optionalEmbed = new Optional<DiscordEmbed>(editedAdvert);

            await e.Message.ModifyAsync(embed: optionalEmbed);

            // Remove the reaction that triggered the event
            await e.Message.DeleteReactionAsync(e.Emoji, e.User);
        }
    }
}
