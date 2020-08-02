using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    static class Match_Start
    {
        public static async Task ExecuteAsync(CommandContext context, DiscordMember opponent, IClient client, MatchCoordinator coordinator)
        {
            var matchID = new Guid();

            if (!coordinator.TryAddMatch(matchID, context.Member, opponent))
            {
                await context.Message.RespondAsync("Couldn't start a new match (match threads might be full).");
            }

            // home teamsheet
            var home = new Teamsheet();

            // away teamsheet
            var away = new Teamsheet();

            var simulator = new MatchSimulator(matchID, home, away, client);
            await simulator.SimulateMatch();

            await context.Message.RespondAsync("Did a thing.");
        }
    }
}
