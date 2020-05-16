using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyGoal : MatchEvent, IScoreEvent
    {
        public Event_PenaltyGoal(int minute) : base(minute) { }

        public int Points => 3;
    }
}
