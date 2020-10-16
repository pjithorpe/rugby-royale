using RugbyRoyale.Entities.Events;
using System;
using System.Linq;
using System.Reflection;

namespace RugbyRoyale.GameEngine
{
    internal static class Events
    {
        public static MatchEvent GetRandomEvent()
        {
            Type[] types = AllMatchEventTypes();
            object randomEvent = Activator.CreateInstance(types[new Random().Next(types.Length)]);

            return randomEvent as MatchEvent;
        }

        public static MatchEvent GetNextEventFromPrevious(MatchEvent previousEvent, int minute, Random randomGenerator)
        {
            // Check last event and use it to inform this event
            if (previousEvent is Event_Try)
            {
                return new Event_Conversion(previousEvent.MatchID, minute);
            }
            else if (previousEvent is IScoreEvent scoreEvent) // Any score other than a try
            {
                if (scoreEvent.Successful || scoreEvent is Event_Conversion)
                {
                    return new Event_Restart(previousEvent.MatchID, minute);
                }
                else
                {
                    return new Event_DropOut(previousEvent.MatchID, minute);
                }
            }
            else if (previousEvent is Event_KnockOn || previousEvent is Event_ForwardPass)
            {
                return new Event_Scrum(previousEvent.MatchID, minute);
            }
            else if (previousEvent is Event_PenaltyAwarded || previousEvent is Event_DropGoal)
            {
                return new Event_PenaltyGoal(previousEvent.MatchID, minute);
            }

            return null;
        }

        private static Type[] AllMatchEventTypes()
        {
            return Assembly.GetExecutingAssembly().DefinedTypes
                .Where(t => !t.IsAbstract && t.IsClass && t.IsSubclassOf(typeof(MatchEvent)))
                .ToArray();
        }
    }
}