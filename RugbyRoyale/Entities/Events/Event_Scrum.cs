using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Scrum : IMatchEventType
    {
        const string _name = "Scrum";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The packs come together for a scrum." };
    }
}