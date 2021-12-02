using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_FinalWhistle : MatchEvent
    {
        public override bool IsHalting => true;

        public Event_FinalWhistle(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Final Whistle"; }
    }
}