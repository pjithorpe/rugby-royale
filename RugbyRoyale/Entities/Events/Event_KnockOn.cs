using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_KnockOn : MatchEvent
    {
        public Event_KnockOn(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Conversion Attempt"; }
    }
}