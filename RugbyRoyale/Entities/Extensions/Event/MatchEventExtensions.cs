using RugbyRoyale.Entities.Events;
using System;

namespace RugbyRoyale.Entities.Extensions.Event
{
    public static class MatchEventExtensions
    {
        /// <summary>
        /// Selects a description that fits this event at random from its list.
        /// </summary>
        public static string GetEventMessage(this MatchEvent matchEvent, Random randomGenerator)
        {
            int randomIndex = randomGenerator.Next(matchEvent.EventType.EventMessages.Count);
            return matchEvent.EventType.EventMessages[randomIndex];
        }
    }
}
