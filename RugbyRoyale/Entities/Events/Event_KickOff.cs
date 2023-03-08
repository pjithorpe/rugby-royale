using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_KickOff : IMatchEventType
    {
        const string _name = "Kick Off";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();
    }
}