using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Restart : IMatchEventType
    {
        const string _name = "Restart";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();
    }
}