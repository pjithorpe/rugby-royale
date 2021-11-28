using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_DropOut : MatchEvent
    {
        public Event_DropOut(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Conversion Attempt"; }
    }
}