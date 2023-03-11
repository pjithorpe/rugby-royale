using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_DropOut : IMatchEventType
    {
        const string _name = "Drop Out";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "A goal line drop-out." };
    }
}