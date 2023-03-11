using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyTry : IMatchEventType, IScoreEvent
    {
        const string _name = "Penalty Try";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The referee awards a penalty try!" };

        public string Abbreviation => "PEN TRY";
        public int Points => 7;
        public bool Successful { get; set; }
    }
}