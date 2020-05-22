using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Model
{
    public class League
    {
        public Guid LeagueID { get; set; }
        public string Name_Long { get; set; }
        public string Name_Short { get; set; }

        public List<Team> Teams { get; set; }
    }
}