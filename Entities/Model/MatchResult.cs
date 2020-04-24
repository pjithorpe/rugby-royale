using RugbyRoyale.Entities.Enums;
using System;

namespace RugbyRoyale.Entities.Model
{
    public class MatchResult
    {
        public Guid ResultID { get; set; }
        public Guid TeamID_Home { get; set; }
        public Guid TeamID_Away { get; set; }
        public MatchOutcome Outcome { get; set; }

        public Team Team { get; set; }
    }
}
