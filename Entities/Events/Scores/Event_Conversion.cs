using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Conversion : MatchEvent, IScoreEvent
    {
        public int Points => 2;
    }
}
