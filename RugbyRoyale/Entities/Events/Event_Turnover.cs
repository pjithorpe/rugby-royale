using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Turnover : IMatchEventType
    {
        const string _name = "Turnover";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();
    }
}