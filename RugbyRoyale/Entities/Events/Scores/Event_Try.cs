using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Try : IMatchEventType, IScoreEvent
    {
        const string _name = "Try";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "A great try!", "They scored a try!" };

        public string Abbreviation => "TRY";
        public int Points => 5;
        public bool Successful { get; set; }
    }
}