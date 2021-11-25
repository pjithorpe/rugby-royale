using RugbyRoyale.Entities.Events;
using RugbyRoyale.GameEngine;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RugbyRoyale.Console
{
    public class Client : IClient
    {
        public void OutputMatchEvent(MatchEvent matchEvent)
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