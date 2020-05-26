﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public class LeagueType_DoubleRoundRobin : LeagueType
    {
        public override string Name => "Double Round-Robin";
        public override string Description => "Over the course of a season, each team plays all other teams both home and away.";

        public override Enums.LeagueType Enumerate() => Enums.LeagueType.DoubleRoundRobin;
    }
}
