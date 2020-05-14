using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine.Modules;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RugbyRoyale.GameEngine
{
    public class MatchSimulator
    {
        private Teamsheet teamHome;
        private Teamsheet teamAway;
        public MatchSimulator(Teamsheet home, Teamsheet away)
        {
            teamHome = home;
            teamAway = away;
        }

        public async Task<MatchResult> SimulateMatch()
        {
            // Calculate duration of an in-game minute
            double duration = Configuration.MATCH_DURATION / 80;

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(duration);

            // Run simulation for every in-game minute
            var timer = new Timer((e) =>
            {
                SimulatePeriod();
            }, null, startTimeSpan, periodTimeSpan);

            return null;
        }

        private void SimulatePeriod()
        {
            //Match events
        }

        private Dictionary<Position, float> CalculateEffectivenessOfTeamsheet(Teamsheet teamsheet)
        {
            var teamEffectiveness = new Dictionary<Position, float>();

            Dictionary<Position, Player> starters = teamsheet.GetStartersDict();
            foreach (Position position in starters.Keys)
            {
                teamEffectiveness[position] = PlayerEffectiveness.CalculateEffectiveness(starters[position], position);
            }

            return teamEffectiveness;
        }

        private Dictionary<Position, float> CalculateTryScoringChanceForTeamsheet(Teamsheet teamsheet)
        {
            var teamTryChance = new Dictionary<Position, float>();

            Dictionary<Position, Player> starters = teamsheet.GetStartersDict();
            foreach (Position position in starters.Keys)
            {
                teamTryChance[position] = ScoringChance.CalculateTryScoringChance(starters[position], position);
            }

            return teamTryChance;
        }
    }
}
