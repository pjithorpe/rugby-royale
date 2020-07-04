using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Model
{
    public class User
    {
        public string UserID { get; set; }

        public Team Team { get; set; }
        public League League { get; set; }

        public ICollection<LeagueUser> LeagueUsers { get; set; }
    }
}