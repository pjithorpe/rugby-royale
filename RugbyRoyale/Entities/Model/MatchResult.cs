using Destructurama.Attributed;
using RugbyRoyale.Entities.Enums;
using System;

namespace RugbyRoyale.Entities.Model
{
    public class MatchResult
    {
        // PKs
        public Guid MatchResultID { get; set; }

        // Fields
        public MatchOutcome Outcome { get; set; }

        // FKs
        public Guid Team_HomeID { get; set; }
        public Guid Team_AwayID { get; set; }
        public Guid LeagueID { get; set; }

        // Navigation properties
        [NotLogged]
        public Team Team_Home { get; set; }
        [NotLogged]
        public Team Team_Away { get; set; }
        [NotLogged]
        public League League { get; set; }
    }
}