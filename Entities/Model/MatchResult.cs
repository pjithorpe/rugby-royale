using RugbyRoyale.Entities.Enums;
using System;

namespace RugbyRoyale.Entities.Model
{
    public class MatchResult
    {
        public Guid MatchResultID { get; set; }
        public MatchOutcome Outcome { get; set; }

        public Guid TeamID_Home { get; set; }
        public Guid TeamID_Away { get; set; }
        public Guid LeagueID { get; set; }

        public Team Team_Home { get; set; }
        public Team Team_Away { get; set; }
        public League League { get; set; }
    }
}