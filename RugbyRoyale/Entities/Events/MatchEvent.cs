using System;
using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public abstract class MatchEvent
    {
        public Guid MatchID { get; set; }
        public int Second { get; set; }
        public int Minute { get { return Second / 60; } }
        public virtual bool IsHalting { get; set; }
        public abstract string Name { get; }

        private Random randomGenerator = new Random();

        public virtual List<string> EventMessages => new List<string>() { $"{Name}." };

        public MatchEvent(Guid matchID, int second)
        {
            MatchID = matchID;
            Second = second;

            IsHalting = false;
        }

        public virtual string GetRandomEventMessage() => EventMessages[randomGenerator.Next(EventMessages.Count)];
    }
}