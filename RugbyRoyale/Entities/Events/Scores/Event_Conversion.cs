using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Conversion : IMatchEventType, IScoreEvent
    {
        public string DisplayName { get => "Conversion Attempt"; }
        public string[] EventMessages => throw new NotImplementedException();

        public string Abbreviation => "CON";
        public int Points => 2;
        public bool Successful { get; set; }
    }
}