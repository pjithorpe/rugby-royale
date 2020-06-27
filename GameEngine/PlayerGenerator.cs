using Redzen.Numerics.Distributions.Double;
using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.GameEngine
{
    public class PlayerGenerator
    {
        private double baseRating;
        private double stdDev = Configuration.PLAYER_GEN_STD_DEV;
        private Random rand;

        public PlayerGenerator(double basePlayerRating)
        {
            baseRating = basePlayerRating;

            rand = new Random();
        }

        public async Task<Player> GeneratePlayer(Position position)
        {
            var player = new Player();

            player = await GenerateName(player);
            player = PositionsGeneration.AddPositions(player, position, rand);
            player = GenerateStats(player);

            player.Focus = player.CalculateFocus();

            return player;
        }

        private async Task<Player> GenerateName(Player player)
        {
            // TODO: nationality

            return await NameGeneration.GenerateRandomName(player, Nationality.French);
        }

        private Player GenerateStats(Player player)
        {
            // Fast normal distribution sampling algorithm
            var distribution = new ZigguratGaussianSampler(baseRating, stdDev);

            player.Attack = Convert.ToInt32(distribution.Sample());
            player.Defence = Convert.ToInt32(distribution.Sample());
            player.Physicality = Convert.ToInt32(distribution.Sample());
            player.Stamina = Convert.ToInt32(distribution.Sample());
            player.Handling = Convert.ToInt32(distribution.Sample());
            player.Kicking = Convert.ToInt32(distribution.Sample());

            // Check if player is overpowered
            if (player.TotalStats() > (baseRating + stdDev) * 6)
            {
                return GenerateStats(player);
            }
            // or underpowered
            else if (player.TotalStats() < (baseRating - stdDev) * 6)
            {
                return GenerateStats(player);
            }

            return player;
        }
    }
}