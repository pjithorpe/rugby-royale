using System;

namespace RugbyRoyale.Entities.Model
{
    public class LeagueUser
    {
        public Guid LeagueID { get; set; }
        public string UserID { get; set; }

        public League League { get; set; }
        public User User { get; set; }
    }
}