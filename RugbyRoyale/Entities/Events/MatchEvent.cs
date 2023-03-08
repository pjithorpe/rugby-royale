using System;

namespace RugbyRoyale.Entities.Events
{
    public sealed class MatchEvent
    {
        public int Second { get; }
        public int Minute
        {
            get
            {
                return (Second / 60) + 1;
            }
        }
        public int LocationXMetres { get; }
        public int LocationYMetres { get; }
        public IMatchEventType EventType { get; set; }

        public MatchEvent(IMatchEventType eventType, int second)
        {
            if (second < 0) throw new ArgumentOutOfRangeException(nameof(second));

            Second = second;
            EventType = eventType;
        }
    }
}