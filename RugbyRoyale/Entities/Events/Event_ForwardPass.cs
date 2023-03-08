using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_ForwardPass : IMatchEventType
    {
        const string _name = "Forward Pass";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();
    }
}