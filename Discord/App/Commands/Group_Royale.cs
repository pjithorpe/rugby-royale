using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using RugbyRoyale.Discord.App.Attributes;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using System.Linq;
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

        /* For dev purposes */
        [Command("generate")]
        public async Task Test(CommandContext context)
        {
            await context.RespondAsync($"Generating random team...");
            var testGen = new PlayerGenerator(50);
            var testTeam = new Teamsheet()
            {
                LooseheadProp = await testGen.GeneratePlayer(Position.Prop),
                Hooker = await testGen.GeneratePlayer(Position.Hooker),
                TightheadProp = await testGen.GeneratePlayer(Position.Prop),
                Number4Lock = await testGen.GeneratePlayer(Position.Lock),
                Number5Lock = await testGen.GeneratePlayer(Position.Lock),
                BlindsideFlanker = await testGen.GeneratePlayer(Position.Flanker),
                OpensideFlanker = await testGen.GeneratePlayer(Position.Flanker),
                Number8 = await testGen.GeneratePlayer(Position.Number8),
                ScrumHalf = await testGen.GeneratePlayer(Position.ScrumHalf),
                FlyHalf = await testGen.GeneratePlayer(Position.FlyHalf),
                InsideCentre = await testGen.GeneratePlayer(Position.Centre),
                OutsideCentre = await testGen.GeneratePlayer(Position.Centre),
                LeftWing = await testGen.GeneratePlayer(Position.Wing),
                RightWing = await testGen.GeneratePlayer(Position.Wing),
                FullBack = await testGen.GeneratePlayer(Position.FullBack),
            };

            string output = "";
            int number = 1;
            foreach(Player player in testTeam.GetPlayers())
            {
                output += $"{number}. {player.FirstName} {player.LastName}";
                output += $"   Primary: {string.Join(' ', player.Positions_Primary.Select(p => p.ToString()))}";
                output += $"   Secondary: {string.Join(' ', player.Positions_Secondary.Select(p => p.ToString()))}";
                output += $"   Stats: ATT {player.Attack}   DEF {player.Defence}   STA {player.Stamina}   PHY {player.Physicality}   HAN {player.Handling}   KIC {player.Kicking}\n";
                number++;
            }

            await context.RespondAsync(output);
        }
        /**/
    }
}