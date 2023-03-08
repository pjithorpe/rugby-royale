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
        private TimeSpan inGameMinuteDuration;

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

            inGameMinuteDuration = CalculateInGameMinuteDuration(Configuration.MATCH_DURATION_MINS);

            InitialiseModifiers(teamHome, teamAway);
        }

        public async Task<MatchResult> SimulateMatch()
        {
            int minute = 0;

            // Perform kickoff
            var kickoffEvent = new MatchEvent(new Event_KickOff(), 0);
            futureEventQueue.Enqueue(kickoffEvent);
            ConsumeMatchEventsForCurrentMinute(minute);

            // Keep generating and outputting match events until match is concluded
            while (eventHistory.Last().EventType is not Event_FinalWhistle)
            {
                // Generate a series of match events ending in a "halting" event
                GenerateMatchEvents();

                // Work out how long the period we will be outputting is
                TimeSpan eventSeriesDuration = CalculateDurationOfNextEventSeries();

                // Each in game minute, send queued events to the client if they occur within that minute
                timer = new Timer((e) =>
                {
                    ConsumeMatchEventsForCurrentMinute(minute);
                    minute++;
                },
                null, TimeSpan.Zero, inGameMinuteDuration);

                // Sleep the thread for the duration of the series of events
                Thread.Sleep(eventSeriesDuration);
                await timer.DisposeAsync();
            }

            return null; //TODO: Output match summary
        }

        /*
         * Keep calulating and adding events until we reach a stopping event or HT or FT
         */

        private void InitialiseModifiers(Teamsheet homeTeam, Teamsheet awayTeam)
        {
            homeTeamEffectiveness = PlayerEffectiveness.CalculateEffectivenessOfTeamsheet(homeTeam);
            awayTeamEffectiveness = PlayerEffectiveness.CalculateEffectivenessOfTeamsheet(awayTeam);
            homeTeamScoringChance = ScoringChance.CalculateTryScoringChanceForTeamsheet(homeTeam);
            awayTeamScoringChance = ScoringChance.CalculateTryScoringChanceForTeamsheet(awayTeam);
        }

        private TimeSpan CalculateInGameMinuteDuration(int matchDurationMinutes) => TimeSpan.FromMinutes(matchDurationMinutes / 80.0);

        private TimeSpan CalculateDurationOfNextEventSeries() => TimeSpan.FromMinutes(futureEventQueue.Last().Minute - eventHistory.Last().Minute);

        private void GenerateMatchEvents()
        {
            // TODO: Use entire match history to inform next event
            // e.g. if a yellow card happens and the player has already had a yellow
            //      card event, then we need to convert it to a red
            MatchEvent previousEvent = eventHistory.Last();

            MatchEvent nextEvent;
            do
            {
                nextEvent = Events.GetNextEvent(previousEvent, randomGenerator);
                futureEventQueue.Enqueue(nextEvent);
                previousEvent = nextEvent;

            } while (!Events.IsHalting(nextEvent));
        }

        private void ConsumeMatchEventsForCurrentMinute(int minute)
        {
            // Check if next queued event is within this game minute
            while (futureEventQueue.Count > 0 && futureEventQueue.TryPeek(out MatchEvent nextEvent) && nextEvent.Minute == minute)
            {
                // If so, dequeue event, send to client to output, and add to match history
                if (futureEventQueue.TryDequeue(out MatchEvent matchEvent))
                {
                    clientInterface.OutputMatchEvent(matchEvent, id);
                    eventHistory.Add(matchEvent);
                }
                else
                {
                    Log.Warning("Failed to dequeue next match event. Match ID: {@matchID}", id);
                }
            }
        }
    }
}