﻿using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Try : MatchEvent, IScoreEvent
    {
        public Event_Try(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Try"; }

        public string Abbreviation => "TRY";
        public int Points => 5;
        public bool Successful { get; set; }
    }
}