﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.Events
{
    public class Event_KnockOn : MatchEvent
    {
        public Event_KnockOn(int minute) : base(minute) { }
    }
}