using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyGoal : IMatchEventType, IScoreEvent
    {
        public string DisplayName => "Penalty Attempt";
        public string[] EventMessages => throw new NotImplementedException();

        public string Abbreviation => "PEN";
        public int Points => 3;
        public bool Successful { get; set; }
    }
}