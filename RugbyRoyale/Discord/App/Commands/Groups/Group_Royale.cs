using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using RugbyRoyale.Data.Repository;
using RugbyRoyale.Discord.App.Attributes;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using Serilog;
using System;
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
        private MessageTracker msgTracker;
        private IRepositoryCollection repos;

        public Group_Royale(IClient gameClient, Settings appSettings, MessageTracker messageTracker, IRepositoryCollection repositories)
        {
            client = gameClient;
            settings = appSettings;
            msgTracker = messageTracker;
            repos = repositories;
        }

        [Command("newteam"), Aliases("nt")]
        public async Task New(CommandContext context)
        {
            await Royale_NewTeam.ExecuteAsync(context, settings, repos);
        }

        [Command("newleague"), Aliases("nl")]
        public async Task NewLeague(CommandContext context)
        {
            await Royale_NewLeague.ExecuteAsync(context, settings, msgTracker, repos);
        }

        [Command("myleague"), Aliases("ml")]
        public async Task MyLeague(CommandContext context)
        {
            await Royale_MyLeague.ExecuteAsync(context, settings, msgTracker, repos);
        }

        /* For dev purposes */
        [Command("test")]
        public async Task Test(CommandContext context)
        {
            Log.Verbose("Test trace level info.");
            Log.Debug("A debug message.");
            Log.Information("An info message.");
            Log.Warning("Uuuh, you should probably change this...");
            Log.Fatal(new NullReferenceException("This is a null ref exception."), "OH SHIT, IT'S ALL ON FIRE!!!");
            try
            {
                await context.RespondAsync("running broken function...");
                BreakingFunction();
            }
            catch (Exception e)
            {
                Log.Error(e, "Custom message");
            }

            void BreakingFunction()
            {
                MajorFunction();
            }
            void MajorFunction()
            {
                try
                {
                    BreakingFunction3();
                }
                catch (Exception e)
                {
                    throw new Exception("Something went wrong in Function 2.", e);
                }
            }
            void BreakingFunction3()
            {
                BreakingFunction4();
            }
            void BreakingFunction4()
            {
                throw new NullReferenceException("A thing caused an exception.");
            }
        }

        [Command("generate-team")]
        public async Task GenerateTeam(CommandContext context)
        {
            await context.RespondAsync($"Generating random team...");

            var nationalities = new Nationality[]
            {
                Nationality.Argentine, Nationality.English, Nationality.French, Nationality.SouthAfrican, Nationality.Georgian,
                Nationality.Irish, Nationality.Scottish, Nationality.Japanese, Nationality.Italian, Nationality.NewZealander,
                Nationality.Russian, Nationality.Welsh, Nationality.Romanian
            };

            var testGen = new PlayerGenerator(50, nationalities[new Random().Next(0, nationalities.Length)]);
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
            foreach (Player player in testTeam.GetPlayers())
            {
                output += $"{number}. {player.FirstName} {player.LastName}";
                output += $"\n   Primary: {string.Join(' ', player.Positions_Primary.Select(p => p.ToString()))}";
                output += $"\n   Secondary: {string.Join(' ', player.Positions_Secondary.Select(p => p.ToString()))}";
                output += $"\n   Focus: {player.Focus}";
                output += $"\n   Stats: ATT {player.Attack}   DEF {player.Defence}   STA {player.Stamina}   PHY {player.Physicality}   HAN {player.Handling}   KIC {player.Kicking}\n";
                number++;
            }

            await context.RespondAsync(output.Substring(0, 2000));
        }
        /**/
    }
}