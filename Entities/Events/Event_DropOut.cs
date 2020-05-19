using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_DropOut : MatchEvent
    {
        public Event_DropOut(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Conversion Attempt"; }
    }
}