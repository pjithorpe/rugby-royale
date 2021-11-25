using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.Console
{
    class Program
    {
        private static Teamsheet home;
        private static Teamsheet away;

        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Randomising teamsheets...");

            home = await new PlayerGenerator(50, Nationality.English).GenerateTeamsheet();
            away = await new PlayerGenerator(50, Nationality.Welsh).GenerateTeamsheet();

            while (true)
            {
                System.Console.WriteLine("RugbyRoyale CLI Match Simulator" +
                    "\n\n1 - Simulate a match" +
                    "\n2 - Alter team data" +
                    "\n3 - Settings" +
                    "\n4 - Quit" +
                    "\n");

                ConsoleKey input = System.Console.ReadKey().Key;
                System.Console.WriteLine();

                switch (input)
                {
                    case ConsoleKey.D1:
                        SimulateAMatch();
                        break;
                    case ConsoleKey.D2:
                        ShowTeamData();
                        //EditTeamData();
                        break;
                    case ConsoleKey.D3:
                        //EditSettings();
                        break;
                    case ConsoleKey.D4:
                        Environment.Exit(0);
                        break;
                    default:
                        System.Console.WriteLine("Unrecognised response.");
                        break;
                }
            }
        }

        private static async void SimulateAMatch()
        {
            var simulator = new MatchSimulator(new Guid(), home, away, new Client());
            await simulator.SimulateMatch();

            System.Console.WriteLine("Match concluded.");
        }

        private static void ShowTeamData()
        {
            System.Console.WriteLine($"HOME TEAM: \n\n {home}");
            System.Console.WriteLine($"AWAY TEAM: \n\n {away}");
        }

        private static void EditTeamData()
        {
            //TODO
            throw new NotImplementedException();
        }
        private static void EditSettings()
        {
            //TODO
            throw new NotImplementedException();
        }

    }
}
