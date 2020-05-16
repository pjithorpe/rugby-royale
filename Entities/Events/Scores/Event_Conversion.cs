using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Conversion : MatchEvent, IScoreEvent
    {
        public bool Successful { get; set; }

        public Event_Conversion(int minute) : base(minute) { }

        public int Points => 2;
    }
}
