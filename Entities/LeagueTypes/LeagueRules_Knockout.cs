using RugbyRoyale.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public class LeagueRules_Knockout : LeagueRules
    {
        public override string Name => "Straight Knockout";
        public override string Description => "A single elimination tournament in which teams are paired head-to-head in a series of rounds.";
        public override LeagueType LeagueType => LeagueType.Knockout;

        public override int MinSize => 8;
        public override int MaxSize => 64;
    }
}
