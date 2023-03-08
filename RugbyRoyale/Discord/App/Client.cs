using DSharpPlus.Entities;
using RugbyRoyale.Entities.Events;
using RugbyRoyale.Entities.Extensions.Event;
using RugbyRoyale.GameEngine;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App
{
    public class Client : IClient
    {
        private MatchCoordinator coordinator;
        private Random randomGenerator;

        public Client()
        {
            coordinator = MatchCoordinator.GetCoordinator();
            randomGenerator = new Random();
        }

        public void OutputMatchEvent(MatchEvent matchEvent, Guid matchID)
        {
            try
            {
                DiscordChannel channel = coordinator.GetMatchChannel(matchID);
                Task.Run(() => channel.SendMessageAsync(FormatMatchEventMessage(matchEvent)));
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while outputting match event.");
            }
        }

        private string FormatMatchEventMessage(MatchEvent matchEvent)
        {
            var eventInfo = new List<string>() { matchEvent.Minute + "'" };

            if (matchEvent.EventType is IScoreEvent scoreEvent)
            {
                eventInfo.Add("**" + scoreEvent.Abbreviation + "**");
            }
            else
            {
                eventInfo.Add("**" + matchEvent.EventType.DisplayName + "**");
            }

            eventInfo.Add(matchEvent.GetEventMessage(randomGenerator));

            return string.Join(" ", eventInfo);
        }
    }
}