using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Restart : MatchEvent
    {
        public Event_Restart(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Restart"; }
    }
}