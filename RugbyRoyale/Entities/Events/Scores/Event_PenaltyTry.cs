using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyTry : MatchEvent, IScoreEvent
    {
        public Event_PenaltyTry(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Penalty Try"; }

        public string Abbreviation => "PEN TRY";
        public int Points => 7;
        public bool Successful { get; set; }
    }
}