using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Conversion : IMatchEventType, IScoreEvent
    {
        const string _name = "Conversion Attempt";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The player lines up the conversion." };

        public string Abbreviation => "CON";
        public int Points => 2;
        public bool Successful { get; set; }
    }
}