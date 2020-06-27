using Redzen.Numerics.Distributions.Double;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.GameEngine.Modules
{
    internal static class StatsGeneration
    {
        private static double stdDev = Configuration.PLAYER_GEN_STD_DEV;

        public static Player GenerateStats(Player player, int baseRating)
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
                return GenerateStats(player, baseRating);
            }
            // or underpowered
            else if (player.TotalStats() < (baseRating - stdDev) * 6)
            {
                return GenerateStats(player, baseRating);
            }

            return player;
        }
    }
}
