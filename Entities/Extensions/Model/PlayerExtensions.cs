using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RugbyRoyale.Entities.Extensions
{
    public static class PlayerExtensions
    {
        public static bool IsAForward(this Player player)
        {
            return player.Positions_Primary.Any(x => x.IsForward());
        }

        public static bool IsABack(this Player player)
        {
            return player.Positions_Primary.Any(x => x.IsBack());
        }

        public static int TotalStats(this Player player)
        {
            return player.Attack + player.Defence + player.Physicality + player.Stamina + player.Handling + player.Kicking;
        }

        public static void AddPrimaryPosition(this Player player, Position position)
        {
            List<Position> newPositions = player.Positions_Primary;
            newPositions.Add(position);
            player.Positions_Primary = newPositions;
        }

        public static void AddSecondaryPosition(this Player player, Position position)
        {
            List<Position> newPositions = player.Positions_Secondary;
            newPositions.Add(position);
            player.Positions_Secondary = newPositions;
        }

        public static PlayerFocus CalculateFocus(this Player player)
        {
            // Significant difference between attack and defence stats
            if (player.Attack >= player.Defence + 20)
            {
                return PlayerFocus.Attacking;
            }
            else if (player.Defence >= player.Attack + 20)
            {
                return PlayerFocus.Defending;
            }

            // Nuanced difference
            var allStats = new int[] { player.Attack, player.Defence, player.Physicality, player.Stamina, player.Handling, player.Kicking };

            // Normalised
            int weakestSkill = allStats.Min();
            int normalisedAtt = player.Attack - weakestSkill;
            int normalisedDef = player.Defence - weakestSkill;
            int normalisedPhy = player.Physicality - weakestSkill;
            int normalisedSta = player.Stamina - weakestSkill;
            int normalisedHan = player.Handling - weakestSkill;
            int normalisedKic = player.Kicking - weakestSkill;

            // Mean deviations
            double meanSkill = allStats.Average();
            double meanDevAtt = player.Attack - meanSkill;
            double meanDevDef = player.Defence - meanSkill;
            double meanDevPhy = player.Physicality - meanSkill;
            double meanDevSta = player.Stamina - meanSkill;
            double meanDevHan = player.Handling - meanSkill;
            double meanDevKic = player.Kicking - meanSkill;

            double attackingScore = meanDevAtt;
            double defendingScore = meanDevDef;

            // Physicality adds to good attack or defence
            if ((meanDevDef < -5 && meanDevAtt > 5) || (meanDevAtt > meanSkill && player.Attack >= player.Defence + 15))
            {
                // If physicality is good, it assists attack
                if (meanDevPhy > 0 || player.Physicality > player.Attack)
                {
                    attackingScore += meanDevPhy;
                }
            }
            else if ((meanDevAtt < -5 && meanDevDef > 5) || (meanDevDef > meanSkill && player.Defence >= player.Attack + 15))
            {
                // If physicality is good, it assists defence
                if (meanDevPhy > 0 || player.Physicality > player.Defence)
                {
                    defendingScore += meanDevPhy;
                }
            }

            // Kicking adds to good attack or defence
            if ((meanDevDef < -5 && meanDevAtt > 5) || (meanDevAtt > meanSkill && player.Attack >= player.Defence + 15))
            {
                // If physicality is good, it assists attack
                if (meanDevKic > 0 || player.Kicking > player.Attack)
                {
                    attackingScore += meanDevKic;
                }
            }
            else if ((meanDevAtt < -5 && meanDevDef > 5) || (meanDevDef > meanSkill && player.Defence >= player.Attack + 15))
            {
                // If physicality is good, it assists defence
                if (meanDevKic > 0 || player.Kicking > player.Defence)
                {
                    defendingScore += meanDevKic;
                }
            }

            defendingScore += meanDevSta;
            attackingScore += meanDevHan;

            if (defendingScore > attackingScore * 1.25)
            {
                return PlayerFocus.Defending;
            }
            else if (attackingScore > defendingScore * 1.25)
            {
                return PlayerFocus.Defending;
            }

            return PlayerFocus.Versatile;
        }
    }
}