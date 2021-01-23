using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RugbyRoyale.Data.Repository;
using RugbyRoyale.Discord.App.Attributes;
using RugbyRoyale.GameEngine;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    [Group("match")]
    public class Group_Match : BaseCommandModule
    {
        private IClient client;
        private MatchCoordinator coordinator;

        public Group_Match(IClient gameClient, MatchCoordinator matchCoordinator)
        {
            client = gameClient;
            coordinator = matchCoordinator;
        }

        [Command("start"), Aliases("st")]
        [MainChannel]
        public async Task Start(CommandContext context, DiscordMember opponent)
        {
            await Match_Start.ExecuteAsync(context, opponent, client, coordinator);
        }
    }
}