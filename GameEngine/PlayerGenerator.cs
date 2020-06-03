using Redzen.Numerics.Distributions.Double;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using System;

namespace RugbyRoyale.GameEngine
{
    public class PlayerGenerator
    {
        private double baseRating;
        private double stdDev = Configuration.PLAYER_GEN_STD_DEV;

        public PlayerGenerator(double basePlayerRating)
        {
            baseRating = basePlayerRating;
        }

        public Player GeneratePlayer()
        {
            var player = new Player();

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
                return GeneratePlayer();
            }
            // or underpowered
            else if (player.TotalStats() < (baseRating - stdDev) * 6)
            {
                return GeneratePlayer();
            }

            return player;
        }
    }
}