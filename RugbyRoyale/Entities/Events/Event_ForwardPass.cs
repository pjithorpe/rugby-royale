using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_ForwardPass : MatchEvent
    {
        public Event_ForwardPass(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Conversion Attempt"; }
    }
}