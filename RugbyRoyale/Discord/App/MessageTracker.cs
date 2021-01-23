using DSharpPlus.Entities;
using System;
using System.Collections.Concurrent;

namespace RugbyRoyale.Discord.App
{
    public sealed class MessageTracker
    {
        private ConcurrentDictionary<ulong, Guid> advertLeagueMappings;

        private MessageTracker()
        {
        }

        public void Initialise()
        {
            advertLeagueMappings = new ConcurrentDictionary<ulong, Guid>();
        }

        public bool TryAddLeagueAdvert(ulong messageID, Guid leagueID) => advertLeagueMappings.TryAdd(messageID, leagueID);

        public bool TryRemoveLeagueAdvert(ulong messageID) => advertLeagueMappings.TryRemove(messageID, out _);

        public bool TryGetAdvertisedLeagueID(DiscordMessage message, out Guid leagueID) => advertLeagueMappings.TryGetValue(message.Id, out leagueID);

        public bool CheckMessageIsCurrentLeagueAdvert(DiscordMessage message) => advertLeagueMappings.ContainsKey(message.Id);

        private static readonly Lazy<MessageTracker> lazy = new Lazy<MessageTracker>(() => new MessageTracker());

        public static MessageTracker GetMessageTracker() => lazy.Value;
    }
}