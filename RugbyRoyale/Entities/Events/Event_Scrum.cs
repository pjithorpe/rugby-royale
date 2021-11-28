using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Scrum : MatchEvent
    {
        public Event_Scrum(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Scrum"; }
    }
}