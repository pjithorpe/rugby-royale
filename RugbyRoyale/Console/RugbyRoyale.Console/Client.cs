using RugbyRoyale.Entities.Events;
using RugbyRoyale.Entities.Extensions.Event;
using RugbyRoyale.GameEngine;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Console
{
    public class Client : IClient
    {
        private Random randomGenerator = new();

        public void OutputMatchEvent(MatchEvent matchEvent, Guid matchID)
        {
            try
            {
                System.Console.WriteLine(FormatMatchEventMessage(matchEvent));
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while outputting match event {@MatchEvent}.", matchEvent);
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