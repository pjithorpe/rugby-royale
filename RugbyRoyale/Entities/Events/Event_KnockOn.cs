using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_KnockOn : IMatchEventType
    {
        const string _name = "Knock On";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The ball is knocked on." };
    }
}