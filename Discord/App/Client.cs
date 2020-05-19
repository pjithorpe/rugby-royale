using DSharpPlus.Entities;
using RugbyRoyale.Entities.Events;
using RugbyRoyale.GameEngine;
using System.Collections.Generic;

namespace RugbyRoyale.Discord.App
{
    public class Client : IClient
    {
        private MatchCoordinator coordinator;

        public Client()
        {
            coordinator = MatchCoordinator.GetCoordinator();
        }

        public void OutputMatchEvent(MatchEvent matchEvent)
        {
            DiscordChannel channel = coordinator.GetMatchChannel(matchEvent.MatchID);
            channel.SendMessageAsync(FormatMatchEventMessage(matchEvent));
        }

        private string FormatMatchEventMessage(MatchEvent matchEvent)
        {
            var eventInfo = new List<string>() { matchEvent.Minute + "'" };

            if (matchEvent is IScoreEvent scoreEvent)
            {
                eventInfo.Add("**" + scoreEvent.Abbreviation + "**");
            }
            else
            {
                eventInfo.Add("**" + matchEvent.Name + "**");
            }

            eventInfo.Add(matchEvent.GetRandomEventMessage());

            return string.Join(" ", eventInfo);
        }
    }
}