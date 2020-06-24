using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using RugbyRoyale.Discord.App.Attributes;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    [Group("royale")]
    [MainChannel]
    public class Group_Royale : BaseCommandModule
    {
        [Command("new"), Aliases("newteam", "nt")]
        public async Task New(CommandContext context)
        {
            await context.RespondAsync($"👋 Hi, {context.User.Mention}!");
        }

        [Command("newleague"), Aliases("nl")]
        public async Task NewLeague(CommandContext context)
        {
            InteractivityExtension interactivity = context.Client.GetInteractivity();
            DiscordDmChannel dmChannel = await context.Member.CreateDmChannelAsync();
            InteractivityResult<DiscordMessage> result;

            // Long name
            await dmChannel.SendMessageAsync("Please respond with the **full name** of the new league (max. 40 characters, e.g. The Gallagher Premiership):");
            result = await dmChannel.GetNextMessageAsync();

            if (!await result.CheckValid(dmChannel, 40)) return;
            string longName = result.Result.Content.Trim();

            // Short name
            await dmChannel.SendMessageAsync("Thanks! Now respond with a shortened version of the league's name (max. 10 characters:");
            result = await dmChannel.GetNextMessageAsync();

            if (!await result.CheckValid(dmChannel, 15)) return;
            string shortName = result.Result.Content.Trim();

            var newLeague = new League()
            {
                Name_Long = longName,
                Name_Short = shortName
            };
        }

        /* For dev purposes
        [Command("test")]
        public async Task Test(CommandContext context)
        {
            await context.RespondAsync($"Testing...");
            var testGen = new PlayerGenerator(50);
            var testPlayer = await testGen.GeneratePlayer(Position.FlyHalf);
            await context.RespondAsync($"Done!");
        }
        */
    }
}