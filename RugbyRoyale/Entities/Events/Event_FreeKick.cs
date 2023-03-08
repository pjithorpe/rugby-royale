using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_FreeKick : IMatchEventType
    {
        const string _name = "Free Kick";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();
    }
}