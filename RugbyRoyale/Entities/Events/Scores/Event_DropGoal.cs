using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_DropGoal : IMatchEventType, IScoreEvent
    {
        public string DisplayName => "Drop Goal Attempt";
        public string[] EventMessages => throw new NotImplementedException();

        public string Abbreviation => "DROP";
        public int Points => 3;
        public bool Successful { get; set; }
    }
}