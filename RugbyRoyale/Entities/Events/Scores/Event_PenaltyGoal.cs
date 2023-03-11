using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyGoal : IMatchEventType, IScoreEvent
    {
        const string _name = "Penalty Attempt";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The captain points to the posts for a penalty kick." };

        public string Abbreviation => "PEN";
        public int Points => 3;
        public bool Successful { get; set; }
    }
}