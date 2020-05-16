﻿using RugbyRoyale.Entities.Enums;
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
        private Teamsheet teamHome;
        private Teamsheet teamAway;
        private List<MatchEvent> orderedMatchEvents;
        private Random randomGenerator;

        public MatchSimulator(Teamsheet home, Teamsheet away)
        {
            teamHome = home;
            teamAway = away;
            orderedMatchEvents = new List<MatchEvent>();
            randomGenerator = new Random();
        }

        public async Task<MatchResult> SimulateMatch()
        {
            // Calculate duration of an in-game minute
            double duration = Configuration.MATCH_DURATION / 80;

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(duration);

            // Run a simulated period for every in-game minute
            int minute = 0;
            var simTasks = new Queue<Task>();
            var timer = new Timer((e) =>
            {
                var doSimulatePeriod = new Action<int>(SimulatePeriod);
                simTasks.Enqueue(Task.Run(() => doSimulatePeriod(minute)));
                minute++;
            },
            null, startTimeSpan, periodTimeSpan);

            return null;
        }

        private void SimulatePeriod(int minute)
        {
            var eventsInPeriod = new List<MatchEvent>();

            MatchEvent lastEvent = orderedMatchEvents.Last();

            // Try

            // Penalty

            // Penalty Try

            // Drop Goal

            // Knock On

            // Forward Pass

            // Free Kick
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