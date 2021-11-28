﻿using RugbyRoyale.Entities.Enums;
using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyAwarded : MatchEvent
    {
        public PenaltyOffence Offence { get; set; }

        public Event_PenaltyAwarded(Guid matchID, int second) : base(matchID, second)
        {
        }

        public override string Name { get => "Penalty Awarded"; }
    }
}