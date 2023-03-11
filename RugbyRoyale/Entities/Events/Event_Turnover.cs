using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Turnover : IMatchEventType
    {
        const string _name = "Turnover";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The ball has been turned over." };
    }
}