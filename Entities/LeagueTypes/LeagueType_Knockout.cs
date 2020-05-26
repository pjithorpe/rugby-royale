using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public class LeagueType_Knockout : LeagueType
    {
        public override string Name => "Straight Knockout";
        public override string Description => "TODO";

        public override int MinSize => 8;
        public override int MaxSize => 64;

        public override Enums.LeagueType Enumerate() => Enums.LeagueType.Knockout;
    }
}
