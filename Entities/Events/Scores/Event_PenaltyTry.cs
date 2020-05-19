using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyTry : MatchEvent, IScoreEvent
    {
        public Event_PenaltyTry(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Conversion Attempt"; }

        public string Abbreviation => "PEN TRY";
        public int Points => 7;
        public bool Successful { get; set; }
    }
}