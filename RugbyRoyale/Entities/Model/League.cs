using Destructurama.Attributed;
using RugbyRoyale.Entities.Enums;
using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Model
{
    public class League
    {
        // PKs
        public Guid LeagueID { get; set; }

        // Fields
        public string Name_Long { get; set; }
        public string Name_Short { get; set; }
        public LeagueType LeagueType { get; set; }
        public bool HasStarted { get; set; }
        public int DaysPerRound { get; set; }
        public int Size { get; set; }
        public int Size_Min { get; set; }
        public int Size_Max { get; set; }

        // FKs
        public string OwnerID { get; set; }

        // Navigation properties
        [NotLogged]
        public User Owner { get; set; }
        
        [LogAsScalar]
        public ICollection<User> Users { get; set; }
    }
}