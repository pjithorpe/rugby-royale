using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine.Modules;
using System.Collections.Generic;
using System.Linq;

namespace RugbyRoyale.GameEngine
{
    public class MatchSimulator
    {
        public Teamsheet teamHome;
        public Teamsheet teamAway;
        public MatchSimulator(Teamsheet home, Teamsheet away)
        {
            teamHome = home;
            teamAway = away;
        }

        public MatchResult Simulate()
        {
            Dictionary<Position, float> homeEffectiveness = CalculateEffectivenessOfTeamsheet(teamHome);
            Dictionary<Position, float> awayEffectiveness = CalculateEffectivenessOfTeamsheet(teamAway);

            return null;
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
    }
}
