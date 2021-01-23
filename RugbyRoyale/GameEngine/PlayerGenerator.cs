using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine.Modules;
using System;
using System.Threading.Tasks;

namespace RugbyRoyale.GameEngine
{
    public class PlayerGenerator
    {
        private int baseRating;
        private Nationality nat;
        private Random rand;

        public PlayerGenerator(int basePlayerRating, Nationality nationality)
        {
            baseRating = basePlayerRating;
            nat = nationality;

            rand = new Random();
        }

        public async Task<Player> GeneratePlayer(Position position)
        {
            var player = new Player();

            player = await NameGeneration.GenerateRandomName(player, nat);
            player = PositionsGeneration.AddPositions(player, position, rand);
            player = StatsGeneration.GenerateStats(player, baseRating);

            player.Focus = player.CalculateFocus();

            return player;
        }
    }
}