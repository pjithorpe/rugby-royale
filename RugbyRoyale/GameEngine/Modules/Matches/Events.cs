using RugbyRoyale.Entities.Events;
using System;
using System.Linq;
using System.Reflection;

namespace RugbyRoyale.GameEngine.Modules
{
    internal static class Events
    {
        public static MatchEvent GetRandomEvent()
        {
            Type[] types = AllMatchEventTypes();
            object randomEvent = Activator.CreateInstance(types[new Random().Next(types.Length)]);

            return randomEvent as MatchEvent;
        }

        public static MatchEvent GetNextEvent(MatchEvent previousEvent, Random randomGenerator)
        {
            // Check last event and use it to inform this event
            if (previousEvent is Event_Try)
            {
                return new Event_Conversion(previousEvent.MatchID, previousEvent.Second + randomGenerator.Next(10, 90));
            }
            else if (previousEvent is IScoreEvent scoreEvent) // Any score other than a try
            {
                if (scoreEvent.Successful || scoreEvent is Event_Conversion)
                {
                    return new Event_Restart(previousEvent.MatchID, previousEvent.Second + randomGenerator.Next(10, 30));
                }
                else
                {
                    //TODO: Add goal line dropout
                    return new Event_DropOut(previousEvent.MatchID, previousEvent.Second + randomGenerator.Next(5, 30));
                }
            }
            else if (previousEvent is Event_KnockOn || previousEvent is Event_ForwardPass)
            {
                return new Event_Scrum(previousEvent.MatchID, previousEvent.Second + randomGenerator.Next(30, 60));
            }
            else if (previousEvent is Event_PenaltyAwarded penaltyAwarded)
            {
                // TODO: return penalty decsion event
            }

            return null;
        }

        private static MatchEvent CreateFutureEventInTimeRange(MatchEvent previousEvent, MatchEvent nextEvent, int secondsMin, int secondsMax, Random random)
        {
            nextEvent.Second = previousEvent.Second + random.Next(secondsMin, secondsMax);
            return nextEvent;
        }

        private static Type[] AllMatchEventTypes()
        {
            return Assembly.GetExecutingAssembly().DefinedTypes
                .Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(MatchEvent)))
                .ToArray();
        }
    }
}