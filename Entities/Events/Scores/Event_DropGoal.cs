using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_DropGoal : MatchEvent, IScoreEvent
    {
        public Event_DropGoal(int minute) : base(minute) { }

        public int Points => 3;
    }
}
