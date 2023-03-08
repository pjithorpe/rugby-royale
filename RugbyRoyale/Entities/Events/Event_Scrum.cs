using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Scrum : IMatchEventType
    {
        const string _name = "Scrum";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();
    }
}