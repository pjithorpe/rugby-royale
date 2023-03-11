using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_DropGoal : IMatchEventType, IScoreEvent
    {
        const string _name = "Drop Goal Attempt";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "They try for a drop goal!" };

        public string Abbreviation => "DROP";
        public int Points => 3;
        public bool Successful { get; set; }
    }
}