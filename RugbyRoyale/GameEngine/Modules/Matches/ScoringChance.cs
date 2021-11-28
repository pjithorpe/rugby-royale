using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;

namespace RugbyRoyale.GameEngine.Modules
{
    internal static class ScoringChance
    {
        public static float CalculateTryScoringChance(Player player, TeamsheetPosition position)
        {
            return GetTryScoringChanceForPosition(position);
        }

        public static double CalculateConversionSuccessChance()
        {
            return 0.5f;
        }

        public static Dictionary<TeamsheetPosition, float> CalculateTryScoringChanceForTeamsheet(Teamsheet teamsheet)
        {
            var teamTryChance = new Dictionary<TeamsheetPosition, float>();

            Dictionary<TeamsheetPosition, Player> starters = teamsheet.GetStartersDict();
            foreach (TeamsheetPosition ts in starters.Keys)
            {
                teamTryChance[ts] = CalculateTryScoringChance(starters[ts], ts);
            }

            return teamTryChance;
        }

        private static float GetTryScoringChanceForPosition(TeamsheetPosition position)
        {
            switch (position)
            {
                case TeamsheetPosition.LooseheadProp:
                    return 0.13f;

                case TeamsheetPosition.Hooker:
                    return 0.25f;

                case TeamsheetPosition.TightheadProp:
                    return 0.12f;

                case TeamsheetPosition.Number4Lock:
                    return 0.15f;

                case TeamsheetPosition.Number5Lock:
                    return 0.15f;

                case TeamsheetPosition.BlindsideFlanker:
                    return 0.27f;

                case TeamsheetPosition.OpensideFlanker:
                    return 0.33f;

                case TeamsheetPosition.Number8:
                    return 0.4f;

                case TeamsheetPosition.ScrumHalf:
                    return 0.34f;

                case TeamsheetPosition.FlyHalf:
                    return 0.33f;

                case TeamsheetPosition.InsideCentre:
                    return 0.4f;

                case TeamsheetPosition.OutsideCentre:
                    return 0.5f;

                case TeamsheetPosition.LeftWing:
                    return 0.94f;

                case TeamsheetPosition.RightWing:
                    return 0.91f;

                case TeamsheetPosition.FullBack:
                    return 0.57f;

                default:
                    throw new Exception("Try scoring chance not found for position: " + position.ToString());
            }
        }
    }
}