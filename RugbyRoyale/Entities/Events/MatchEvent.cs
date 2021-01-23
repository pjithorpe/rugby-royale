using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public abstract class MatchEvent
    {
        public Guid MatchID { get; set; }
        public int Minute { get; set; }
        public abstract string Name { get; }

        private Random randomGenerator = new Random();

        public virtual List<string> EventMessages => new List<string>()
        {
            $"A {Name} happened.",
            $"Oh look, a {Name}!"
        };

        public MatchEvent(Guid matchID, int minute)
        {
            MatchID = matchID;
            Minute = minute;
        }

        public virtual string GetRandomEventMessage()
        {
            return EventMessages[randomGenerator.Next(EventMessages.Count)];
        }
    }
}