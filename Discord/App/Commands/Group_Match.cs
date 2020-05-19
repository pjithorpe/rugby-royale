using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    [Group("match")]
    internal class Group_Match : BaseCommandModule
    {
        private IClient client;
        private MatchCoordinator coordinator;
        private IPlayerRepository playerRepo;

        public Group_Match(IClient gameEngineClient, MatchCoordinator matchCoordinator, IPlayerRepository playerRepository)
        {
            client = gameEngineClient;
            coordinator = matchCoordinator;
        }

        [Command("start"), Aliases("st")]
        public async Task Start(CommandContext context, DiscordMember opponent)
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

            await context.Message.RespondAsync("Did a thing.");
        }
    }
}