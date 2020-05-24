using System;
using System.Collections.Generic;
using System.Text;

namespace RugbyRoyale.Entities.LeagueTypes
{
    public class LeagueType_TwoConference : LeagueType
    {
        public override string Name => "Two Conference";
        public override string Description => "Over the course of a season, each team plays all other teams in their conference both home and away, and once against each team in the other conference, either home or away.";
    }
}