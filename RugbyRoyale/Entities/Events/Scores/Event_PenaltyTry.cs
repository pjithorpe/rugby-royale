using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyTry : IMatchEventType, IScoreEvent
    {
        public string DisplayName => "Penalty Try";
        public string[] EventMessages => throw new NotImplementedException();

        public string Abbreviation => "PEN TRY";
        public int Points => 7;
        public bool Successful { get; set; }
    }
}