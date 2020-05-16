namespace RugbyRoyale.Entities.Events
{
    public class Event_Conversion : MatchEvent, IScoreEvent
    {
        public Event_Conversion(int minute) : base(minute)
        {
        }

        public int Points => 2;
        public bool Successful { get; set; }
    }
}