using DSharpPlus.Entities;
using RugbyRoyale.Entities.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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

        public bool TryAddLeagueAdvert(ulong messageID, Guid leagueID)
        {
            return advertLeagueMappings.TryAdd(messageID, leagueID);
        }

        public bool TryRemoveLeagueAdvert(ulong messageID)
        {
            return advertLeagueMappings.TryRemove(messageID, out _);
        }

        public Guid GetAdvertisedLeagueID(DiscordMessage message) => advertLeagueMappings[message.Id];

        public bool CheckMessageIsCurrentLeagueAdvert(DiscordMessage message) => advertLeagueMappings.ContainsKey(message.Id);

        private static readonly Lazy<MessageTracker> lazy = new Lazy<MessageTracker>(() => new MessageTracker());

        public static MessageTracker GetMessageTracker()
        {
            return lazy.Value;
        }
    }
}