namespace RugbyRoyale.Entities.Events
{
    public interface IScoreEvent
    {
        string Abbreviation { get; }
        int Points { get; }
        bool Successful { get; set; }
    }
}