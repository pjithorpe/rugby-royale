using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_FreeKick : MatchEvent
    {
        public Event_FreeKick(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Conversion Attempt"; }
    }
}