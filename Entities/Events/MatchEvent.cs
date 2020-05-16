namespace RugbyRoyale.Entities.Events
{
    public abstract class MatchEvent
    {
        public int Minute { get; set; }

        public MatchEvent(int minute)
        {
            Minute = minute;
        }
    }
}