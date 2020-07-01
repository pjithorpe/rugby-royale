using System;

namespace RugbyRoyale.Entities.Model
{
    public class User
    {
        public string UserID { get; set; }

        public Team Team { get; set; }
        public League League { get; set; }
    }
}