using RugbyRoyale.Entities.Events;
using System;

namespace RugbyRoyale.GameEngine
{
    public interface IClient
    {
        void OutputMatchEvent(MatchEvent matchEvent, Guid matchID);
    }
}