using RugbyRoyale.Entities.Events;
using System;

namespace RugbyRoyale.GameEngine.Modules
{
    internal static class Events
    {
        public static MatchEvent GetNextEvent(MatchEvent previousEvent, Random randomGenerator)
        {
            IMatchEventType eventType;
            Range secondsRange;

            // Check last event and use it to inform this event
            if (previousEvent.EventType is Event_Try)
            {
                eventType = new Event_Conversion();
                secondsRange = new Range(10, 90);
            }
            else if (previousEvent.EventType is IScoreEvent scoreEvent) // Any score other than a try
            {
                if (scoreEvent.Successful || scoreEvent is Event_Conversion)
                {
                    eventType = new Event_Restart();
                    secondsRange = new Range(10, 30);
                }
                else
                {
                    eventType = new Event_DropOut();
                    secondsRange = new Range(5, 30);
                }
            }
            else if (previousEvent.EventType is Event_KnockOn || previousEvent.EventType is Event_ForwardPass)
            {
                eventType = new Event_Scrum();
                secondsRange = new Range(30, 60);
            }
            else
            {
                eventType = new Event_FinalWhistle();
                secondsRange = new Range(60, 120);
            }

            int seconds = previousEvent.Second + randomGenerator.Next(secondsRange.Start.Value, secondsRange.End.Value);

            return new MatchEvent(eventType, seconds);
        }

        public static bool IsHalting(MatchEvent matchEvent)
        {
            if (matchEvent.EventType is IScoreEvent)
            {
                return true;
            }

            return false;
        }
    }
}