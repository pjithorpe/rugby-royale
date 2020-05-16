namespace RugbyRoyale.Entities.Events
{
    public class Event_DropGoal : MatchEvent, IScoreEvent
    {
        public Event_DropGoal(int minute) : base(minute)
        {
        }

        public int Points => 3;
    }
}