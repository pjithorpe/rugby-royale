using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_KnockOn : MatchEvent
    {
        public Event_KnockOn(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Conversion Attempt"; }
    }
}