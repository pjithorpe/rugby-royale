using RugbyRoyale.Entities.Enums;
using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Model
{
    public class League
    {
        public Guid LeagueID { get; set; }
        public string Name_Long { get; set; }
        public string Name_Short { get; set; }
        public LeagueType LeagueType { get; set; }
        public string Colour { get; set; }
        public bool HasStarted { get; set; }

        public string UserID { get; set; }

        public User User { get; set; }
        public List<Team> Teams { get; set; }
    }
}