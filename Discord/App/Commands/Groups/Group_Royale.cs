using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using RugbyRoyale.Discord.App.Attributes;
using RugbyRoyale.Discord.App.Repository;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.LeagueTypes;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Commands
{
    [Group("royale")]
    [MainChannel]
    public class Group_Royale : BaseCommandModule
    {
        private IClient client;
        private Settings settings;
        private ILeagueRepository leagueRepo;

        public Group_Royale(IClient gameClient, Settings appSettings, ILeagueRepository leagueRepository)
        {
            client = gameClient;
            settings = appSettings;
            leagueRepo = leagueRepository;
        }

        [Command("newteam"), Aliases("nt")]
        public async Task New(CommandContext context)
        {
            await Royale_NewTeam.ExecuteAsync(context, settings);
        }

        [Command("newleague"), Aliases("nl")]
        public async Task NewLeague(CommandContext context)
        {
            await Royale_NewLeague.ExecuteAsync(context, settings, leagueRepo);
        }

        [Command("myleague"), Aliases("ml")]
        public async Task MyLeague(CommandContext context)
        {
            await Royale_MyLeague.ExecuteAsync(context, leagueRepo);
        }

        /* For dev purposes */
        [Command("generate-team")]
        public async Task Test(CommandContext context)
        {
            await context.RespondAsync($"Generating random team...");

            var nationalities = new Nationality[]
            {
                Nationality.Argentine, Nationality.English, Nationality.French, Nationality.SouthAfrican, Nationality.Georgian,
                Nationality.Irish, Nationality.Scottish, Nationality.Japanese, Nationality.Italian, Nationality.NewZealander,
                Nationality.Russian, Nationality.Welsh, Nationality.Romanian
            };

            var testGen = new PlayerGenerator(50,  nationalities[new Random().Next(0, nationalities.Length)]);
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
                output += $"\n   Primary: {string.Join(' ', player.Positions_Primary.Select(p => p.ToString()))}";
                output += $"\n   Secondary: {string.Join(' ', player.Positions_Secondary.Select(p => p.ToString()))}";
                output += $"\n   Focus: {player.Focus}";
                output += $"\n   Stats: ATT {player.Attack}   DEF {player.Defence}   STA {player.Stamina}   PHY {player.Physicality}   HAN {player.Handling}   KIC {player.Kicking}\n";
                number++;
            }

            await context.RespondAsync(output);
        }
        /**/
    }
}