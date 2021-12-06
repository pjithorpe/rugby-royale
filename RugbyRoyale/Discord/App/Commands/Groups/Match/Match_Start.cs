using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using Serilog;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    static class Match_Start
    {
        public static async Task ExecuteAsync(CommandContext context, DiscordMember opponent, IClient client, MatchCoordinator coordinator)
        {
            var matchID = Guid.NewGuid();

            if (!coordinator.TryAddMatch(matchID, context.Member, opponent))
            {
                await context.Message.RespondAsync("Couldn't start a new match (one of the players might be already in a match, or the match threads might be full).");
                return;
            }

            /* TEMP: randomly generate teams */
            // home teamsheet
            var testGenHome = new PlayerGenerator(50, Nationality.Welsh);
            Teamsheet home = await testGenHome.GenerateTeamsheet();

            // away teamsheet
            var testGenAway = new PlayerGenerator(50, Nationality.Scottish);
            Teamsheet away = await testGenAway.GenerateTeamsheet();
            /* END TEMP */

            var simulator = new MatchSimulator(matchID, home, away, client);
            await simulator.SimulateMatch();

            if (!coordinator.TryRemoveMatch(matchID))
            {
                Log.Warning("Failed to remove match from coordinator");
            }

            await context.Message.RespondAsync("Match concluded.");
        }
    }
}
