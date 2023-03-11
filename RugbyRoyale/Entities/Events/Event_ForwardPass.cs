using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_ForwardPass : IMatchEventType
    {
        const string _name = "Forward Pass";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The referee has spotted a forward pass." };
    }
}