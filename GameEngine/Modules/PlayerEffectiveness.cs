using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RugbyRoyale.GameEngine.Modules
{
    public static class PlayerEffectiveness
    {
        public static float CalculateEffectiveness(Player player, Position position)
        {
            // Playing in one of their primary positions
            if (ContainsPositionOrAlternative(player.Positions_Primary, position))
            {
                return 1.0f;
            }
            // Playing in one of their secondary positions
            else if (ContainsPositionOrAlternative(player.Positions_Secondary, position))
            {
                return 0.9f;
            }
            // Playing in a common alternative of a primary position
            else if (ContainsPositionOrAlternative(player.Positions_Primary, position, "common"))
            {
                return 0.85f;
            }
            // Playing in a common alternative of a secondary position
            else if (ContainsPositionOrAlternative(player.Positions_Primary, position, "common"))
            {
                return 0.75f;
            }
            // Playing in an uncommon alternative of a primary position
            else if (ContainsPositionOrAlternative(player.Positions_Primary, position, "uncommon"))
            {
                return 0.65f;
            }
            // Playing in an uncommon alternative of a secondary position
            else if (ContainsPositionOrAlternative(player.Positions_Primary, position, "uncommon"))
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

        private static bool ContainsPositionOrAlternative(IEnumerable<Position> positionCollection, Position positionToCheck, string alternativeRarity = null)
        {
            switch (alternativeRarity)
            {
                case "common":
                    return positionCollection.Contains(positionToCheck) || positionCollection.Any(x => x.CommonAlternatives().Contains(positionToCheck));
                case "uncommon":
                    return positionCollection.Contains(positionToCheck) || positionCollection.Any(x => x.UncommonAlternatives().Contains(positionToCheck));
                default:
                    return positionCollection.Contains(positionToCheck) || positionCollection.Any(x => x.Alternatives().Contains(positionToCheck));
            }
        }
    }
}
