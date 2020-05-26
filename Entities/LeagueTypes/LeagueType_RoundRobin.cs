using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public class LeagueType_RoundRobin : LeagueType
    {
        public override string Name => "Round-Robin";
        public override string Description => "Over the course of a season, each team plays all other teams once, either home or away.";

        public override int MaxSize => 24;

        public override Enums.LeagueType Enumerate() => Enums.LeagueType.RoundRobin;
    }
}
