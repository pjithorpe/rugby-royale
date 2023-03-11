using System.Collections.Generic;
using RugbyRoyale.Entities.Enums;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyAwarded : IMatchEventType
    {
        const string _name = "Penalty Awarded";
        public string DisplayName { get => _name; }
        public IList<string> EventMessages => new string[] { "The referee blows his whistle for a penalty." };

        public PenaltyOffence Offence { get; }

        public Event_PenaltyAwarded(PenaltyOffence penaltyOffence)
        {
            Offence = penaltyOffence;
        }
    }
}