using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Penalty : MatchEvent
    {
        public Event_Penalty(int minute) : base(minute) { }
    }
}
