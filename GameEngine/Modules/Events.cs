using RugbyRoyale.Entities.Events;
using System;

namespace RugbyRoyale.GameEngine
{
    internal static class Events
    {
        public static MatchEvent GetNextEventFromPrevious(MatchEvent previousEvent, int minute, Random randomGenerator)
        {
            // Check last event and use it to inform this event
            if (previousEvent is Event_Try)
            {
                var conversionEvent = new Event_Conversion(previousEvent.MatchID, minute);

                if (randomGenerator.NextDouble() <= ScoringChance.CalculateConversionSuccessChance())
                {
                    conversionEvent.Successful = true;
                }
                else
                {
                    conversionEvent.Successful = false;
                }

                return conversionEvent;
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
    }
}