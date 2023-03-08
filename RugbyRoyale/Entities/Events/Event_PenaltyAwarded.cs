using RugbyRoyale.Entities.Enums;
using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyAwarded : IMatchEventType
    {
        const string _name = "Penalty Awarded";
        public string DisplayName { get => _name; }
        public string[] EventMessages => throw new NotImplementedException();

        public PenaltyOffence Offence { get; }

        public Event_PenaltyAwarded(PenaltyOffence penaltyOffence)
        {
            Offence = penaltyOffence;
        }
    }
}