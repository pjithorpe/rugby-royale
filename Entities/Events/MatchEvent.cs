using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public abstract class MatchEvent
    {
        public int Minute { get; set; }
    }
}
