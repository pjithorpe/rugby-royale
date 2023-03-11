using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_FinalWhistle : IMatchEventType
    {
        const string _name = "Final Whistle";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "That's the final whistle!" };
    }
}