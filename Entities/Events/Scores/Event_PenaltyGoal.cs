using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyGoal : MatchEvent, IScoreEvent
    {
        public Event_PenaltyGoal(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Conversion Attempt"; }

        public string Abbreviation => "PEN";
        public int Points => 3;
        public bool Successful { get; set; }
    }
}