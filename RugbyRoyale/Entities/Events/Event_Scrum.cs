using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Scrum : MatchEvent
    {
        public Event_Scrum(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Scrum"; }
    }
}