using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Restart : IMatchEventType
    {
        const string _name = "Restart";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "Play is restarted with a kick from half way." };
    }
}