using RugbyRoyale.Entities.Enums;
using RugbyRoyale.Entities.Events;
using RugbyRoyale.Entities.Model;
using RugbyRoyale.GameEngine.Modules;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RugbyRoyale.GameEngine
{
    public class MatchSimulator
    {
        // Inputs
        private Guid id;
        private Teamsheet teamHome;
        private Teamsheet teamAway;
        private IClient clientInterface;

        // Local variables
        private List<MatchEvent> eventHistory;
        private Queue<MatchEvent> futureEventQueue;
        private Random randomGenerator;
        private Timer timer;

        // Modifiers
        private Dictionary<TeamsheetPosition, float> homeTeamEffectiveness;
        private Dictionary<TeamsheetPosition, float> awayTeamEffectiveness;
        private Dictionary<TeamsheetPosition, float> homeTeamScoringChance;
        private Dictionary<TeamsheetPosition, float> awayTeamScoringChance;

        public MatchSimulator(Guid matchID, Teamsheet home, Teamsheet away, IClient client)
        {
            id = matchID;
            teamHome = home;
            teamAway = away;
            clientInterface = client;

            eventHistory = new List<MatchEvent>();
            futureEventQueue = new Queue<MatchEvent>();
            randomGenerator = new Random();

            InitialiseModifiers();
        }

        public async Task<MatchResult> SimulateMatch()
        {
            // Calculate duration of an in-game minute
            double duration = Configuration.MATCH_DURATION_MINS / 80.0;

            var startTimeSpan = TimeSpan.Zero;
            var matchTimeSpan = TimeSpan.FromMinutes(Configuration.MATCH_DURATION_MINS);
            var periodTimeSpan = TimeSpan.FromMinutes(duration);

            // Generate a series of match events ending in a "halting" event
            GenerateMatchEvents();

            // Each in game minute, send queued events to the client
            int minute = 0;
            timer = new Timer((e) =>
            {
                OutputMatchEventsForCurrentMinute(minute);
                minute++;
            },
            null, startTimeSpan, periodTimeSpan);

            Thread.Sleep(matchTimeSpan);
            await timer.DisposeAsync();

            return null;
        }

        /*
         * Generate entire match future immediately?
         * Some indication ona match event whether it is a "stopping" event which requires user input
         * Keep calulating and adding events until we reach a stopping event or HT or FT
         * 
         */

        private void InitialiseModifiers()
        {
            homeTeamEffectiveness = PlayerEffectiveness.CalculateEffectivenessOfTeamsheet(teamHome);
            awayTeamEffectiveness = PlayerEffectiveness.CalculateEffectivenessOfTeamsheet(teamAway);
            homeTeamScoringChance = ScoringChance.CalculateTryScoringChanceForTeamsheet(teamHome);
            awayTeamScoringChance = ScoringChance.CalculateTryScoringChanceForTeamsheet(teamHome);
        }

        private void GenerateMatchEvents()
        {
            MatchEvent previousEvent = eventHistory.Last();

            MatchEvent nextEvent;
            do
            {
                nextEvent = Events.GetNextEvent(previousEvent, randomGenerator);
            } while (!nextEvent.IsHalting);
        }

        private void OutputMatchEventsForCurrentMinute(int minute)
        {
            if (futureEventQueue.Count > 0)
            {
                if (futureEventQueue.TryPeek(out MatchEvent nextEvent))
                {
                    // Check if next queued event is within this game minute
                    while (nextEvent.Minute == minute)
                    {
                        if (futureEventQueue.TryDequeue(out MatchEvent matchEvent))
                        {
                            clientInterface.OutputMatchEvent(matchEvent);
                        }
                        else
                        {
                            Log.Warning("Failed to dequeue next match event. Match ID: {@matchID}", id);
                        }
                    }
                }
                else
                {
                    Log.Warning("Failed to peek at next match event. Match ID: {@matchID}", id);
                }
            }
            else
            {
                Log.Warning("Expected another match event in the queue, but queue was empty. Match ID: {@matchID}", id);
            }
        }

        //private void SimulatePeriod(int minute)
        //{
        //    var eventsInPeriod = new List<MatchEvent>();
        //    // Look at history, teamsheets, effectiveness, scoring chances to inform next event
        //    MatchEvent previousEvent = null; // Null if first event
        //    if (eventsInPeriod.Count > 0)
        //    {
        //        previousEvent = eventsInPeriod.Last();
        //    }
        //    else if (eventHistory.Count > 0)
        //    {
        //        previousEvent = eventHistory.Last();
        //    }

        //    MatchEvent nextEvent = null;
        //    // First, check if we must constrict the set of possible next events based on the last event
        //    if (previousEvent != null)
        //    {
        //        nextEvent = Events.GetNextEventFromPrevious(previousEvent, minute, randomGenerator);
        //    }
        //}
    }
}