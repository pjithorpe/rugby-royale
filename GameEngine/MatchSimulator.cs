using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Events;
using RugbyRoyale.Entities.Extensions;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RugbyRoyale.GameEngine
{
    public class MatchSimulator
    {
        private Guid id;
        private Teamsheet teamHome;
        private Teamsheet teamAway;
        private IClient clientInterface;

        private List<MatchEvent> matchHistory;
        private Random randomGenerator;
        private Timer timer;

        public MatchSimulator(Guid matchID, Teamsheet home, Teamsheet away, IClient client)
        {
            id = matchID;
            teamHome = home;
            teamAway = away;
            clientInterface = client;
            matchHistory = new List<MatchEvent>();
            randomGenerator = new Random();
        }

        public async Task<MatchResult> SimulateMatch()
        {
            // Calculate duration of an in-game minute
            double duration = Configuration.MATCH_DURATION_MINS / 80.0;

            var startTimeSpan = TimeSpan.Zero;
            var matchTimeSpan = TimeSpan.FromMinutes(Configuration.MATCH_DURATION_MINS);
            var periodTimeSpan = TimeSpan.FromMinutes(duration);

            // Run a simulated period for every in-game minute
            int minute = 0;
            timer = new Timer((e) =>
            {
                SimulatePeriod(minute);
                minute++;
            },
            null, startTimeSpan, periodTimeSpan);

            Thread.Sleep(matchTimeSpan);
            await timer.DisposeAsync();

            return null;
        }

        private void SimulatePeriod(int minute)
        {
            var eventsInPeriod = new List<MatchEvent>();
            // Look at history, teamsheets, effectiveness, scoring chances to inform next event
            MatchEvent previousEvent = null; // Null if first event
            if (eventsInPeriod.Count > 0)
            {
                previousEvent = eventsInPeriod.Last();
            }
            else if (matchHistory.Count > 0)
            {
                previousEvent = matchHistory.Last();
            }

            MatchEvent nextEvent = null;
            // First, check if we must constrict the set of possible next events based on the last event
            if (previousEvent != null)
            {
                nextEvent = Events.GetNextEventFromPrevious(previousEvent, minute, randomGenerator);
            }

            if (nextEvent != null)
            {
                matchHistory.Add(nextEvent);
                clientInterface.OutputMatchEvent(nextEvent);
            }
        }

        private Dictionary<TeamsheetPosition, float> CalculateEffectivenessOfTeamsheet(Teamsheet teamsheet)
        {
            var teamEffectiveness = new Dictionary<TeamsheetPosition, float>();

            Dictionary<TeamsheetPosition, Player> starters = teamsheet.GetStartersDict();
            foreach (TeamsheetPosition tp in starters.Keys)
            {
                teamEffectiveness[tp] = PlayerEffectiveness.CalculateEffectiveness(starters[tp], tp.ToPosition());
            }

            return teamEffectiveness;
        }

        private Dictionary<TeamsheetPosition, float> CalculateTryScoringChanceForTeamsheet(Teamsheet teamsheet)
        {
            var teamTryChance = new Dictionary<TeamsheetPosition, float>();

            Dictionary<TeamsheetPosition, Player> starters = teamsheet.GetStartersDict();
            foreach (TeamsheetPosition ts in starters.Keys)
            {
                teamTryChance[ts] = ScoringChance.CalculateTryScoringChance(starters[ts], ts);
            }

            return teamTryChance;
        }
    }
}