namespace RugbyRoyale.Entities.Events
{
    public interface IScoreEvent
    {
        bool Successful { get; set; }
        int Points { get; }
    }
}