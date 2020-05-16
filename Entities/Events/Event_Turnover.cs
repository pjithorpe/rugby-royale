using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Turnover : MatchEvent
    {
        public Event_Turnover(int minute) : base(minute) { }
    }
}
