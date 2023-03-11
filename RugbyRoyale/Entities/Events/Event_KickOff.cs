using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_KickOff : IMatchEventType
    {
        const string _name = "Kick Off";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The whistle is blown, and we're underway." };
    }
}