using Destructurama.Attributed;
using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Model
{
    public class User
    {
        // PKs
        public string UserID { get; set; }

        // Fields

        // FKs

        // Navigation properties
        public Team Team { get; set; }
        public League OwnedLeague { get; set; }
        
        [LogAsScalar]
        public ICollection<League> Leagues { get; set; }
    }
}