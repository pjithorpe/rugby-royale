﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_FreeKick : MatchEvent
    {
        public Event_FreeKick(int minute) : base(minute) { }
    }
}