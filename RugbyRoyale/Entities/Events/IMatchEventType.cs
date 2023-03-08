using System.Collections.Generic;

namespace RugbyRoyale.Entities.Events
{
    public interface IMatchEventType
    {
        string DisplayName { get; }
        string[] EventMessages { get; }
    }
}