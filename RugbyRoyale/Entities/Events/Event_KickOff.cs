using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_KickOff : MatchEvent
    {
        public Event_KickOff(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Kick Off"; }
    }
}