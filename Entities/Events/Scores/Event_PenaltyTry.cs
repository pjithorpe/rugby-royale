namespace RugbyRoyale.Entities.Events
{
    public class Event_PenaltyTry : MatchEvent, IScoreEvent
    {
        public Event_PenaltyTry(int minute) : base(minute)
        {
        }

        public int Points => 7;
    }
}