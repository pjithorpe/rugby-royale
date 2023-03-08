using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_FinalWhistle : IMatchEventType
    {
        const string _name = "Final Whistle";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();
    }
}