using System;

namespace RugbyRoyale.Entities.Events
{
    public class Event_Conversion : MatchEvent, IScoreEvent
    {
        public Event_Conversion(Guid matchID, int minute) : base(matchID, minute)
        {
        }

        public override string Name { get => "Conversion Attempt"; }

        public string Abbreviation => "CON";
        public int Points => 2;
        public bool Successful { get; set; }
    }
}