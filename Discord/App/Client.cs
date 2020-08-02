﻿using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using RugbyRoyale.Entities.Events;
using RugbyRoyale.GameEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App
{
    public class Client : IClient
    {
        ILogger log;
        private MatchCoordinator coordinator;

        public Client(ILoggerProvider loggerProvider)
        {
            log = loggerProvider.CreateLogger("Client");
            coordinator = MatchCoordinator.GetCoordinator();
        }

        public void OutputMatchEvent(MatchEvent matchEvent)
        {
            try
            {
                DiscordChannel channel = coordinator.GetMatchChannel(matchEvent.MatchID);
                Task.Run(() => channel.SendMessageAsync(FormatMatchEventMessage(matchEvent)));
            }
            catch (Exception e)
            {
                log.LogError(e, "Error while outputting match event.");
            }
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