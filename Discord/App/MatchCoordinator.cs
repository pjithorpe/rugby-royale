using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Concurrency;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RugbyRoyale.Discord.App
{
    public sealed class MatchCoordinator
    {
        private ConcurrentHashSet<DiscordChannel> availableMatchChannels;
        private ConcurrentDictionary<DiscordMember, Guid> memberMatchPairings;
        private ConcurrentDictionary<Guid, DiscordChannel> matchChannelPairings;

        private MatchCoordinator()
        {
        }

        public void Initialise(IEnumerable<DiscordChannel> matchThreadChannels)
        {
            availableMatchChannels = new ConcurrentHashSet<DiscordChannel>(matchThreadChannels);
            memberMatchPairings = new ConcurrentDictionary<DiscordMember, Guid>();
            matchChannelPairings = new ConcurrentDictionary<Guid, DiscordChannel>();
        }

        public bool TryAddMatch(Guid matchID, DiscordMember home, DiscordMember away)
        {
            // Check if there is a free match channel
            if (availableMatchChannels.Count > 0)
            {
                // Check that neither player is already in a game
                if (!memberMatchPairings.ContainsKey(home) && !memberMatchPairings.ContainsKey(away))
                {
                    DiscordChannel matchChannel = availableMatchChannels.First();
                    if (!availableMatchChannels.TryRemove(matchChannel)) return false;

                    matchChannelPairings[matchID] = matchChannel;
                    memberMatchPairings[home] = matchID;
                    memberMatchPairings[away] = matchID;
                }
                return true;
            }
            return false;
        }

        public bool TryRemoveMatch(Guid matchID)
        {
            if (!matchChannelPairings.TryRemove(matchID, out DiscordChannel freedChannel))
            {
                return false;
            }
            availableMatchChannels.Add(freedChannel);

            foreach (DiscordMember member in memberMatchPairings.Keys)
            {
                if (memberMatchPairings[member] == matchID)
                {
                    if (memberMatchPairings.TryRemove(member, out _))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public DiscordChannel GetMatchChannel(Guid matchID) => matchChannelPairings[matchID];

        public bool CheckMatchInProgress(Guid matchID) => matchChannelPairings.ContainsKey(matchID);

        private static readonly Lazy<MatchCoordinator> lazy = new Lazy<MatchCoordinator>(() => new MatchCoordinator());

        public static MatchCoordinator GetCoordinator()
        {
            return lazy.Value;
        }
    }
}