using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Try : IMatchEventType, IScoreEvent
    {
        public string DisplayName => "Try";
        public string[] EventMessages => throw new NotImplementedException();

        public string Abbreviation => "TRY";
        public int Points => 5;
        public bool Successful { get; set; }
    }
}