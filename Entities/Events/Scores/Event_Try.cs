﻿namespace RugbyRoyale.Entities.Events
{
    public class Event_Try : MatchEvent, IScoreEvent
    {
        public Event_Try(int minute) : base(minute)
        {
        }

        public int Points => 5;
        public bool Successful { get; set; }
    }
}