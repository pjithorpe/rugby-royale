using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
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
            // TODO: Too naive, needs work
            double attPoints = player.Attack + player.Handling * 0.75 + player.Kicking * 0.5;
            double defPoints = player.Defence + player.Physicality * 0.75 + player.Stamina * 0.5;

            if (attPoints >= defPoints + 20)
            {
                return PlayerFocus.Attacking;
            }
            else if (defPoints >= attPoints + 20)
            {
                return PlayerFocus.Defending;
            }

            return PlayerFocus.Versatile;
        }
    }
}