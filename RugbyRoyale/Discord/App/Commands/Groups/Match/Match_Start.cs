using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
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

            // home teamsheet
            var home = new Teamsheet();

            // away teamsheet
            var away = new Teamsheet();

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
