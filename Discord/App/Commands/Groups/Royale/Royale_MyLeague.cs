using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Entities.Model;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    static class Royale_MyLeague
    {
        public static async Task ExecuteAsync(CommandContext context, ILeagueRepository leagueRepo)
        {
            League league = await leagueRepo.GetAsync(context.User.Id.ToString());

            // If not yet started, advertise for people to join
            if (league.HasStarted)
            {
                //TODO
                var leagueAdvert = new DiscordEmbedBuilder()
                {
                    Title = "A New Competition",
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
    }
}
