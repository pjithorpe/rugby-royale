using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_KnockOn : IMatchEventType
    {
        const string _name = "Knock On";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();
    }
}