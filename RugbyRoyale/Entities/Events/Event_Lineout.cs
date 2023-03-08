using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Lineout : IMatchEventType
    {
        const string _name = "Lineout";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();
    }
}