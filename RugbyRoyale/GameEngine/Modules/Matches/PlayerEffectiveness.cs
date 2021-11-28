using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RugbyRoyale.GameEngine.Modules
{
    internal static class PlayerEffectiveness
    {
        public static float CalculateEffectiveness(Player player, Position position)
        {
            // Playing in one of their primary positions
            if (player.Positions_Primary.Contains(position))
            {
                return 1.0f;
            }
            // Playing in one of their secondary positions
            else if (player.Positions_Secondary.Contains(position))
            {
                return 0.9f;
            }
            // Playing in a common alternative of a primary position
            else if (ContainsPositionOrCommonAlternative(player.Positions_Primary, position))
            {
                return 0.85f;
            }
            // Playing in a common alternative of a secondary position
            else if (ContainsPositionOrCommonAlternative(player.Positions_Primary, position))
            {
                return 0.75f;
            }
            // Playing in an uncommon alternative of a primary position
            else if (ContainsPositionOrUncommonAlternative(player.Positions_Primary, position))
            {
                return 0.65f;
            }
            // Playing in an uncommon alternative of a secondary position
            else if (ContainsPositionOrUncommonAlternative(player.Positions_Primary, position))
            {
                return 0.55f;
            }
            // At least playing in the right half of the team
            else if (position.IsForward() && player.IsAForward() || position.IsBack() && player.IsABack())
            {
                return 0.5f;
            }
            // Playing completely out of position but not in the spine
            else if (!position.IsSpine())
            {
                return 0.3f;
            }
            // Playing completely out of position and in the spine
            return 0.2f;
        }

        public static Dictionary<TeamsheetPosition, float> CalculateEffectivenessOfTeamsheet(Teamsheet teamsheet)
        {
            var teamEffectiveness = new Dictionary<TeamsheetPosition, float>();

            Dictionary<TeamsheetPosition, Player> starters = teamsheet.GetStartersDict();
            foreach (TeamsheetPosition tp in starters.Keys)
            {
                teamEffectiveness[tp] = CalculateEffectiveness(starters[tp], tp.ToPosition());
            }

            return teamEffectiveness;
        }

        private static bool ContainsPositionOrCommonAlternative(IEnumerable<Position> positionCollection, Position positionToCheck)
        {
            return positionCollection.Contains(positionToCheck) || positionCollection.Any(x => x.CommonAlternatives().Contains(positionToCheck));
        }

        private static bool ContainsPositionOrUncommonAlternative(IEnumerable<Position> positionCollection, Position positionToCheck)
        {
            return positionCollection.Contains(positionToCheck) || positionCollection.Any(x => x.UncommonAlternatives().Contains(positionToCheck));
        }
    }
}