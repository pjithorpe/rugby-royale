using System;

namespace RugbyRoyale.Entities.Model
{
    public class Team
    {
        public Guid TeamID { get; set; }
        public string Name_Long { get; set; }
        public string Name_Short { get; set; }
        public string Name_Abbreviated { get; set; }
        public string Colour_Primary { get; set; }
        public string Colour_Secondary { get; set; }
        public string Colour_Tertiary { get; set; }

        public string UserID { get; set; }
        public Guid LeagueID { get; set; }

        public User User { get; set; }
        public League League { get; set; }
        public Teamsheet Teamsheet { get; set; }
    }
}