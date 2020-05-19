using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_ForwardPass : MatchEvent
    {
        public Event_ForwardPass(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Conversion Attempt"; }
    }
}