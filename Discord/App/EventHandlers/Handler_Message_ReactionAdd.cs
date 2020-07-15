using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.EventHandlers
{
    public static class Handler_Message_ReactionAdd
    {
        public static async Task ExecuteAsync(MessageTracker messageTracker, MessageReactionAddEventArgs e, ILeagueRepository leagueRepo, ILeagueUserRepository leagueUserRepo)
        {
            if (messageTracker.CheckMessageIsCurrentLeagueAdvert(e.Message))
            {
                Guid leagueID = messageTracker.GetAdvertisedLeagueID(e.Message);

                League league = await leagueRepo.GetAsync(leagueID);

                // Check if league is full
                if (await leagueUserRepo.CountAsync(leagueID) == league.Size)
                {
                    await e.Channel.SendMessageAsync($"Sorry {e.User.Mention}, that competition is full. 😥");
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
                }

                // Update message
                DiscordEmbed embed = e.Message.Embeds.FirstOrDefault();
                if (embed == null)
                {
                    // Something went wrong. As a backup, send a message confirming the user has been added to the league
                    await e.Channel.SendMessageAsync($"{e.User.Mention} has joined {league.Name_Long}.");
                    return;
                }

                DiscordMember member = await e.Guild.GetMemberAsync(e.User.Id);

                int lastField = embed.Fields.Count - 1;
                string userList = embed.Fields[lastField].Value;
                if (userList == "-")
                {
                    userList = member.Nickname;
                }
                else
                {
                    userList += ", " + member.Nickname;
                }

                var editedAdvert = new DiscordEmbedBuilder(embed)
                    .RemoveFieldAt(lastField)
                    .AddField(embed.Fields[lastField].Name, userList);
                Optional<DiscordEmbed> optionalEmbed = new Optional<DiscordEmbed>(editedAdvert);

                await e.Message.ModifyAsync(embed: optionalEmbed);
            }
        }
    }
}
