using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RugbyRoyale.GameEngine.Modules
{
    public static class ScoringChance
    {
        public static float CalculateTryScoringChance(Player player, Position position)
        {
            float scoringChance = GetTryScoringChanceForPosition(position);

            scoringChance = scoringChance * GetBadgeModifier(player);

            return scoringChance;
        }

        private static float GetTryScoringChanceForPosition(Position position)
        {
            switch (position)
            {
                case Position.LooseheadProp:
                    return 0.13f;
                case Position.Hooker:
                    return 0.25f;
                case Position.TightheadProp:
                    return 0.12f;
                case Position.Number4Lock:
                    return 0.15f;
                case Position.Number5Lock:
                    return 0.15f;
                case Position.BlindsideFlanker:
                    return 0.27f;
                case Position.OpensideFlanker:
                    return 0.33f;
                case Position.Number8:
                    return 0.4f;
                case Position.ScrumHalf:
                    return 0.34f;
                case Position.FlyHalf:
                    return 0.33f;
                case Position.InsideCentre:
                    return 0.4f;
                case Position.OutsideCentre:
                    return 0.5f;
                case Position.LeftWing:
                    return 0.94f;
                case Position.RightWing:
                    return 0.91f;
                case Position.FullBack:
                    return 0.57f;
                default:
                    throw new Exception("Try scoring chance not found for position: " + position.ToString());
            }
        }

        private static float GetBadgeModifier(Player player)
        {
            float modifier = 1.0f;
            List<PlayerBadge> badgeCollection = player.Badges;

            if (badgeCollection.Contains(PlayerBadge.Finisher))
            {
                modifier = 1.2f;
            }

            if (badgeCollection.Contains(PlayerBadge.Magician))
            {
                modifier *= 1.05f;
            }

            if (badgeCollection.Contains(PlayerBadge.Fast))
            {
                modifier *= 1.1f;
            }

            return modifier;
        }
    }
}
