﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_ForwardPass : MatchEvent
    {
        public Event_ForwardPass(int minute) : base(minute) { }
    }
}
