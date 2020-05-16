using RugbyRoyale.Entities.Enums;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyAwarded : MatchEvent
    {
        public PenaltyOffence Offence { get; set; }

        public Event_PenaltyAwarded(int minute) : base(minute)
        {
        }
    }
}