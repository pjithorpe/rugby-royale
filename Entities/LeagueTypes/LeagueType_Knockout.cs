using RugbyRoyale.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public class LeagueType_Knockout : LeagueRules
    {
        public override string Name => "Straight Knockout";
        public override string Description => "TODO";
        public override LeagueType LeagueType => LeagueType.Knockout;

        public override int MinSize => 8;
        public override int MaxSize => 64;
    }
}
