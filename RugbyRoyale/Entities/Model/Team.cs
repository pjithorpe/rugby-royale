using Destructurama.Attributed;
using System;

namespace RugbyRoyale.Entities.Model
{
    public class Team
    {
        // PKs
        public Guid TeamID { get; set; }

        // Fields
        public string Name_Long { get; set; }
        public string Name_Short { get; set; }
        public string Name_Abbreviated { get; set; }
        public string Colour_Primary { get; set; }
        public string Colour_Secondary { get; set; }
        public string Colour_Tertiary { get; set; }

        // FKs
        public string UserID { get; set; }

        // Navigation properties
        [NotLogged]
        public User User { get; set; }
        [NotLogged]
        public Teamsheet Teamsheet { get; set; }
    }
}