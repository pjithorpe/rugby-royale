using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Turnover : MatchEvent
    {
        public Event_Turnover(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Turnover"; }
    }
}