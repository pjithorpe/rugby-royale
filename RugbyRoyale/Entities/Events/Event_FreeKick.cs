using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_FreeKick : IMatchEventType
    {
        const string _name = "Free Kick";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The referee gives a short-arm penalty." };
    }
}