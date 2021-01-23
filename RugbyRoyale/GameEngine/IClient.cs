using RugbyRoyale.Entities.Events;

namespace RugbyRoyale.GameEngine
{
    public interface IClient
    {
        void OutputMatchEvent(MatchEvent matchEvent);
    }
}