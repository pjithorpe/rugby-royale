using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Lineout : MatchEvent
    {
        public Event_Lineout(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Conversion Attempt"; }
    }
}