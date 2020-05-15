using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyTry : MatchEvent, IScoreEvent
    {
        public int Points => 7;
    }
}
