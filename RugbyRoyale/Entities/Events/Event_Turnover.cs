using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Turnover : MatchEvent
    {
        public Event_Turnover(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Turnover"; }
    }
}