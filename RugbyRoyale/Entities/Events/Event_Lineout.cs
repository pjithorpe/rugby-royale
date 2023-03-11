using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Lineout : IMatchEventType
    {
        const string _name = "Lineout";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "Time for a lineout." };
    }
}