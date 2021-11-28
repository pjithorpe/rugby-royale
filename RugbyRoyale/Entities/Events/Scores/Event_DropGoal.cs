using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_DropGoal : MatchEvent, IScoreEvent
    {
        public Event_DropGoal(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Drop Goal Attempt"; }

        public string Abbreviation => "DROP";
        public int Points => 3;
        public bool Successful { get; set; }
    }
}