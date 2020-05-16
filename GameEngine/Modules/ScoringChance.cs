using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Model;
using System;

namespace RugbyRoyale.GameEngine
{
    internal static class ScoringChance
    {
        public static float CalculateTryScoringChance(Player player, Position position)
        {
            return GetTryScoringChanceForPosition(position);
        }

        public static double CalculateConversionSuccessChance()
        {
            return 0.5f;
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
    }
}